using DevMagazine.Search.Mvc.Models;
using DevMagazine.Search.Mvc.ViewModels;
using System;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;
using Telerik.Sitefinity.Services.Search;

namespace DevMagazine.Search.Mvc.Controllers
{
    /// <summary>
    /// Represents the Controller of the Search widget.
    /// </summary>
    [ControllerToolboxItem(Name = "SearchWidget", Title = "Search Widget", SectionName = "MvcWidgets")]
    public class SearchController : Controller
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchController"/> class.
        /// </summary>
        public SearchController(ISearchModel searchModel)
        {
            this.model = searchModel;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Search widget model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ISearchModel Model
        {
            get
            {
                return this.model;
            }
        }

        #endregion

        #region Actions


        /// <summary>
        ///  Renders appropriate list view depending on the <see cref="TemplateName" />
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult" />.
        /// </returns>
        public ActionResult Index(int? page, string query = null)
        {
            // Identify the correct view name
            var fullTemplateName = "SearchResults.Default";

            var model = new SearchResultsViewModel();

            // Get the model
            if (!String.IsNullOrEmpty(query))
                model = this.model.GetSearhResultsModel(query, page);

            return View(fullTemplateName, model);
        }

        #endregion


        /// <summary>
        /// Returns an instance of the <see cref="T:Telerik.Sitefinity.Services.Search.ISearchService"/>.
        /// </summary>
        /// <returns></returns>
        protected ISearchService GetSearchService()
        {
            return Telerik.Sitefinity.Services.ServiceBus.ResolveService<ISearchService>();
        }


        #region Private fields and constants

        private ISearchModel model;
        private string searchBoxModePrefix = "SearchBox.";
        private string searchBoxModeName = "Default";
        private string resultsModePrefix = "SearchResults.";
        private string resultsModeName = "Default";

        #endregion
    }
}
