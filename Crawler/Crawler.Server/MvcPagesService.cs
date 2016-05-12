using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Crawler.Server.Cache;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Frontend.Mvc.Helpers;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers;
using Telerik.Sitefinity.Frontend.Mvc.Models;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Modules.Pages;
using Telerik.Sitefinity.Modules.Pages.Configuration;
using Telerik.Sitefinity.Mvc.Store;
using Telerik.Sitefinity.Pages.Model;
using Telerik.Sitefinity.Services;
using Telerik.Sitefinity.Web;

namespace Crawler.Server
{
    /// <summary>
    /// This class provides API for getting page information of live pages that contain MVC widgets
    /// </summary>
    public class MvcPagesService : IDisposable
    {
        public MvcPagesService()
        {
            SystemConfig systemConfig = Config.Get<SystemConfig>();
            OutputCacheElement cacheSettings = systemConfig.CacheSettings;

            this.SaveCacheSettings(cacheSettings);
        }

        /// <summary>
        /// Gets the page and widget URLs for all live pages that contain MVC widgets
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAllLiveHybridMvcPageUrls()
        {
            // Get all live pages containg MVC widgets
            var pageManager = PageManager.GetManager();
            var pages = this.GetAllLiveHybridMvcPages(pageManager);

            var pageUrls = new HashSet<string>();
            foreach (PageData page in pages)
            {
                PageNode pageNode = page.NavigationNode;

                var pageCultures = pageNode.AvailableCultures;

                // Take default page URL in case no cultures are specified
                string defaultPageUrl = UrlPath.ResolveAbsoluteUrl(pageNode.GetFullUrl());
                pageUrls.Add(defaultPageUrl);

                // Take all culture defined page URLs
                foreach (CultureInfo cultureInfo in pageCultures)
                {
                    string cultureDefinedPageUrl = UrlPath.ResolveAbsoluteUrl(pageNode.GetFullUrl(cultureInfo, fallbackToAnyLanguage: false));
                    pageUrls.Add(cultureDefinedPageUrl);
                }

                // Take URL requesting details view for all MVC widgets on the page
                foreach (PageControl mvcWidget in page.Controls)
                {
                    var mvcWidgetUrl = this.GetMvcWidgetDetailsUrl(pageNode.Id, mvcWidget);
                    if (!string.IsNullOrEmpty(mvcWidgetUrl))
                    {
                        pageUrls.Add(mvcWidgetUrl);
                    }
                }
            }

            return pageUrls;
        }

        /// <summary>
        /// Gets all live pages containing MVC widgets.
        /// </summary>
        /// <param name="pageManager">The page manager.</param>
        /// <returns></returns>
        public IEnumerable<PageData> GetAllLiveHybridMvcPages(PageManager pageManager)
        {
            IEnumerable<PageData> pages = pageManager
                .GetPageDataList()
                .Where(pageData =>
                    pageData.Status == ContentLifecycleStatus.Live &&
                    pageData.Status != ContentLifecycleStatus.Deleted &&
                    pageData.Controls.Any(objectData => objectData.ObjectType == WidgetWrapperTypeName))
                .ToList();

            return pages;
        }

        /// <summary>
        /// Gets the MVC widget URL requesting details view.
        /// </summary>
        /// <param name="navigationNodeId">The id of the page navigation node.</param>
        /// <param name="mvcWidget">The MVC widget.</param>
        /// <returns></returns>
        public string GetMvcWidgetDetailsUrl(Guid navigationNodeId, PageControl mvcWidget)
        {
            var controllerFactory = ControllerBuilder.Current.GetControllerFactory() as FrontendControllerFactory;
            if (controllerFactory == null)
            {
                return null;
            }

            string controllerName = mvcWidget.Properties.FirstOrDefault(p => p.Name == ControllerPropertyName).Value;
            var controllerInfo = ControllerStore.Controllers().FirstOrDefault(c => c.ControllerType.ToString() == controllerName);
            var controllerType = controllerInfo.ControllerType;

            var controller = controllerFactory.CreateController(HttpContext.Current.Request.RequestContext, controllerType.FullName);

            var modelPropertyInfo = controllerType
                .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .FirstOrDefault(p => p.Name == ModelPropertyName);

            if (modelPropertyInfo == null)
            {
                return null;
            }

            var widgetDataItem = this.GetMvcWidgetDataItem(modelPropertyInfo, controller);
            if (widgetDataItem == null)
            {
                return null;
            }

            return HyperLinkHelpers.GetDetailPageUrl(widgetDataItem, navigationNodeId);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            SystemConfig systemConfig = Config.Get<SystemConfig>();
            OutputCacheElement cacheSettings = systemConfig.CacheSettings;

            this.RestoreCacheSettings(this.cacheSettingsCopy, cacheSettings);
        }

        private IDataItem GetMvcWidgetDataItem(PropertyInfo modelPropertyInfo, object widgetController)
        {
            if (modelPropertyInfo == null)
            {
                return null;
            }

            var model = modelPropertyInfo.GetValue(widgetController, null) as ContentModelBase;
            if (model == null)
            {
                return null;
            }

            var modelContentType = model.ContentType;
            var manager = ManagerBase.GetMappedManager(modelContentType, null);

            int defaultSkip = 0;
            int defaultTake = 1;
            var firstItem = manager
                .GetItems(modelContentType, default(string), default(string), defaultSkip, defaultTake)
                .OfType<IDataItem>()
                .FirstOrDefault();

            return firstItem as ILocatable;
        }

        private void SaveCacheSettings(OutputCacheElement cacheSettings)
        {
            this.CopyCacheSettings(cacheSettings);
            this.DisableCacheSettings(cacheSettings);
        }

        private void CopyCacheSettings(OutputCacheElement cacheSettings)
        {
            if (cacheSettings == null)
            {
                this.cacheSettingsCopy = null;
                return;
            }

            this.cacheSettingsCopy = new CacheSettingsPOCO
            {
                EnableClientCache = cacheSettings.EnableClientCache,
                EnableOutputCache = cacheSettings.EnableOutputCache
            };
        }

        private void DisableCacheSettings(OutputCacheElement cacheSettings)
        {
            if (cacheSettings == null)
            {
                return;
            }

            cacheSettings.EnableClientCache = false;
            cacheSettings.EnableOutputCache = false;
        }

        private OutputCacheElement RestoreCacheSettings(CacheSettingsPOCO cacheSettingsCopy, OutputCacheElement cacheSettings)
        {
            if (cacheSettings == null)
            {
                return null;
            }

            if (cacheSettingsCopy == null)
            {
                return cacheSettings;
            }

            cacheSettings.EnableClientCache = cacheSettingsCopy.EnableClientCache;
            cacheSettings.EnableOutputCache = cacheSettingsCopy.EnableOutputCache;

            return cacheSettings;
        }

        private CacheSettingsPOCO cacheSettingsCopy;

        private const string ModelPropertyName = "Model";
        private const string ControllerPropertyName = "ControllerName";
        private const string WidgetWrapperTypeName = "Telerik.Sitefinity.Mvc.Proxy.MvcControllerProxy";
    }
}