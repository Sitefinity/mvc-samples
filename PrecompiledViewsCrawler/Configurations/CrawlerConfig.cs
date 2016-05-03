using System.Web.Mvc;
using System.Web.Routing;
using PrecompiledViewsCrawler.Mvc.Controllers.Infrastructure.ActionFilters;

namespace PrecompiledViewsCrawler.Configurations
{
    public static class CrawlerConfig
    {
        public static void RegisterCrawler(GlobalFilterCollection filterCollection, RouteCollection routeCollection)
        {
            filterCollection.Add(new ViewEngineFilterAttribute());
            RouteCollectionExtensions.MapRoute(routeCollection, "Crawler", "Precompilation/{controller}/{action}/{id}", new { controller = "Crawler", action = "Index", id = 0 });
        }
    }
}
