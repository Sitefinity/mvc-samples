using MvcCrawler.Server.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcCrawler.Server
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

        public static void AddItem(WidgetViewInfo newItem)
        {
            var oldItem = viewsInfo.FirstOrDefault(x => x.ViewName == newItem.ViewName && x.Url == newItem.Url);
            if (oldItem == null)
            {
                viewsInfo.Add(newItem);
                return;
            }

            if (newItem.IsPrecompiled)
            {
                viewsInfo.Remove(oldItem);
                viewsInfo.Add(newItem);
            }
        }

        public static void Dispose()
        {
            viewsInfo.Clear();
        }

        private static ICollection<WidgetViewInfo> viewsInfo;
    }
}
