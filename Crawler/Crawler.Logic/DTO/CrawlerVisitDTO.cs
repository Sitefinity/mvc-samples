using System;
using System.Collections.Generic;
using Crawler.Server.Mvc;

namespace Crawler.Core.DTO
{
    /// <summary>
    /// This class represents page information collected from crawler visits.
    /// </summary>
    public class CrawlerVisitDTO
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public IEnumerable<WidgetViewInfo> ViewsInfo { get; set; }
    }
}