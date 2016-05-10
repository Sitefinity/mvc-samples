using System.Collections.Generic;
using System.Net;
using Crawler.Server;

namespace Crawler.Core
{
    /// <summary>
    /// This class provides a facade for requesting Sitefinity pages
    /// </summary>
    public class PageCrawler
    {
        /// <summary>
        /// Starts crawling.
        /// </summary>
        public void Start()
        {
            using (var pageOperator = new PageOperator())
            {
                IEnumerable<string> pageUrls = pageOperator.GetPageUrls();
                this.RequestPages(pageUrls);
            }
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
