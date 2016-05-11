using KendoEvents.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Telerik.Sitefinity.ContentLocations;
using Telerik.Sitefinity.Frontend.Events.Mvc.Controllers;
using Telerik.Sitefinity.Frontend.Events.Mvc.Models;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers;
using Telerik.Sitefinity.Modules.Pages.Configuration;
using Telerik.Sitefinity.Mvc;
using Telerik.Sitefinity.Services;

namespace KendoEvents.Mvc.Controllers
{
    /// <summary>
    /// The controller for Events Scheduler widget.
    /// </summary>
    [ControllerToolboxItem(Name = "SchedulerEvents_MVC", Title = "Scheduler Events", SectionName = ToolboxesConfig.ContentToolboxSectionName, ModuleName = "Events")]
    public class SchedulerEventsController : EventController
    {
        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public override IEventModel Model
        {
            get
            {
                if (this.model == null)
                    this.model = new SchedulerEventsModel();

                return this.model;
            }
        }

        /// <summary>
        /// Gets the scheduler events.
        /// </summary>
        /// <returns></returns>
        [Route("web-interface/events/")]
        public ActionResult GetSchedulerEvents()
        {
            var model = this.Model as SchedulerEventsModel;
            var events = model.GetSchedulerEvents();

            return this.Json(events, JsonRequestBehavior.AllowGet);
        }

        private SchedulerEventsModel model;
    }
}
