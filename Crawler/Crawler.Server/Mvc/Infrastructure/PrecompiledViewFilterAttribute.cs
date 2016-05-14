using System;
using System.Linq;
using System.Web.Mvc;

namespace Crawler.Server.Mvc.Infrastructure
{
    /// <summary>
    /// Filter that registers the precompiled view engine. 
    /// </summary>
    /// <seealso cref="System.Web.Mvc.ActionFilterAttribute" />
    internal class PrecompiledViewFilterAttribute : ActionFilterAttribute
    {
        public PrecompiledViewFilterAttribute()
        {
        }

        /// <summary>
        /// Registers the precompiled view engine.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var precompiledViewEngineManager = new PrecompiledViewEngineManager();

            Controller controller = filterContext.Controller as Controller;
            if (controller == null)
            {
                return;
            }

            precompiledViewEngineManager.RegisterPrecompiledViewEngine(controller, typeof(PrecompiledViewEngine));

            base.OnActionExecuting(filterContext);
        }
    }
}
