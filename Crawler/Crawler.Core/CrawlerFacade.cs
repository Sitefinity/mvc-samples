using System.Collections.Generic;
using System.Net;
using Crawler.Server;
using System;
using Crawler.Server.Mvc;
using Crawler.Core.DTO;

namespace Crawler.Core
{
    /// <summary>
    /// Crawler facade class
    /// </summary>
    public class CrawlerFacade
    {
        public CrawlerFacade()
        {
            this.logger = new JsonLogger();
        }

        /// <summary>
        /// Visits pages and collects visit info
        /// </summary>
        public CrawlerVisitDTO Run(bool saveVisitInfoToFile = true)
        {
            using (var pagesService = new MvcPagesService())
            {
                // Register start time and initialize view info builder
                DateTime startTime = DateTime.UtcNow;
                ViewInfoBuilder.Initialize();

                // Visit pages and collect views information
                IEnumerable<string> pageUrls = pagesService.GetAllLiveHybridMvcPageUrls();
                this.RequestPages(pageUrls);
                IEnumerable<ViewInfo> viewsInfo = this.GetViewsInfo();

                // Register end time and release collected views info
                DateTime endTime = DateTime.UtcNow;
                ViewInfoBuilder.Dispose();

                var visitInfo = new CrawlerVisitDTO()
                {
                    StartTime = startTime,
                    EndTime = endTime,
                    ViewsInfo = viewsInfo
                };

                if (saveVisitInfoToFile)
                {
                    this.SaveVisitInfoToFile(visitInfo, FileName);
                }

                return visitInfo;
            }
        }

        /// <summary>
        /// Saves the crawler visit information to file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void SaveVisitInfoToFile(CrawlerVisitDTO visitInfo, string fileName)
        {
            this.logger.SaveToFile(new { StartTime = visitInfo.StartTime, PageVisitInfo = visitInfo.ViewsInfo, EndTime = visitInfo.EndTime }, fileName);
        }

        private IEnumerable<ViewInfo> GetViewsInfo()
        {
            if (ViewInfoBuilder.ViewsInfo == null)
            {
                return null;
            }

            return new List<ViewInfo>(ViewInfoBuilder.ViewsInfo);
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

        private readonly JsonLogger logger;
        private const string FileName = "PrecompiledViewsUsage.json";
    }
}
