using System.Web.Mvc;
using System.Web.Routing;
using Crawler.Server.Mvc.Infrastructure;

namespace Crawler.Server.Mvc.Configurations
{
    /// <summary>
    /// Configuration class.
    /// </summary>
    public static class CrawlerConfig
    {
        /// <summary>
        /// Registers the crawler precompiled view  engine action filter and a new route for the crawler.
        /// </summary>
        /// <param name="filterCollection">The filter collection.</param>
        /// <param name="routeCollection">The route collection.</param>
        public static void RegisterCrawler(GlobalFilterCollection filterCollection, RouteCollection routeCollection)
        {
            filterCollection.Add(new PrecompiledViewFilterAttribute());
            RouteCollectionExtensions.MapRoute(routeCollection, "Crawler", "Precompilation/{controller}/{action}/{id}", new { controller = "Crawler", action = "Index", id = 0 });
        }
    }
}
