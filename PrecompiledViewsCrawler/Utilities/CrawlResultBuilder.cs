using System.Collections.Generic;
using System.Linq;

using PrecompiledViewsCrawler.Models;

namespace PrecompiledViewsCrawler.Utilities
{
    public static class CrawlResultBuilder
    {
        static CrawlResultBuilder()
        {
            Items = new List<CrawlItemViewModel>();
        }

        public static void Add(CrawlItemViewModel newItem)
        {
            var oldItem = Items.FirstOrDefault(x => x.ViewPath == newItem.ViewPath && x.Url == newItem.Url);
            if(oldItem == null)
            {
                Items.Add(newItem);
                return;
            }

            Items.Remove(oldItem);
            Items.Add(newItem);
        }

        public static IEnumerable<CrawlItemViewModel> GetItems()
        {
            return Items;
        }

        private static readonly ICollection<CrawlItemViewModel> Items;
    }
}