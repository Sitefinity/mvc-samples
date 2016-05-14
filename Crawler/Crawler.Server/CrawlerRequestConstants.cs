namespace Crawler.Server
{
    /// <summary>
    /// This class contains constants related to the crawler HTTP requests.
    /// </summary>
    public static class CrawlerRequestConstants
    {
        /// <summary>
        /// Name of the header used to distinguish crawler HTTP requests
        /// </summary>
        public const string HeaderName = "X-Crawler";


        /// <summary>
        /// Value of the header used to distinguish crawler HTTP requests
        /// </summary>
        public const string HeaderValue = "Enabled";
    }
}
