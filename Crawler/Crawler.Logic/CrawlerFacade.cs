using System.Collections.Generic;
using System.Net;
using Crawler.Server;

namespace Crawler.Core
{
    /// <summary>
    /// Crawler facade class
    /// </summary>
    public class CrawlerFacade
    {
        /// <summary>
        /// Starts crawling.
        /// </summary>
        public void Start()
        {
            using (var pagesService = new MvcPagesService())
            {
                IEnumerable<string> pageUrls = pagesService.GetAllLiveHybridMvcPageUrls();
                this.RequestPages(pageUrls);
            }
        }

        private void RequestPages(IEnumerable<string> pageUrls)
        {
            foreach (var pageUrl in pageUrls)
            {
                if (!string.IsNullOrEmpty(pageUrl))
                {
                    this.CrawlPage(pageUrl);
                }
            }
        }

        private HttpWebRequest CreateWebRequest(string pageUrl)
        {
            var webRequest = WebRequest.Create(pageUrl) as HttpWebRequest;
            if (webRequest == null)
            {
                return null;
            }

            webRequest.Timeout = 2 * 60 * 1000; // 2 minutes
            webRequest.CookieContainer = new CookieContainer();

            return webRequest;
        }

        private HttpWebResponse CrawlPage(string pageUrl)
        {
            var webRequest = this.CreateWebRequest(pageUrl);
            webRequest.Headers.Add(CrawlerRequestConstants.HeaderName, CrawlerRequestConstants.HeaderValue);

            var webResponse = webRequest.GetResponse() as HttpWebResponse;

            return webResponse;
        }
    }
}
