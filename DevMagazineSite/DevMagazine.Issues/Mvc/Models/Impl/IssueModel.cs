using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.RelatedData;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.News.Model;
using DevMagazine.Core.Modules.Libraries.Documents.Models;
using DevMagazine.Core.Modules.Libraries.Images.Models;
using DevMagazine.Core.Modules.Libraries.Images.ViewModels;
using DevMagazine.Core.Exceptions;
using Telerik.Sitefinity.Data;
using DevMagazine.Issues.Mvc.ViewModels;
using Telerik.Sitefinity.Utilities.TypeConverters;

namespace DevMagazine.Issues.Mvc.Models.Impl
{
    /// <summary>
    /// This class represents an abstraction used by the model of the Issues widget.
    /// </summary>
    public class IssueModel : IIssueModel
    {
        #region Constructors

        public IssueModel(IImagesModel imageModel, IDocumenstModel documentModel)
        {
            this.imageModel = imageModel;
            this.documentModel = documentModel;
            this.InitializeManager();
        }

        #endregion

        #region Public properties

        /// <inheritdoc />
        public IEnumerable<IssueViewModel> Issues
        {
            get
            {
                return this.issues;
            }

            private set
            {
                this.issues = value;
            }
        }


        /// <inheritdoc />
        public IssueViewModel DetailIssue
        {
            get
            {
                return this.detailIssue;
            }

            set
            {
                this.detailIssue = value;
            }
        }

        /// <inheritdoc />
        public string ProviderName
        {
            get;
            set;
        }


        /// <inheritdoc />
        public int ItemsPerPage
        {
            get
            {
                return this.itemsPerPage;
            }
            set
            {
                this.itemsPerPage = value;
            }
        }

        /// <inheritdoc />
        public int InitialItems
        {
            get
            {
                return this.initialItems;
            }
            set
            {
                this.initialItems = value;
            }
        }

        /// <inheritdoc />
        public int? TotalPagesCount { get; set; }

        /// <inheritdoc />
        public int CurrentPage { get; set; }

        /// <inheritdoc />
        public string DetailPageUrl
        {
            get;
            set;
        }

        public static Type IssueType
        {
            get
            {
                return TypeResolutionService.ResolveType("Telerik.Sitefinity.DynamicTypes.Model.Issues.Issue");
            }
        }
        
        #endregion

        #region Public Methods

        /// <inheritdoc />
        public virtual void PopulateModel(IssueSelectionMode selectionMode)
        {
            IQueryable<DynamicContent> issues = this.dynamicManager.GetDataItems(IssueModel.IssueType);

            var filterExpression = string.Format("Visible = true AND Status = Live");
            var sortingExpression = "PublicationDate DESC";
            int? totalCount = 0;
            int? skip = this.CurrentPage > 1 ? (this.InitialItems + (this.CurrentPage - 2) * this.ItemsPerPage) : 0;
            int? take = this.CurrentPage > 1 ? this.ItemsPerPage : this.InitialItems;

            issues = DataProviderBase.SetExpressions(issues, filterExpression, sortingExpression, skip, take, ref totalCount);

            // if we need the latest issue
            if (selectionMode == IssueSelectionMode.LatestIssue)
                issues = issues.Take(1);

            this.Issues = issues.ToList()
                .Select(item => this.GetIssue(item));

            // set the number of pages
            this.TotalPagesCount = (totalCount > this.InitialItems) ? (int)(Math.Ceiling((double)(totalCount - 7) / this.ItemsPerPage) + 1) : 1;
        }

        /// <inheritdoc />
        public IssueViewModel GetIssue(DynamicContent item)
        {
            IssueViewModel issue = new IssueViewModel();

            issue.Title = item.GetString("Title");
            issue.Id = item.Id;
            issue.UrlName = item.ItemDefaultUrl;
            issue.Description = item.GetString("Description");
            issue.Number = item.GetString("IssueNumber");
            issue.Cover = new ImageViewModel();
            issue.Cover = imageModel.GetRelatedImage(item, "IssueCover");
            issue.ProviderName = item.ProviderName;
            issue.PrintedVersion = documentModel.GetRelatedDocument(item, "IssueDocument");
            issue.Articles = item.GetRelatedItems<NewsItem>("Articles");
            issue.FeaturedArticle = item.GetRelatedItems<NewsItem>("FeaturedArticle");

            return issue;
        }

