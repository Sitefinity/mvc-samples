using System;
using System.Collections.Generic;

namespace PrecompiledViewsCrawler.Mvc.Models
{
    public interface ICrawlResultViewModel
    {
        DateTime StartTime { get; set; }

        DateTime EndTime { get; set; }

        IEnumerable<CrawlItemViewModel> CrawlItems { get; set; }

        void BuildCrawlItems();

        void SaveToFile(string fileName);
    }
}
