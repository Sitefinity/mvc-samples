using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Telerik.Sitefinity.ContentLocations;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Mvc;
using Telerik.Sitefinity.Services;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers;
using DevMagazine.Authors.Mvc.Models;
using DevMagazine.Authors.Mvc.Models.Impl;

namespace DevMagazine.Authors.Mvc.Controllers
{
    /// <summary>
    /// Represents the Controller of the Authors widget.
    /// </summary>
    [ControllerToolboxItem(Name = "AuthorWidget", Title = "AuthorWidget", SectionName = "MvcWidgets")]
    public class AuthorController : Controller, IContentLocatableView
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorController"/> class.
        /// </summary>
        public AuthorController(IAuthorModel authorsModel)
        {
            this.model = authorsModel;
            this.model.DetailPageUrl = this.detailPageUrl;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the name of the template that will be displayed when widget is in List view.
        /// </summary>
        /// <value></value>
        public string TemplateName
        {
            get
            {
                return this.templateName;
            }
            set
            {
                this.templateName = value;
            }
        }

        /// <summary>
        /// Gets or sets the whether the detail mode would be enabled.
        /// </summary>
        /// <value></value>
        public bool EnableDetailMode
        {
            get
            {
                return this.enableDetailMode;
            }

            set
            {
                this.enableDetailMode = value;
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

        #endregion

        #region Actions

        /// <summary>
        /// Renders appropriate list view depending on the <see cref="TemplateName" />
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns>
        /// The <see cref="ActionResult" />.
        /// </returns>
        public ActionResult Index(int? page)
        {
            var fullTemplateName = this.listTemplateNamePrefix + this.TemplateName;
            this.model.PopulateAuthors(page);
            var viewModel = this.model.GetAuthorsViewModel();
            this.AddCacheDependencies(); // add cache dependancies

            return this.View(fullTemplateName, viewModel);
        }

        /// <summary>
        /// Renders appropriate list view depending on the <see cref="DetailTemplateName"/>
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult Details(DynamicContent author)
        {
            var fullTemplateName = this.detailTemplateNamePrefix + this.TemplateName;
            this.model.Authors.Add(author);
            var viewModel = this.model.GetAuthorsViewModel().FirstOrDefault();

            this.model.DetailAuthor = viewModel;
            this.AddCacheDependencies(); // add cache dependancies

            return this.View(fullTemplateName, viewModel);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets the information for all of the content types that a control is able to show.
        /// </summary>
        /// <returns>
        /// List of location info of the content that this control is able to show.
        /// </returns>
        [NonAction]
        public IEnumerable<IContentLocationInfo> GetLocations()
        {
            //TODO - Fix when controll context bug is fixed
            // Currently, the properties of the widget are not get correctly
            //if (this.EnableDetailMode == false)
            //    return null;

            var location = new ContentLocationInfo();
            location.ContentType = AuthorModel.AuthorType;

            // TODO add the ProviderName and the filter expressiosn as parameters
            location.ProviderName = "OpenAccessProvider";

            var filterExpression = "";
            if (!string.IsNullOrEmpty(filterExpression))
            {
                location.Filters.Add(new BasicContentLocationFilter(filterExpression));
            }

            return new[] { location };
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Adds the cache dependencies.
        /// </summary>
        private void AddCacheDependencies()
        {
            if (SystemManager.CurrentHttpContext != null)
            {
                this.AddCacheDependencies(this.model.GetKeysOfDependentObjects());
            }
        }

        #endregion

        #region Private fields and constants

        private IAuthorModel model;
        private bool enableDetailMode = true;
        private bool? disableCanonicalUrlMetaTag;
        private string templateName = "Authors";
        private string listTemplateNamePrefix = "List.";
        private string detailTemplateNamePrefix = "Detail.";
        private string detailPageUrl = "authors";

        #endregion
    }
}
