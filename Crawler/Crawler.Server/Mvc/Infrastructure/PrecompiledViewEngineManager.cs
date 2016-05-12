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

namespace Crawler.Server.Mvc.Infrastructure
{
    /// <summary>
    /// Class that provides methods for registering and removing the crawler precompiled view engine.
    /// </summary>
    internal class PrecompiledViewEngineManager
    {
        /// <summary>
        /// Registers the precompiled view engine.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="precompiledViewEngineType">Type of the view engine.</param>
        public void RegisterPrecompiledViewEngine(Controller controller, Type precompiledViewEngineType)
        {
            // Get EnhanceViewEnginesAttribute
            MethodInfo getEnhanceAttributeMethod = typeof(FrontendControllerFactory).GetMethod(GetEnhanceAttributeMethodName, BindingFlags.NonPublic | BindingFlags.Static);
            EnhanceViewEnginesAttribute enhanceAttribute = getEnhanceAttributeMethod.Invoke(
                    ControllerBuilder.Current.GetControllerFactory(),
                    new object[] { controller.GetType() })
                as EnhanceViewEnginesAttribute;

            // Get controller path transformations
            MethodInfo getControllerPathTransformationsMethod = typeof(FrontendControllerFactory).GetMethod(GetControllerPathTransformationsMethodName, BindingFlags.NonPublic | BindingFlags.Static);
            IList<Func<string, string>> pathTransformations = (IList<Func<string, string>>)getControllerPathTransformationsMethod.Invoke(
                    ControllerBuilder.Current.GetControllerFactory(),
                    new object[] { controller, enhanceAttribute.VirtualPath });

            var precompiledAssemblies = this.ControllerContainerAssemblies
                .Where(a => a.GetCustomAttribute<ControllerContainerAttribute>() != null)
                .Select(a => new PrecompiledViewAssemblyWrapper(a, null))
                .ToArray();

            MethodInfo getViewEngineMethod = typeof(ControllerExtensions).GetMethod(GetViewEngineMethodName, BindingFlags.NonPublic | BindingFlags.Static);

            // Create transformed precompiled view engine with specified precompiled assemblies
            if (precompiledAssemblies.Length > 0)
            {
                IViewEngine transformedViewEngine = getViewEngineMethod.Invoke(
                        new { },
                        new object[] { Activator.CreateInstance(precompiledViewEngineType, precompiledAssemblies) as IViewEngine, pathTransformations })
                    as IViewEngine;

                if (transformedViewEngine != null)
                {
                    controller.ViewEngineCollection.Insert(0, transformedViewEngine);
                }
            }

            // Create transformed precompiled view engine with specified precompiled assemblies and resource packages
            var precompiledResourcePackages = this.GetPrecompiledResourcePackages(this.ControllerContainerAssemblies);
            foreach (var package in precompiledResourcePackages.Keys)
            {
                if (package == null)
                    continue;

                var packageAssemblies = precompiledResourcePackages[package];
                if (packageAssemblies.Count > 0)
                {
                    IViewEngine transformedViewEngine = getViewEngineMethod.Invoke(
                        new { },
                        new object[] { Activator.CreateInstance(precompiledViewEngineType, packageAssemblies, null, package) as IViewEngine, pathTransformations })
                    as IViewEngine;

                    if (transformedViewEngine != null)
                    {
                        controller.ViewEngineCollection.Insert(0, transformedViewEngine);
                    }
                }
            }
        }

        /// <summary>
        /// Removes all occurrences of the precompiled view engine.
        /// </summary>
        /// <param name="viewEngines">The view engines collection.</param>
        public void RemovePrecompiledViewEngine(ViewEngineCollection viewEngines)
        {
            var engine = viewEngines.FirstOrDefault(ve => ve.GetType() == typeof(PrecompiledViewEngine));
            while (engine != null)
            {
                viewEngines.Remove(engine);
                engine = viewEngines.FirstOrDefault(ve => ve.GetType() == typeof(PrecompiledViewEngine));
            }
        }

        private IEnumerable<Assembly> ControllerContainerAssemblies
        {
            get
            {
                if (PrecompiledViewEngineManager.controllerContainerAssemblies == null)
                {
                    lock (PrecompiledViewEngineManager.ControllerContainerAssembliesLock)
                    {
                        if (PrecompiledViewEngineManager.controllerContainerAssemblies == null)
                        {
                            PrecompiledViewEngineManager.controllerContainerAssemblies = this.RetrieveAssemblies();
                        }
                    }
                }

                return PrecompiledViewEngineManager.controllerContainerAssemblies;
            }

            set
            {
                lock (PrecompiledViewEngineManager.ControllerContainerAssembliesLock)
                {
                    PrecompiledViewEngineManager.controllerContainerAssemblies = value;
                }
            }
        }

        private IEnumerable<Assembly> RetrieveAssemblies()
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

        private IEnumerable<string> RetrieveAssembliesFileNames()
        {
            var controllerAssemblyPath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "bin");
            return Directory.EnumerateFiles(controllerAssemblyPath, "*.dll", SearchOption.TopDirectoryOnly);
        }

        private bool IsControllerContainer(string assemblyFileName)
        {
            return this.IsMarkedAssembly<ControllerContainerAttribute>(assemblyFileName);
        }

        private Assembly LoadAssembly(string assemblyFileName)
        {
            return Assembly.LoadFrom(assemblyFileName);
        }

        private void InitializeControllerContainer(Assembly container)
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

        private string AssemblyPackage(Assembly assembly)
        {
            var attribute = assembly.GetCustomAttribute<ResourcePackageAttribute>();
            if (attribute == null)
                return null;
            else
                return attribute.Name;
        }

        private Dictionary<string, List<PrecompiledViewAssemblyWrapper>> GetPrecompiledResourcePackages(IEnumerable<Assembly> assemblies)
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


        private static IEnumerable<Assembly> controllerContainerAssemblies;
        private static readonly object ControllerContainerAssembliesLock = new object();

        private const string GetEnhanceAttributeMethodName = "GetEnhanceAttribute";
        private const string GetControllerPathTransformationsMethodName = "GetControllerPathTransformations";
        private const string GetViewEngineMethodName = "GetViewEngine";
    }
}
