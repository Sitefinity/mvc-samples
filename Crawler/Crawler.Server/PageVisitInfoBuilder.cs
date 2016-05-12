using System;
using System.Collections.Generic;
using System.Linq;
using Crawler.Server.Mvc;

namespace Crawler.Server
{
    /// <summary>
    /// This class provides methods for operating with widget views info.
    /// </summary>
    public static class PageVisitInfoBuilder
    {
        public static IEnumerable<WidgetViewInfo> ViewsInfo
        {
            get { return viewsInfo; }
        }

        public static void Initialize()
        {
            viewsInfo = new List<WidgetViewInfo>();
        }

        public static void AddItem(WidgetViewInfo viewInfo)
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

        public static void Dispose()
        {
            viewsInfo.Clear();
        }

        private static ICollection<WidgetViewInfo> viewsInfo;
    }
}
