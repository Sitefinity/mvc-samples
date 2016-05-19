using KendoEvents.Mvc.Models;
using System.Web.Mvc;

namespace KendoEvents.Mvc.Controllers
{
    /// <summary>
    /// The controller for Events Scheduler widget.
    /// </summary>
    public class SchedulerEventsController : Controller
    {
        /// <summary>
        /// Gets the scheduler events.
        /// </summary>
        /// <returns></returns>
        [Route("web-interface/events/")]
        public ActionResult GetSchedulerEvents(SchedulerEventsModel model)
        {
            var events = model.GetSchedulerEvents();

            return this.Json(events, JsonRequestBehavior.AllowGet);
        }
    }
}
