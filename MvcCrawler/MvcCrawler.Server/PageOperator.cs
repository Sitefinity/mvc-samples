using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Frontend.Mvc.Helpers;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers;
using Telerik.Sitefinity.Frontend.Mvc.Models;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Modules.Pages;
using Telerik.Sitefinity.Mvc.Store;
using Telerik.Sitefinity.Pages.Model;
using Telerik.Sitefinity.Web;

namespace MvcCrawler.Server
{
    /// <summary>
    /// This class provides methods to operate with Sitefinity pages
    /// </summary>
    public class PageOperator
    {
        /// <summary>
        /// Gets the page urls.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetPageUrls()
        {
            var pageManager = PageManager.GetManager();
            var pages = this.GetPageInfo(pageManager);

            var pageUrls = new HashSet<string>();

            var controllerFactory = ControllerBuilder.Current.GetControllerFactory() as FrontendControllerFactory;
            if (controllerFactory == null)
            {
                return null;
            }

            foreach (PageData page in pages)
            {
                PageNode pageNode = page.NavigationNode;

                var pageCultures = pageNode.AvailableCultures;

                string defaultPageUrl = UrlPath.ResolveAbsoluteUrl(pageNode.GetFullUrl());
                pageUrls.Add(defaultPageUrl);

                foreach (CultureInfo cultureInfo in pageCultures)
                {
                    string cultureDefinedPageUrl = UrlPath.ResolveAbsoluteUrl(pageNode.GetFullUrl(cultureInfo, fallbackToAnyLanguage: false));
                    pageUrls.Add(cultureDefinedPageUrl);
                }

                foreach (PageControl control in page.Controls)
                {
                    var controlUrl = this.GetPageControlUrl(pageNode.Id, control, controllerFactory);
                    if (!string.IsNullOrEmpty(controlUrl))
                    {
                        pageUrls.Add(controlUrl);
                    }
                }
            }

            return pageUrls;
        }

        private IEnumerable<PageData> GetPageInfo(PageManager pageManager)
        {
            IEnumerable<PageData> pages = pageManager
                .GetPageDataList()
                .Where(pageData =>
                    pageData.Status == ContentLifecycleStatus.Live &&
                    pageData.Status != ContentLifecycleStatus.Deleted &&
                    pageData.Controls.Where(objectData => objectData.ObjectType == WidgetWrapperTypeName).Any())
                .ToList();

            return pages;
        }

        private string GetPageControlUrl(Guid navigationNodeId, PageControl control, FrontendControllerFactory controllerFactory)
        {
            string controllerName = control.Properties.FirstOrDefault(p => p.Name == ControllerPropertyName).Value;
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

            var firstItem = this.GetControlItem(modelPropertyInfo, controller);
            if (firstItem == null)
            {
                return null;
            }

            return HyperLinkHelpers.GetDetailPageUrl(firstItem, navigationNodeId);
        }

        private IDataItem GetControlItem(PropertyInfo modelPropertyInfo, object convertedController)
        {
            if (modelPropertyInfo == null)
            {
                return null;
            }

            var model = modelPropertyInfo.GetValue(convertedController, null) as ContentModelBase;
            if (model == null)
            {
                return null;
            }

            var modelContentType = model.ContentType;
            var manager = ManagerBase.GetMappedManager(modelContentType, null);

            var firstItem = manager.GetItems(modelContentType, null, null, 0, 1).OfType<IDataItem>().FirstOrDefault();

            return firstItem as ILocatable;
        }

        private const string ModelPropertyName = "Model";
        private const string ControllerPropertyName = "ControllerName";
        private const string WidgetWrapperTypeName = "Telerik.Sitefinity.Mvc.Proxy.MvcControllerProxy";
    }
}
