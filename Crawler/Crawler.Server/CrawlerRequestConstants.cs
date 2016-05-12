namespace Crawler.Server
{
    /// <summary>
    /// This class contains constants related to HTTP requests made by the crawler.
    /// </summary>
    public static class CrawlerRequestConstants
    {
        /// <summary>
        /// Name of the header used to distinguish crawler requests
        /// </summary>
        public const string HeaderName = "X-Crawler";


        /// <summary>
        /// Value of the header used to distinguish crawler requests
        /// </summary>
        public const string HeaderValue = "Enabled";
    }
}
