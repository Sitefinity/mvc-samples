using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcCrawler.Server.Mvc.Models;
using Telerik.Sitefinity.Frontend.Mvc.Controllers;

namespace MvcCrawler.Server.Mvc.Infrastructure
{
    /// <summary>
    /// Precompiled Razor view engine that enables registration of usage of both precompiled and not precompiled views.
    /// </summary>
    /// <seealso cref="Telerik.Sitefinity.Frontend.Mvc.Controllers.CompositePrecompiledMvcEngineWrapper" />
    /// <seealso cref="Telerik.Sitefinity.Frontend.Mvc.Controllers.ICompositePrecompiledMvcEngineWrapper" />
    internal class ExtendedPrecompiledViewEngine : CompositePrecompiledMvcEngineWrapper, ICompositePrecompiledMvcEngineWrapper
    {
        public ExtendedPrecompiledViewEngine(params PrecompiledViewAssemblyWrapper[] viewAssemblies)
            : this(viewAssemblies, null)
        {
        }

        public ExtendedPrecompiledViewEngine(IEnumerable<PrecompiledViewAssemblyWrapper> viewAssemblies, IViewPageActivator viewPageActivator)
            : this(viewAssemblies, viewPageActivator, string.Empty)
        {
        }

        public ExtendedPrecompiledViewEngine(IEnumerable<PrecompiledViewAssemblyWrapper> viewAssemblies, IViewPageActivator viewPageActivator, string packageName)
            : base(viewAssemblies, viewPageActivator, packageName)
        {
        }

        public override ICompositePrecompiledMvcEngineWrapper Clone()
        {
            return new ExtendedPrecompiledViewEngine(this.PrecompiledAssemblies, null, this.PackageName);
        }

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

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            var viewInfo = this.GetViewInfo(controllerContext, masterPath, true);
            this.RegisterCrawlItem(viewInfo);

            return base.CreateView(controllerContext, viewPath, masterPath);
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            var viewInfo = this.GetViewInfo(controllerContext, partialPath, true);
            this.RegisterCrawlItem(viewInfo);

            return base.CreatePartialView(controllerContext, partialPath);
        }

        private void RegisterCrawlItem(WidgetViewInfo viewInfo)
        {
            var viewName = viewInfo.ViewName.Split(new[] { '/' }).Last();
            if (viewName.EndsWith("Mobile.cshtml") || string.IsNullOrEmpty(viewName))
            {
                return;
            }

            PageVisitInfoBuilder.AddItem(viewInfo);
        }

        private string GetViewNameFromVirtualPath(string virtualPath)
        {
            return virtualPath.Split(new[] { '/' }).Last();
        }

        private WidgetViewInfo GetViewInfo(ControllerContext controllerContext, string virtualPath, bool isPrecompiled)
        {
            string viewName = this.GetViewNameFromVirtualPath(virtualPath);
            string widgetName = controllerContext.Controller.GetType().Name.Replace("Controller", string.Empty);
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
    }
}