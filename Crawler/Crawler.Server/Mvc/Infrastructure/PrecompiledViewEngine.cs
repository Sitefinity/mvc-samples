using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Telerik.Sitefinity.Frontend.Mvc.Controllers;

namespace Crawler.Server.Mvc.Infrastructure
{
    /// <summary>
    /// Precompiled Razor view engine that enables registration of usage of both precompiled and not precompiled views.
    /// </summary>
    /// <seealso cref="Telerik.Sitefinity.Frontend.Mvc.Controllers.CompositePrecompiledMvcEngineWrapper" />
    /// <seealso cref="Telerik.Sitefinity.Frontend.Mvc.Controllers.ICompositePrecompiledMvcEngineWrapper" />
    internal class PrecompiledViewEngine : CompositePrecompiledMvcEngineWrapper, ICompositePrecompiledMvcEngineWrapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrecompiledViewEngine"/> class.
        /// </summary>
        /// <param name="viewAssemblies">The view assemblies.</param>
        public PrecompiledViewEngine(params PrecompiledViewAssemblyWrapper[] viewAssemblies)
            : this(viewAssemblies, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrecompiledViewEngine"/> class.
        /// </summary>
        /// <param name="viewAssemblies">The view assemblies.</param>
        /// <param name="viewPageActivator">The view page activator.</param>
        public PrecompiledViewEngine(IEnumerable<PrecompiledViewAssemblyWrapper> viewAssemblies, IViewPageActivator viewPageActivator)
            : this(viewAssemblies, viewPageActivator, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrecompiledViewEngine"/> class.
        /// </summary>
        /// <param name="viewAssemblies">The view assemblies.</param>
        /// <param name="viewPageActivator">The view page activator.</param>
        /// <param name="packageName">Name of the package.</param>
        public PrecompiledViewEngine(IEnumerable<PrecompiledViewAssemblyWrapper> viewAssemblies, IViewPageActivator viewPageActivator, string packageName)
            : base(viewAssemblies, viewPageActivator, packageName)
        {
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public override ICompositePrecompiledMvcEngineWrapper Clone()
        {
            return new PrecompiledViewEngine(this.PrecompiledAssemblies, null, this.PackageName);
        }

        /// <summary>
        /// Checks whether the view exists.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="virtualPath">The view path.</param>
        /// <returns></returns>
        protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            bool isPrecompiled = base.FileExists(controllerContext, virtualPath);
            if (string.IsNullOrEmpty(virtualPath))
            {
                return isPrecompiled;
            }

            var viewInfo = this.GetViewInfo(controllerContext, virtualPath, isPrecompiled);
            this.RegisterCrawlItem(viewInfo);

            return isPrecompiled;
        }

        /// <summary>
        /// Creates the view.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="viewPath">The view path.</param>
        /// <param name="masterPath">The master path.</param>
        /// <returns></returns>
        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            var viewInfo = this.GetViewInfo(controllerContext, masterPath, true);
            this.RegisterCrawlItem(viewInfo);

            return base.CreateView(controllerContext, viewPath, masterPath);
        }

        /// <summary>
        /// Creates the partial view.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="partialPath">The partial path.</param>
        /// <returns></returns>
        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            var viewInfo = this.GetViewInfo(controllerContext, partialPath, true);
            this.RegisterCrawlItem(viewInfo);

            return base.CreatePartialView(controllerContext, partialPath);
        }

        private void RegisterCrawlItem(WidgetViewInfo viewInfo)
        {
            var viewName = viewInfo.ViewName.Split(new[] { PathSeparatorChar }).Last();
            if (viewName.EndsWith(MobileRazorViewSuffix) || string.IsNullOrEmpty(viewName))
            {
                return;
            }

            PageVisitInfoBuilder.AddItem(viewInfo);
        }

        private string GetViewNameFromVirtualPath(string virtualPath)
        {
            return virtualPath.Split(new[] { PathSeparatorChar }).Last();
        }

        private WidgetViewInfo GetViewInfo(ControllerContext controllerContext, string virtualPath, bool isPrecompiled)
        {
            string viewName = this.GetViewNameFromVirtualPath(virtualPath);
            string widgetControllerName = controllerContext.Controller.GetType().Name;
            string widgetName = widgetControllerName.Replace(ControllerSuffix, string.Empty);
            WidgetViewInfo viewInfo = new WidgetViewInfo()
            {
                WidgetName = widgetName,
                Url = controllerContext.HttpContext.Request.Url.AbsoluteUri,
                ViewName = viewName,
                ViewPath = virtualPath,
                IsPrecompiled = isPrecompiled
            };

            return viewInfo;
        }

        private const char PathSeparatorChar = '/';
        private const string MobileRazorViewSuffix = "Mobile.cshtml";
        private const string ControllerSuffix = "Controller";
    }
}