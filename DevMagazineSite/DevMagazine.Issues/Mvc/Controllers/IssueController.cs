using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using Telerik.Sitefinity.ContentLocations;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Mvc;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers;
using Telerik.Sitefinity.Services;
using Telerik.Sitefinity.Mvc.ActionFilters;
using DevMagazine.Issues.Mvc.Models;
using DevMagazine.Issues.Mvc.ViewModels;
using Telerik.Sitefinity.Utilities.TypeConverters;
using DevMagazine.Issues.Mvc.Models.Impl;

namespace DevMagazine.Issues.Mvc.Controllers
{
    /// <summary>
    /// Represents the Controller of the Issues widget.
    /// </summary>
    [ControllerToolboxItem(Name = "IssuesWidget", Title = "Issues Widget", SectionName = "MvcWidgets")]
    public class IssueController : Controller, IContentLocatableView
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IssueController"/> class.
        /// </summary>
        public IssueController(IIssueModel issuesModel)
        {
            this.model = issuesModel;
        }

        #endregion

        #region Properties

        public Guid IssueId { get; set; }

        /// <summary>
        /// Gets or sets the name of the template that will be displayed when widget is in List view.
        /// </summary>
        /// <value></value>
        public string ListTemplateName
        {
            get
            {
                return this.listTemplateName;
            }

            set
            {
                this.listTemplateName = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the template that will be displayed when widget is in Detail view.
        /// </summary>
        /// <value></value>
        public string DetailTemplateName
        {
            get
            {
                return this.detailTemplateName;
            }

            set
            {
                this.detailTemplateName = value;
            }
        }


        /// <summary>
        /// Gets or sets the selection mode of the Issues Widget.
        /// </summary>
        /// <value></value>
        public IssueSelectionMode SelectionMode
        {
            get
            {
                return this.selectionMode;
            }

            set
            {
                this.selectionMode = value;
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

        /// <summary>
        /// Gets or sets a value indicating whether detail view for the issue widget should be opened in the same page.
        /// </summary>
        /// <value>
        /// <c>true</c> if details link should be opened in the same page; otherwise, (if should redirect to custom selected page)<c>false</c>.
        /// </value>
        public bool OpenInSamePage
        {
            get
            {
                return this.openInSamePage;
            }

            set
            {
                this.openInSamePage = value;
            }
        }


        /// <summary>
        /// Gets or sets the page URL where will be displayed details view for selected issue widget.
        /// </summary>
        /// <value>
        /// The page URL where will be displayed details view for selected issue widget.
        /// </value>
        public string DetailsPageUrl
        {
            get
            {
                if (!this.OpenInSamePage)
                {
                    return this.detailsPageUrl;
                }
                else
                {
                    var url = this.GetCurrentPageUrl();
                    return url;
                }
            }

            set
            {
                this.detailsPageUrl = value;
            }
        }
        
        #endregion

        #region Actions


        /// <summary>
        /// Renders appropriate list view depending on the <see cref="TemplateName" />
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult" />.
        /// </returns>
        public ActionResult Index()
        {
            // Identify the correct view name
            var fullTemplateName = this.listTemplateNamePrefix + this.ListTemplateName;
            this.ViewBag.DetailsPageUrl = this.DetailsPageUrl;
            this.model.CurrentPage = 1;
            this.AddCacheDependencies(); // add the cache dependancies

            // Get the model
            this.model.PopulateModel(this.SelectionMode);

            return View(fullTemplateName, this.model);
        }

        /// <summary>
        /// Action used for ajax collection of archived issues
        /// </summary>
        /// <param name="page">The page number</param>
        /// <returns>
        /// The <see cref="ActionResult" /> that returns paged archives suitable for Ajax
        /// </returns>
        [StandaloneResponseFilter]
        public ActionResult Archive(int? page)
        {
            this.model.CurrentPage = page ?? 2;
            this.model.PopulateModel(this.SelectionMode);

            return View("List.ArchivedIssues", this.model);
        }

        /// <summary>
        /// Renders appropriate list view depending on the <see cref="DetailTemplateName"/>
        /// </summary>
        /// <param name="issue">The Dynamic Item that will be resolved by the widget.</param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult Details(DynamicContent issue)
        {
            var fullTemplateName = this.detailTemplateNamePrefix + this.DetailTemplateName;

            // return to the default action if detail view is not allowed
            if (!this.EnableDetailMode)
                return RedirectToAction("Index");

            IssueViewModel viewModel = model.GetIssue(issue);
            this.model.DetailIssue = viewModel;
            this.AddCacheDependencies(); // add the cache dependancies

            // add the issue to the page <title> tag
            this.ViewBag.Title = viewModel.Title;

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
            //if (this.SelectionMode == IssueSelectionMode.LatestIssue)
            //    return null;

            var location = new ContentLocationInfo();
            location.ContentType = IssueModel.IssueType;

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

        private IIssueModel model;
        private string listTemplateNamePrefix = "List.";
        private string listTemplateName = "Issues";
        private string detailTemplateNamePrefix = "Detail.";
        private string detailTemplateName = "Issue";
        private bool enableDetailMode = true;
        private bool? disableCanonicalUrlMetaTag;
        private string detailsPageUrl;
        private bool openInSamePage = true;
        private IssueSelectionMode selectionMode = IssueSelectionMode.LatestIssue;

        #endregion
    }
}
