using System;
using System.Collections.Generic;
using System.Linq;
using PrecompiledViewsCrawler.Mvc.Models;

namespace PrecompiledViewsCrawler.Utilities
{
    public class DefaultCrawlResultBuilder : ICrawlResultBuilder
    {
        public IEnumerable<CrawlItemViewModel> CrawlItems
        {
            get { return this.crawlItems; }
        }

        public void Initialize()
        {
            this.crawlItems = new List<CrawlItemViewModel>();
        }

        public void AddItem(CrawlItemViewModel newItem)
        {
            var oldItem = this.crawlItems.FirstOrDefault(x => x.ViewName == newItem.ViewName && x.Url == newItem.Url);
            if (oldItem == null)
            {
                this.crawlItems.Add(newItem);
                return;
            }

            if (newItem.IsPrecompiled)
            {
                this.crawlItems.Remove(oldItem);
                this.crawlItems.Add(newItem);
            }
        }

        public void Dispose()
        {
            this.crawlItems.Clear();
        }

        private ICollection<CrawlItemViewModel> crawlItems;
    }
}
