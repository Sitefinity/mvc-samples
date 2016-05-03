using System;
using System.Collections.Generic;
using PrecompiledViewsCrawler.Crawlers;
using PrecompiledViewsCrawler.Utilities;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers;
using Telerik.Sitefinity.Modules.Pages.Configuration;
using Telerik.Sitefinity.Services;

namespace PrecompiledViewsCrawler.Mvc.Models
{
    public class CrawlResultViewModel : ICrawlResultViewModel
    {
        public CrawlResultViewModel()
        {
            this.StartTime = DateTime.Now;
            this.CrawlItems = new List<CrawlItemViewModel>();
            this.crawler = ControllerModelFactory.GetModel<ICrawler>(this.GetType());
            this.crawlResultBuilder = ControllerModelFactory.GetModel<ICrawlResultBuilder>(this.GetType());
            this.logger = ControllerModelFactory.GetModel<IJsonLogger>(this.GetType());
        }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public IEnumerable<CrawlItemViewModel> CrawlItems { get; set; }

        public void BuildCrawlItems()
        {
            this.crawlResultBuilder.Initialize();

            SystemConfig systemConfig = Config.Get<SystemConfig>();
            OutputCacheElement cacheSettings = systemConfig.CacheSettings;
            CacheSettingsModel cacheSettingsModel = this.StoreCacheSettings(cacheSettings);

            this.DisableCacheSettings(cacheSettings);

            this.crawler.Start();

            this.EndTime = DateTime.Now;
            this.CrawlItems = new List<CrawlItemViewModel>(this.crawlResultBuilder.CrawlItems);
            this.SaveToFile(CrawlResultViewModel.FileName);

            this.RestoreCacheSettings(cacheSettingsModel, cacheSettings);

            this.crawlResultBuilder.Dispose();
        }

        public void SaveToFile(string fileName)
        {
            this.logger.SaveToFile(new { this.StartTime, this.CrawlItems, this.EndTime }, fileName);
        }

        private CacheSettingsModel StoreCacheSettings(OutputCacheElement cacheSettings)
        {
            if (cacheSettings == null)
            {
                return null;
            }

            return new CacheSettingsModel
            {
                EnableClientCache = cacheSettings.EnableClientCache,
                EnableOutputCache = cacheSettings.EnableOutputCache
            };
        }

        private void DisableCacheSettings(OutputCacheElement cacheSettings)
        {
            if (cacheSettings == null)
            {
                return;
            }

            cacheSettings.EnableClientCache = false;
            cacheSettings.EnableOutputCache = false;
        }

        private OutputCacheElement RestoreCacheSettings(CacheSettingsModel cacheSettingsModel, OutputCacheElement cacheSettings)
        {
            if (cacheSettings == null)
            {
                return null;
            }

            if (cacheSettingsModel == null)
            {
                return cacheSettings;
            }

            cacheSettings.EnableClientCache = cacheSettingsModel.EnableClientCache;
            cacheSettings.EnableOutputCache = cacheSettingsModel.EnableOutputCache;

            return cacheSettings;
        }
        
        private readonly ICrawler crawler;
        private readonly ICrawlResultBuilder crawlResultBuilder;
        private readonly IJsonLogger logger;
        private const string FileName = "PrecompiledViewsUsage.json";
    }
}