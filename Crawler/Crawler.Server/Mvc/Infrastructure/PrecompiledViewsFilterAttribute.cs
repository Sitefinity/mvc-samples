using System;
using System.Linq;
using System.Web.Mvc;

namespace Crawler.Server.Mvc.Infrastructure
{
    /// <summary>
    /// Filter that registers the extended precompiled view engine when that is required. 
    /// </summary>
    /// <seealso cref="System.Web.Mvc.ActionFilterAttribute" />
    internal class PrecompiledViewsFilterAttribute : ActionFilterAttribute
    {
        public PrecompiledViewsFilterAttribute()
        {
        }

        /// <summary>
        /// Registers the precompiled view engine if the request contains the crawler header.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var precompiledViewEngineInstaller = new PrecompiledViewEngineManager();

            Controller controller = filterContext.Controller as Controller;
            if (controller == null)
            {
                return;
            }

            bool requestContainsCrawlerHeader = filterContext.HttpContext.Request.Headers.GetValues(CrawlerRequestConstants.HeaderName) != null;
            if (requestContainsCrawlerHeader)
            {
                precompiledViewEngineInstaller.RegisterPrecompiledViewEngine(controller, typeof(PrecompiledViewEngine));
            }
            else
            {
                precompiledViewEngineInstaller.RemovePrecompiledViewEngine(controller.ViewEngineCollection);
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
