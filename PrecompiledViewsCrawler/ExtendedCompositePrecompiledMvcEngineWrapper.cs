using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Newtonsoft.Json.Linq;
using PrecompiledViewsCrawler.Models;
using PrecompiledViewsCrawler.Utilities;
using Telerik.Sitefinity.Frontend.Mvc.Controllers;

namespace PrecompiledViewsCrawler
{
    internal class ExtendedCompositePrecompiledMvcEngineWrapper : CompositePrecompiledMvcEngineWrapper
    {
        public ExtendedCompositePrecompiledMvcEngineWrapper(params PrecompiledViewAssemblyWrapper[] viewAssemblies)
            : base(viewAssemblies)
        {
        }

        public ExtendedCompositePrecompiledMvcEngineWrapper(IEnumerable<PrecompiledViewAssemblyWrapper> viewAssemblies, IViewPageActivator viewPageActivator)
            : base(viewAssemblies, viewPageActivator)
        {
        }

        public ExtendedCompositePrecompiledMvcEngineWrapper(IEnumerable<PrecompiledViewAssemblyWrapper> viewAssemblies, IViewPageActivator viewPageActivator, string packageName)
            : base(viewAssemblies, viewPageActivator, packageName)
        {
            this.precompiledAssemblies = viewAssemblies.ToArray();
            this.packageName = packageName;
        }        

        protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            bool isPrecompiled = base.FileExists(controllerContext, virtualPath);

            CrawlItemViewModel viewInfo = new CrawlItemViewModel()
            {
                Url = controllerContext.HttpContext.Request.Url.AbsoluteUri,
                ViewPath = virtualPath,
                IsPrecompiled = isPrecompiled
            };

            this.AddViewInfoToLogger(viewInfo);

            return isPrecompiled;
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            CrawlItemViewModel viewInfo = new CrawlItemViewModel()
            {
                Url = controllerContext.HttpContext.Request.Url.AbsoluteUri,
                ViewPath = viewPath,
                IsPrecompiled = true
            };

            this.AddViewInfoToLogger(viewInfo);
            return base.CreateView(controllerContext, viewPath, masterPath);
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            CrawlItemViewModel viewInfo = new CrawlItemViewModel()
            {
                Url = controllerContext.HttpContext.Request.Url.AbsoluteUri,
                ViewPath = partialPath,
                IsPrecompiled = true
            };

            this.AddViewInfoToLogger(viewInfo);
            return base.CreatePartialView(controllerContext, partialPath);
        }

        private void AddViewInfoToLogger(CrawlItemViewModel viewInfo)
        {
            var viewModel = new CrawlItemViewModel();
            viewModel.Url = viewInfo.Url;
            viewModel.ViewPath = viewInfo.ViewPath;
            viewModel.IsPrecompiled = viewInfo.IsPrecompiled;

            var viewName = viewInfo.ViewPath.Split(new[] { '/' }).Last();
            if (viewName.EndsWith("Mobile.cshtml"))
            {
                return;
            }

            var result = new JObject();
            result.Add(new JProperty("url", viewInfo.Url));
            result.Add(new JProperty("viewPath", viewInfo.ViewPath));
            result.Add(new JProperty("isPrecompiled", viewInfo.IsPrecompiled));

            JsonLogger.AddToLog(result);

            CrawlResultBuilder.Add(viewModel);
        }

        private readonly PrecompiledViewAssemblyWrapper[] precompiledAssemblies;
        private readonly string packageName;
    }
}