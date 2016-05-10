using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Hosting;
using System.Web.Mvc;
using Telerik.Sitefinity.Frontend.Mvc.Controllers;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers.Attributes;

namespace MvcCrawler.Server.Mvc.Infrastructure
{
    /// <summary>
    /// Filter that registers the extended precompiled view engine when that is required. 
    /// </summary>
    /// <seealso cref="System.Web.Mvc.ActionFilterAttribute" />
    public class ViewEngineFilterAttribute : ActionFilterAttribute
    {
        public ViewEngineFilterAttribute()
        {
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Controller controller = filterContext.Controller as Controller;
            if (controller == null)
            {
                return;
            }

            if (filterContext.HttpContext.Request.Headers.GetValues(CrawlerRequestConstants.HeaderName) != null)
            {
                this.RegisterPrecompiledViewEngines(this.ControllerContainerAssemblies, (Controller)filterContext.Controller, typeof(ExtendedPrecompiledViewEngine));
            }
            else
            {
                this.RemoveExtendedPrecompiledMvcEngineWrapper(((Controller)filterContext.Controller).ViewEngineCollection);
            }

            base.OnActionExecuting(filterContext);
        }

        public IEnumerable<Assembly> ControllerContainerAssemblies
        {
            get
            {
                if (ViewEngineFilterAttribute.controllerContainerAssemblies == null)
                {
                    lock (ViewEngineFilterAttribute.ControllerContainerAssembliesLock)
                    {
                        if (ViewEngineFilterAttribute.controllerContainerAssemblies == null)
                        {
                            ViewEngineFilterAttribute.controllerContainerAssemblies = this.RetrieveAssemblies();
                        }
                    }
                }

                return ViewEngineFilterAttribute.controllerContainerAssemblies;
            }

            private set
            {
                lock (ViewEngineFilterAttribute.ControllerContainerAssembliesLock)
                {
                    ViewEngineFilterAttribute.controllerContainerAssemblies = value;
                }
            }
        }

        public IEnumerable<Assembly> RetrieveAssemblies()
        {
            var assemblyFileNames = this.RetrieveAssembliesFileNames().ToArray();
            var result = new List<Assembly>();

            foreach (var assemblyFileName in assemblyFileNames)
            {
                if (this.IsControllerContainer(assemblyFileName))
                {
                    var assembly = this.LoadAssembly(assemblyFileName);
                    this.InitializeControllerContainer(assembly);

                    result.Add(assembly);
                }

                if (this.IsMarkedAssembly<ResourcePackageAttribute>(assemblyFileName))
                {
                    result.Add(this.LoadAssembly(assemblyFileName));
                }
            }

            return result;
        }

        protected virtual IEnumerable<string> RetrieveAssembliesFileNames()
        {
            var controllerAssemblyPath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "bin");
            return Directory.EnumerateFiles(controllerAssemblyPath, "*.dll", SearchOption.TopDirectoryOnly);
        }

        protected virtual bool IsControllerContainer(string assemblyFileName)
        {
            return this.IsMarkedAssembly<ControllerContainerAttribute>(assemblyFileName);
        }

        protected virtual Assembly LoadAssembly(string assemblyFileName)
        {
            return Assembly.LoadFrom(assemblyFileName);
        }

        protected virtual void InitializeControllerContainer(Assembly container)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            var containerAttribute = container.GetCustomAttributes(false).Single(attr => attr.GetType().AssemblyQualifiedName == typeof(ControllerContainerAttribute).AssemblyQualifiedName) as ControllerContainerAttribute;

            if (containerAttribute.InitializationType == null || containerAttribute.InitializationMethod.IsNullOrWhitespace())
                return;