        /// <inheritdoc />
        public IssueViewModel GetIssue(Guid id)
        {
            var issue = dynamicManager.GetDataItems(IssueModel.IssueType)
                .Where(d => d.Status == ContentLifecycleStatus.Live)
                .FirstOrDefault();

            return this.GetIssue(issue);
        }

        /// <inheritdoc />
        public IssueViewModel GetIssue(string urlName, ref bool found)
        {
            string redirectUrl = null;
            found = true;

            // TODO try to use the GetItemFromUrl approach
            //DynamicContent item = dynamicManager.Provider.GetItemFromUrl(DynamicContent, urlName, out redirectUrl) as DynamicContent;
            DynamicContent item = dynamicManager.GetDataItems(IssueModel.IssueType)
                .Where(i => i.UrlName == urlName)
                .Where(i => i.Status == Telerik.Sitefinity.GenericContent.Model.ContentLifecycleStatus.Live)
                .FirstOrDefault();

            if (item == null)
            {
                found = false;
                return new IssueViewModel();
            }

            return this.GetIssue(item);
        }

        /// <summary>
        /// Gets a collection of <see cref="CacheDependencyNotifiedObject"/>.
        ///     The <see cref="CacheDependencyNotifiedObject"/> represents a key for which cached items could be subscribed for
        ///     notification.
        ///     When notified, all cached objects with dependency on the provided keys will expire.
        /// </summary>
        /// <returns>
        /// The <see cref="IList"/>.
        /// </returns>
        public virtual IList<CacheDependencyKey> GetKeysOfDependentObjects()
        {
            var result = new List<CacheDependencyKey>(1);
            if (this.DetailIssue != null && this.DetailIssue.Id != Guid.Empty)
            {
                result.Add(new CacheDependencyKey { Key = this.DetailIssue.Id.ToString(), Type = typeof(DynamicContent) });
            }
            else
            {
                result.Add(new CacheDependencyKey { Key = IssueModel.IssueType.FullName, Type = typeof(DynamicContent) });
            }

            return result;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Initializes the manager.
        /// </summary>
        private void InitializeManager()
        {
            DynamicModuleManager dynamicManager;

            // try to resolve manager with control definition provider
            dynamicManager = this.ResolveManagerWithProvider(this.ProviderName);
            if (dynamicManager == null)
            {
                dynamicManager = this.ResolveManagerWithProvider(null);
            }

            this.dynamicManager = dynamicManager;
        }

        /// <summary>
        /// Resolves the dynamic manager with provider.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <returns></returns>
        private DynamicModuleManager ResolveManagerWithProvider(string providerName)
        {
            try
            {
                return DynamicModuleManager.GetManager(providerName);
            }
            catch (Exception)
            {
                throw new ManagerNullException("A null manager case was encountered", typeof(DynamicModuleManager));
            }
        }


        private string AddLiveFilterExpression(string filterExpression)
        {
            if (filterExpression.IsNullOrEmpty())
            {
                filterExpression = "Visible = true AND Status = Live";
            }
            else
            {
                filterExpression = filterExpression + " AND Visible = true AND Status = Live";
            }

            return filterExpression;
        }

        #endregion

        #region Private fields

        private IEnumerable<IssueViewModel> issues = new List<IssueViewModel>();
        private int itemsPerPage = 3;
        private int initialItems = 7;
        private IssueViewModel detailIssue;
        private readonly IImagesModel imageModel;
        private readonly IDocumenstModel documentModel;
        private DynamicModuleManager dynamicManager;

        #endregion
    }
}
