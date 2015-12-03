using SingleDynamicContent.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Telerik.Sitefinity.ContentLocations;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Mvc;
using Telerik.Sitefinity.Services;

namespace SingleDynamicContent.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "SingleDynamicContent", Title = "Single Dynamic Content", SectionName = "MvcWidgets")]
    public class SingleDynamicContentController : Controller
    {

        #region Properties

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public SingleDynamicContentModel Model
        {
            get
            {
                if (this.model == null)
                    this.model = new SingleDynamicContentModel(); ;

                return this.model;
            }
        }

        public string ItemType
        {
            get
            {
                return this.itemType;
            }

            set
            {
                this.itemType = value;
            }
        }   

        #endregion

        #region Actions

        /// <summary>
        /// This is the default Action.
        /// </summary>
        public ActionResult Index()
        {
            try
            {
                this.Model.Populate(this.ItemType);

                return View("Default", this.Model);
            }
            catch (ArgumentException e)
            {
                return Content("Type {0} doesn't exists!".Arrange(this.ItemType));
            }
        }

        #endregion

        #region Private fields and constants

        private string itemType = "Telerik.Sitefinity.DynamicTypes.Model.Athletes.Athlete";
        private SingleDynamicContentModel model;

        #endregion
    }
}
