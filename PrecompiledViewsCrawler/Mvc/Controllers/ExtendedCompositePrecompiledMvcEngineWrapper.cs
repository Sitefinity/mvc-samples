using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PrecompiledViewsCrawler.Mvc.Models;
using PrecompiledViewsCrawler.Utilities;
using Telerik.Sitefinity.Frontend.Mvc.Controllers;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers;

namespace PrecompiledViewsCrawler.Mvc.Controllers
{
    internal class ExtendedCompositePrecompiledMvcEngineWrapper : CompositePrecompiledMvcEngineWrapper, ICompositePrecompiledMvcEngineWrapper
    {
        public ExtendedCompositePrecompiledMvcEngineWrapper(params PrecompiledViewAssemblyWrapper[] viewAssemblies)
            : this(viewAssemblies, null)
        {
        }

        public ExtendedCompositePrecompiledMvcEngineWrapper(IEnumerable<PrecompiledViewAssemblyWrapper> viewAssemblies, IViewPageActivator viewPageActivator)
            : this(viewAssemblies, viewPageActivator, string.Empty)
        {
        }

        public ExtendedCompositePrecompiledMvcEngineWrapper(IEnumerable<PrecompiledViewAssemblyWrapper> viewAssemblies, IViewPageActivator viewPageActivator, string packageName)
            : base(viewAssemblies, viewPageActivator, packageName)
        {
            this.crawlResultBuilder = ControllerModelFactory.GetModel<ICrawlResultBuilder>(this.GetType());
        }

        public override ICompositePrecompiledMvcEngineWrapper Clone()
        {
            return new ExtendedCompositePrecompiledMvcEngineWrapper(this.PrecompiledAssemblies, null, this.PackageName);
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

        private void RegisterCrawlItem(CrawlItemViewModel viewInfo)
        {
            var viewName = viewInfo.ViewName.Split(new[] { '/' }).Last();
            if (viewName.EndsWith("Mobile.cshtml") || string.IsNullOrEmpty(viewName))
            {
                return;
            }

            this.crawlResultBuilder.AddItem(viewInfo);
        }

        private string GetViewNameFromVirtualPath(string virtualPath)
        {
            return virtualPath.Split(new[] { '/' }).Last();
        }

        private CrawlItemViewModel GetViewInfo(ControllerContext controllerContext, string virtualPath, bool isPrecompiled)
        {
            string viewName = this.GetViewNameFromVirtualPath(virtualPath);
            string widgetName = controllerContext.Controller.GetType().Name.Replace("Controller", string.Empty);
            CrawlItemViewModel viewInfo = new CrawlItemViewModel()
            {
                WidgetName = widgetName,
                Url = controllerContext.HttpContext.Request.Url.AbsoluteUri,
                ViewName = viewName,
                ViewPath = virtualPath,
                IsPrecompiled = isPrecompiled
            };

            return viewInfo;
        }

        private readonly ICrawlResultBuilder crawlResultBuilder;
    }
}