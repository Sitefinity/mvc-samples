using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Telerik.Sitefinity.Mvc;

namespace ListWidget.Mvc.Controllers
{
    /// <summary>
    /// This class represents controller of the SampleList widget
    /// </summary>
    [ControllerToolboxItem(Name = "SampleList", SectionName = "Feather samples", Title = "Sample List")]
    public class ListController : Controller
    {
        #region Public properties

        /// <summary>
        /// Gets or sets the list title.
        /// </summary>
        /// <value>
        /// The list title.
        /// </value>
        public string ListTitle
        {
            get
            {
                return this.listTitle;
            }

            set
            {
                this.listTitle = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of the html elements used for the list.
        /// </summary>
        /// <value>
        /// The type of the list.
        /// </value>
        public ListMode ListType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the list items.
        /// </summary>
        /// <value>
        /// The list items.
        /// </value>
        public string ListItems
        {
            get
            {
                return this.listItems;
            }

            set
            {
                this.listItems = value;
            }
        }

        #endregion 

        #region Actions

        /// <summary>
        /// The default action
        /// </summary>
        /// <returns>The default widget view</returns>
        public ActionResult Index()
        {
            this.ViewBag.ListTitle = this.ListTitle;
            this.ViewBag.ListType = this.ListType;
            this.ViewBag.ListItems = this.DeserializeItems();

            return this.View();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Deserialize the items.
        /// </summary>
        /// <returns>The list of items</returns>
        private IList<string> DeserializeItems()
        {
            var serializer = new JavaScriptSerializer();
            IList<string> items = new List<string>();

            if (!string.IsNullOrEmpty(this.ListItems))
                items = serializer.Deserialize<IList<string>>(this.ListItems);

            return items;
        }

        #endregion

        #region Private fields and constants

        /// <summary>
        /// The list title
        /// </summary>
        private string listTitle = "My list title";

        /// <summary>
        /// The list items
        /// </summary>
        private string listItems = "[\"First Item\", \"Second Item\", \"Third Item\"]";

        #endregion
    }
}