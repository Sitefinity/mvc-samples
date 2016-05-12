using System;
using System.Collections.Generic;
using Crawler.Core;
using Crawler.Server;
using Crawler.Server.Mvc;

namespace Crawler.Client.Mvc.Models
{
    /// <summary>
    /// This class represents page information collected from crawler visits.
    /// </summary>
    public class PageVisitInfo
    {
        public PageVisitInfo()
        {
            this.StartTime = DateTime.Now;
            this.CrawlItems = new List<WidgetViewInfo>();
            this.crawler = new CrawlerFacade();
            this.logger = new JsonLogger();
        }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public IEnumerable<WidgetViewInfo> CrawlItems { get; set; }

        public void CollectInfo()
        {
            PageVisitInfoBuilder.Initialize();

            this.crawler.Start();

            this.EndTime = DateTime.Now;
            this.CrawlItems = new List<WidgetViewInfo>(PageVisitInfoBuilder.ViewsInfo);
            this.SaveToFile(PageVisitInfo.FileName);

            PageVisitInfoBuilder.Dispose();
        }

        public void SaveToFile(string fileName)
        {
            this.logger.SaveToFile(new { this.StartTime, this.CrawlItems, this.EndTime }, fileName);
        }

        private readonly CrawlerFacade crawler;
        private readonly JsonLogger logger;
        private const string FileName = "PrecompiledViewsUsage.json";
    }
}