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

        /// <summary>
        /// Gets the calendars.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [Route("web-interface/calendars/")]
        public ActionResult GetCalendars(SchedulerEventsModel model)
        {
            var calendars = model.GetCalendars();

            return this.Json(calendars, JsonRequestBehavior.AllowGet);
        }
    }
}
