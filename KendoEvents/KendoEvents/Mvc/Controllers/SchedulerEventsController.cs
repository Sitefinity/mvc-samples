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
    public class SchedulerEventsController : Controller, IContentLocatableView
    {
        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public virtual IEventModel Model
        {
            get
            {
                if (this.model == null)
                    this.model = new SchedulerEventsModel();

                return this.model;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the canonical URL tag should be added to the page when the canonical meta tag should be added to the page.
        /// If the value is not set, the settings from SystemConfig -> ContentLocationsSettings -> DisableCanonicalURLs will be used. 
        /// </summary>
        /// <value>The disable canonical URLs.</value>
        public bool? DisableCanonicalUrlMetaTag
        {
            get
            {
                return this.disableCanonicalUrlMetaTag;
            }

            set
            {
                this.disableCanonicalUrlMetaTag = value;
            }
        }

        /// <summary>
        /// The default action
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        public ActionResult Index(int? page)
        {
            var viewModel = this.Model.CreateListViewModel(null, page: page ?? 1);

            if (SystemManager.CurrentHttpContext != null)
                this.AddCacheDependencies(this.Model.GetKeysOfDependentObjects(viewModel));

            return this.View("List.Scheduler", viewModel);
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

        /// <summary>
        /// Gets the information for all of the content types that a control is able to show.
        /// </summary>
        /// <returns>
        /// List of location info of the content that this control is able to show.
        /// </returns>
        [NonAction]
        public virtual IEnumerable<IContentLocationInfo> GetLocations()
        {
            return this.Model.GetLocations();
        }

        /// <summary>
        /// Called when a request matches this controller, but no method with the specified action name is found in the controller.
        /// </summary>
        /// <param name="actionName">The name of the attempted action.</param>
        protected override void HandleUnknownAction(string actionName)
        {
            this.Index(null).ExecuteResult(this.ControllerContext);
        }

        private SchedulerEventsModel model;
        private bool? disableCanonicalUrlMetaTag;
    }
}
