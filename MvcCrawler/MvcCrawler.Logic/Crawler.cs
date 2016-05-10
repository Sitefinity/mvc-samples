using System.Collections.Generic;
using System.Net;
using MvcCrawler.Server;
using MvcCrawler.Server.Cache;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Modules.Pages.Configuration;
using Telerik.Sitefinity.Services;

namespace MvcCrawler.Logic
{
    /// <summary>
    /// This class provides a facade for requesting Sitefinity pages
    /// </summary>
    public class Crawler
    {
        /// <summary>
        /// Starts crawling.
        /// </summary>
        public void Start()
        {
            SystemConfig systemConfig = Config.Get<SystemConfig>();
            OutputCacheElement cacheSettings = systemConfig.CacheSettings;

            CacheSettingsModel cacheSettingsModel = this.StoreCacheSettings(cacheSettings);
            this.DisableCacheSettings(cacheSettings);

            var pageOperator = new PageOperator();
            IEnumerable<string> pageUrls = pageOperator.GetPageUrls();
            this.RequestPages(pageUrls);

            this.RestoreCacheSettings(cacheSettingsModel, cacheSettings);
        }

        private void RequestPages(IEnumerable<string> pageUrls)
        {
            foreach (var item in pageUrls)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    this.MakeWebRequest(item, isCrawled: true);
                }
            }
        }

        private CacheSettingsModel StoreCacheSettings(OutputCacheElement cacheSettings)
        {
            if (cacheSettings == null)
            {
                return null;
            }

            return new CacheSettingsModel
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

        private OutputCacheElement RestoreCacheSettings(CacheSettingsModel cacheSettingsModel, OutputCacheElement cacheSettings)
        {
            if (cacheSettings == null)
            {
                return null;
            }

            if (cacheSettingsModel == null)
            {
                return cacheSettings;
            }

            cacheSettings.EnableClientCache = cacheSettingsModel.EnableClientCache;
            cacheSettings.EnableOutputCache = cacheSettingsModel.EnableOutputCache;

            return cacheSettings;
        }

        private HttpWebRequest CreateStandardWebRequest(string pageUrl)
        {
            var webRequest = WebRequest.Create(pageUrl) as HttpWebRequest;
            if (webRequest == null)
            {
                return null;
            }

            webRequest.Timeout = 120 * 1000; // 120 sec
            webRequest.CookieContainer = new CookieContainer();

            return webRequest;
        }

        private HttpWebResponse MakeWebRequest(string pageUrl, bool isCrawled = false)
        {
            var webRequest = this.CreateStandardWebRequest(pageUrl);
            if (isCrawled)
            {
                webRequest.Headers.Add(CrawlerRequestConstants.HeaderName, CrawlerRequestConstants.HeaderValue);
            }

            var webResponse = webRequest.GetResponse() as HttpWebResponse;
            return webResponse;
        }
    }
}
