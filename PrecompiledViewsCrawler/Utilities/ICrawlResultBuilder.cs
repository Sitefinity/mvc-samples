using System;
using System.Collections.Generic;
using PrecompiledViewsCrawler.Mvc.Models;

namespace PrecompiledViewsCrawler.Utilities
{
    public interface ICrawlResultBuilder
    {
        IEnumerable<CrawlItemViewModel> CrawlItems { get; }

        void AddItem(CrawlItemViewModel newItem);

        void Initialize();

        void Dispose();
    }
}
