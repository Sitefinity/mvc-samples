using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Frontend.Mvc.Helpers;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers;
using Telerik.Sitefinity.Frontend.Mvc.Models;
using PrecompiledViewsCrawler.Contracts;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Modules.Pages;
using Telerik.Sitefinity.Mvc.Store;
using Telerik.Sitefinity.Pages.Model;
using Telerik.Sitefinity.Web;

namespace PrecompiledViewsCrawler
{
    public class DefaultCrawler : ICrawler
    {
        public DefaultCrawler()
            : this(PageManager.GetManager(), ControllerBuilder.Current)
        {
        }

        public DefaultCrawler(PageManager pageManager, ControllerBuilder controllerBuidler)
        {
            this.PageManager = pageManager;
            this.ControllerBuilder = controllerBuidler;
        }

        public PageManager PageManager { get; private set; }

        public ControllerBuilder ControllerBuilder { get; private set; }

        public void Crawl()
        {
            IEnumerable<PageData> info = this.PageManager
                .GetPageDataList()
                .Where(pageData =>
                    pageData.Status == ContentLifecycleStatus.Live &&
                    pageData.Status != ContentLifecycleStatus.Deleted &&
                    pageData.Controls.Where(objectData => objectData.ObjectType == WidgetWrapperTypeName).Any())
                .ToList();

            foreach (PageData pageData in info)
            {
                PageNode pageNode = pageData.NavigationNode;

                CultureInfo[] pageCultures = pageNode.AvailableCultures;
                ICollection<string> pageUrls = new List<string>();

                string defaultPageUrl = UrlPath.ResolveAbsoluteUrl(pageNode.GetFullUrl());
                pageUrls.Add(defaultPageUrl);

                foreach (CultureInfo cultureInfo in pageCultures)
                {
                    string cultureDefinedPageUrl = UrlPath.ResolveAbsoluteUrl(pageNode.GetFullUrl(cultureInfo, fallbackToAnyLanguage: false));
                    pageUrls.Add(cultureDefinedPageUrl);
                }

                foreach (string url in pageUrls)
                {
                    this.MakeWebRequest(url);
                }

                FrontendControllerFactory controllerFactory = this.ControllerBuilder.GetControllerFactory() as FrontendControllerFactory;
                if (controllerFactory == null)
                {
                    return;
                }

                foreach (PageControl control in pageData.Controls)
                {
                    string controllerName = control.Properties.FirstOrDefault(p => p.Name == "ControllerName").Value;

                    ControllerInfo controllerInfo = ControllerStore.Controllers().FirstOrDefault(c => c.ControllerType.ToString() == controllerName);
                    Type controllerType = controllerInfo.ControllerType;

                    Controller controller = controllerFactory.CreateController(HttpContext.Current.Request.RequestContext, controllerType.FullName) as Controller;
                    dynamic convertedController = Convert.ChangeType(controller, controllerType);

                    PropertyInfo modelPropertyInfo = controllerType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault(p => p.Name == "Model");
                    if (modelPropertyInfo == null)
                    {
                        continue;
                    }

                    ContentModelBase model = modelPropertyInfo.GetValue(convertedController, null) as ContentModelBase;
                    if (model == null)
                    {
                        continue;
                    }

                    Type modelContentType = model.ContentType;
                    IManager manager = ManagerBase.GetMappedManager(modelContentType, null);

                    IDataItem firstItem = manager.GetItems(modelContentType, null, null, 0, 1).OfType<IDataItem>().FirstOrDefault();
                    if (firstItem == null)
                    {
                        continue;
                    }

                    if (firstItem is ILocatable)
                    {
                        string firstItemUrl = HyperLinkHelpers.GetDetailPageUrl(firstItem, pageData.NavigationNodeId);
                        this.MakeWebRequest(firstItemUrl);
                    }
                }
            }
        }

        private HttpWebResponse MakeWebRequest(string url)
        {
            var webRequest = WebRequest.Create(url) as HttpWebRequest;
            if (webRequest == null)
            {
                return null;
            }

            webRequest.Timeout = 120 * 1000; // 120 sec
            webRequest.CookieContainer = new CookieContainer();
            webRequest.Headers["Authorization"] = System.Web.HttpContext.Current.Request.Headers["Authorization"];
            webRequest.Headers["X-Crawler"] = "Enabled";
            webRequest.CachePolicy = new RequestCachePolicy();
            var webResponse = webRequest.GetResponse() as HttpWebResponse;

            return webResponse;
        }

        private const string WidgetWrapperTypeName = "Telerik.Sitefinity.Mvc.Proxy.MvcControllerProxy";
    }
}
