using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrecompiledViewsCrawler.Models
{
    public class CrawlResultViewModel
    {
        public CrawlResultViewModel()
        {
            this.StartTime = DateTime.Now;
            this.CrawlItems = new List<CrawlItemViewModel>();
            this.EndTime = DateTime.Now;
        }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public IEnumerable<CrawlItemViewModel> CrawlItems { get; set; }
    }
}