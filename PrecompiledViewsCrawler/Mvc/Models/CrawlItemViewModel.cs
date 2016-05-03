namespace PrecompiledViewsCrawler.Mvc.Models
{
    public class CrawlItemViewModel
    {
        public string WidgetName { get; set; }

        public string Url { get; set; }

        public string ViewName { get; set; }

        public string ViewPath { get; set; }

        public bool IsPrecompiled { get; set; }
    }
}