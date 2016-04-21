using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrecompiledViewsCrawler.Models
{
    public class CrawlItemViewModel
    {
        public string Url { get; set; }

        public string ViewPath { get; set; }

        public bool IsPrecompiled { get; set; }
    }
}