            var initializationMethod = containerAttribute.InitializationType.GetMethod(containerAttribute.InitializationMethod);
            initializationMethod.Invoke(null, null);
        }

        private bool IsMarkedAssembly<TAttribute>(string assemblyFileName)
            where TAttribute : Attribute
        {
            if (assemblyFileName == null)
                return false;

            bool result;
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += this.CurrentDomain_ReflectionOnlyAssemblyResolve;
            try
            {
                try
                {
                    var reflOnlyAssembly = Assembly.ReflectionOnlyLoadFrom(assemblyFileName);

                    result = reflOnlyAssembly != null &&
                            reflOnlyAssembly.GetCustomAttributesData()
                                .Any(d => d.Constructor.DeclaringType.AssemblyQualifiedName == typeof(TAttribute).AssemblyQualifiedName);
                }
                catch (IOException)
                {
                    // We might not be able to load some .DLL files as .NET assemblies. Those files cannot contain controllers.
                    result = false;
                }
                catch (BadImageFormatException)
                {
                    result = false;
                }
            }
            finally
            {
                AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= this.CurrentDomain_ReflectionOnlyAssemblyResolve;
            }

            return result;
        }

        private Assembly CurrentDomain_ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
        {
            var assWithPolicy = AppDomain.CurrentDomain.ApplyPolicy(args.Name);

            return Assembly.ReflectionOnlyLoad(assWithPolicy);
        }

        private void RegisterPrecompiledViewEngines(IEnumerable<Assembly> assemblies, Controller controller, Type viewEngineType)
        {
            MethodInfo getEnhanceAttributeMethod = typeof(FrontendControllerFactory).GetMethod("GetEnhanceAttribute", BindingFlags.NonPublic | BindingFlags.Static);
            MethodInfo getControllerPathTransformationsMethod = typeof(FrontendControllerFactory).GetMethod("GetControllerPathTransformations", BindingFlags.NonPublic | BindingFlags.Static);
            MethodInfo getViewEngineMethod = typeof(ControllerExtensions).GetMethod("GetViewEngine", BindingFlags.NonPublic | BindingFlags.Static);

            EnhanceViewEnginesAttribute enhanceAttribute = getEnhanceAttributeMethod.Invoke(
                    ControllerBuilder.Current.GetControllerFactory(),
                    new object[] { controller.GetType() })
                as EnhanceViewEnginesAttribute;

            IList<Func<string, string>> pathTransformations = (IList<Func<string, string>>)getControllerPathTransformationsMethod.Invoke(
                    ControllerBuilder.Current.GetControllerFactory(),
                    new object[] { controller, enhanceAttribute.VirtualPath });

            var precompiledAssemblies = assemblies.Where(a => a.GetCustomAttribute<ControllerContainerAttribute>() != null).Select(a => new PrecompiledViewAssemblyWrapper(a, null)).ToArray();
            if (precompiledAssemblies.Length > 0)
            {
                IViewEngine transformedViewEngine = getViewEngineMethod.Invoke(
                        new { },
                        new object[] { Activator.CreateInstance(viewEngineType, precompiledAssemblies) as IViewEngine, pathTransformations })
                    as IViewEngine;

                if (transformedViewEngine != null)
                {
                    controller.ViewEngineCollection.Insert(0, transformedViewEngine);
                }
            }

            var precompiledResourcePackages = this.PrecompiledResourcePackages(assemblies);
            foreach (var package in precompiledResourcePackages.Keys)
            {
                if (package == null)
                    continue;

                var packageAssemblies = precompiledResourcePackages[package];
                if (packageAssemblies.Count > 0)
                {
                    IViewEngine transformedViewEngine = getViewEngineMethod.Invoke(
                        new { },
                        new object[] { Activator.CreateInstance(viewEngineType, packageAssemblies, null, package) as IViewEngine, pathTransformations })
                    as IViewEngine;

                    if (transformedViewEngine != null)
                    {
                        controller.ViewEngineCollection.Insert(0, transformedViewEngine);
                    }
                }
            }
        }

        private string AssemblyPackage(Assembly assembly)
        {
            var attribute = assembly.GetCustomAttribute<ResourcePackageAttribute>();
            if (attribute == null)
                return null;
            else
                return attribute.Name;
        }

        private Dictionary<string, List<PrecompiledViewAssemblyWrapper>> PrecompiledResourcePackages(IEnumerable<Assembly> assemblies)
        {
            var precompiledViewAssemblies = new Dictionary<string, List<PrecompiledViewAssemblyWrapper>>();
            foreach (var assembly in assemblies)
            {
                var package = this.AssemblyPackage(assembly);
                if (package == null)
                    continue;

                if (!precompiledViewAssemblies.ContainsKey(package))
                    precompiledViewAssemblies[package] = new List<PrecompiledViewAssemblyWrapper>();

                precompiledViewAssemblies[package].Add(new PrecompiledViewAssemblyWrapper(assembly, package));
            }

            return precompiledViewAssemblies;
        }

        private void RemoveExtendedPrecompiledMvcEngineWrapper(ViewEngineCollection viewEngines)
        {
            var engine = viewEngines.FirstOrDefault(ve => ve.GetType() == typeof(ExtendedPrecompiledViewEngine));
            while (engine != null)
            {
                viewEngines.Remove(engine);
                engine = viewEngines.FirstOrDefault(ve => ve.GetType() == typeof(ExtendedPrecompiledViewEngine));
            }
        }

        private static IEnumerable<Assembly> controllerContainerAssemblies;
        private static readonly object ControllerContainerAssembliesLock = new object();
    }
}
