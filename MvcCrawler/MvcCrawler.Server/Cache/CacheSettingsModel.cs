namespace MvcCrawler.Server.Cache
{
    /// <summary>
    /// Model class for Sitefinity cache settings
    /// </summary>
    public class CacheSettingsModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether output cache is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if output cache is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool EnableOutputCache { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether client cache is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if client cache is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool EnableClientCache { get; set; }
    }
}
