namespace Crawler.Server.Mvc
{
    /// <summary>
    /// View model for the widget view info
    /// </summary>
    public class WidgetViewInfo
    {
        /// <summary>
        /// Gets or sets the name of the widget.
        /// </summary>
        /// <value>
        /// The name of the widget.
        /// </value>
        public string WidgetName { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
        public string ViewName { get; set; }

        /// <summary>
        /// Gets or sets the view path.
        /// </summary>
        /// <value>
        /// The view path.
        /// </value>
        public string ViewPath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is precompiled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is precompiled; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrecompiled { get; set; }
    }
}