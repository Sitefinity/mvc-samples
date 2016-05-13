using System;
using System.Collections.Generic;
using System.Linq;
using Crawler.Server.Mvc;

namespace Crawler.Server
{
    /// <summary>
    /// This class provides methods for operating with widget views info.
    /// </summary>
    public static class ViewInfoBuilder
    {
        /// <summary>
        /// Gets the views information.
        /// </summary>
        /// <value>
        /// The views information.
        /// </value>
        public static IEnumerable<ViewInfo> ViewsInfo
        {
            get { return viewsInfo; }
        }

        /// <summary>
        /// Initializes the view info collection.
        /// </summary>
        public static void Initialize()
        {
            viewsInfo = new List<ViewInfo>();
        }

        /// <summary>
        /// Adds view information.
        /// </summary>
        /// <param name="viewInfo">The view information.</param>
        public static void Add(ViewInfo viewInfo)
        {
            var existingViewInfo = viewsInfo.FirstOrDefault(x => x.ViewName == viewInfo.ViewName && x.Url == viewInfo.Url);
            if (existingViewInfo == null)
            {
                viewsInfo.Add(viewInfo);
                return;
            }

            if (viewInfo.IsPrecompiled)
            {
                viewsInfo.Remove(existingViewInfo);
                viewsInfo.Add(viewInfo);
            }
        }

        /// <summary>
        /// Clears the view info collection.
        /// </summary>
        public static void Dispose()
        {
            viewsInfo.Clear();
        }

        private static ICollection<ViewInfo> viewsInfo;
    }
}
