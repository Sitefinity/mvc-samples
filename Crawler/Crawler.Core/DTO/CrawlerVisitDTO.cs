using System;
using System.Collections.Generic;
using Crawler.Server.Mvc;

namespace Crawler.Core.DTO
{
    /// <summary>
    /// This class represents page information collected from crawler visits.
    /// </summary>
    public class CrawlerVisitDTO
    {
        /// <summary>
        /// Gets or sets the crawler visit start time.
        /// </summary>
        /// <value>
        /// The crawler visit start time.
        /// </value>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the crawler visit end time.
        /// </summary>
        /// <value>
        /// The crawler visit end time.
        /// </value>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets the crawler visit views information.
        /// </summary>
        /// <value>
        /// The crawler visit views information.
        /// </value>
        public IEnumerable<ViewInfo> ViewsInfo { get; set; }
    }
}