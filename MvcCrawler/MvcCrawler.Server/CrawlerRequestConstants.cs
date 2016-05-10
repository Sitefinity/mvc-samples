namespace MvcCrawler.Server
{
    /// <summary>
    /// This class contains al constants used by the crawler.
    /// </summary>
    public static class CrawlerRequestConstants
    {
        /// <summary>
        /// Name of the header used to distinguish crawler requests
        /// </summary>
        public const string HeaderName = "X-Crawler";


        /// <summary>
        /// value of the header used to distinguish crawler requests
        /// </summary>
        public const string HeaderValue = "Enabled";
    }
}
