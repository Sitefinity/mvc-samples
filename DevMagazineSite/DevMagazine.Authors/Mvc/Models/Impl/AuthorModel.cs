using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Data;
using DevMagazine.Authors.Mvc.ViewModels;
using Telerik.Sitefinity.Utilities.TypeConverters;
using DevMagazine.Core.Modules.Libraries.Documents.Models;
using DevMagazine.Core.Modules.Libraries.Images.Models;
using DevMagazine.Core.Modules.Libraries.Images.ViewModels;
using Telerik.Sitefinity.News.Model;
using Telerik.Sitefinity.Modules.News;
using DevMagazine.Core.Content;
using DevMagazine.Issues.Mvc.ViewModels;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.RelatedData;

namespace DevMagazine.Authors.Mvc.Models.Impl
{
    /// <summary>
    /// This class represents an abstraction used by the model of the Authors widget.
    /// </summary>
    public class AuthorModel : IAuthorModel
    {
        #region Constructors

        public AuthorModel(IImagesModel imageModel, IDocumenstModel documentModel)
        {
            this.imageModel = imageModel;
            this.documentModel = documentModel;
        }

        #endregion

        #region Properties

        /// <inheritdoc />
        public string ProviderName
        {
            get;
            set;
        }

        /// <inheritdoc />
        public string DetailPageUrl
        {
            get;
            set;
        }
        
        /// <inheritdoc />
        public IList<DynamicContent> Authors
        {
            get
            {
                if (this.authors == null)
                {
                    this.PopulateAuthors(null);
                }

                return this.authors;
            }
            private set
            {
                this.authors = value;
            }
        }

        /// <inheritdoc />
        public AuthorViewModel DetailAuthor
        {
            get
            {
                return this.detailAuthor;
            }
            set
            {
                this.detailAuthor = value;
            }
        }

        public static Type AuthorType
        {
            get
            {
                return TypeResolutionService.ResolveType("Telerik.Sitefinity.DynamicTypes.Model.Authors.Author");
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Populates the authors.
        /// </summary>
        /// <param name="numberAuthors">The number authors.</param>
        public virtual void PopulateAuthors(int? numberAuthors)
        {
            DynamicModuleManager manager = DynamicModuleManager.GetManager(this.ProviderName);

            var authors = manager.GetDataItems(AuthorModel.AuthorType)
                .Where(a => a.Status == ContentLifecycleStatus.Live)
                .Take(numberAuthors.GetValueOrDefault(6));

            this.authors = authors.ToArray();
        }

        /// <summary>
        /// Forms the detail URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public string GetDetailUrl(string url)
        {
            return this.DetailPageUrl + url;
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
            if (this.DetailAuthor != null && this.DetailAuthor.Id != Guid.Empty)
            {
                result.Add(new CacheDependencyKey { Key = this.DetailAuthor.Id.ToString(), Type = typeof(DynamicContent) });
            }
            else
            {
                result.Add(new CacheDependencyKey { Key = AuthorModel.AuthorType.FullName, Type = typeof(DynamicContent) });
            }

            return result;
        }

        /// <summary>
        /// Gets the authors view model.
        /// </summary>
        /// <param name="authorsModel">The authors model.</param>
        /// <returns>Author view models</returns>
        public IList<AuthorViewModel> GetAuthorsViewModel()
        {
            var dynamicManager = DynamicModuleManager.GetManager();
            var latestIssue = dynamicManager.GetDataItems(DevMagazine.Issues.Mvc.Models.Impl.IssueModel.IssueType)
              .Where(di => di.Status == ContentLifecycleStatus.Live)
              .Where(di => di.Visible == true)
              .OrderByDescending(di => di.PublicationDate).FirstOrDefault();

            return this.Authors.Select(author => new AuthorViewModel()
            {
                Author = author,
                Id = author.Id,
                Name = author.GetString("Name"),
                JobTitle = author.GetString("JobTitle"),
                Bio = author.GetString("Bio"),
                UrlName = this.GetDetailUrl(author.ItemDefaultUrl),
                Avatar = new ImageViewModel() { ImageUrl = UrlHelper.GetRelatedMediaUrl(author, "Avatar") },
                ProviderName = this.ProviderName,
                AuthorType = AuthorModel.AuthorType.FullName,
                RelatedArticles = AuthorModel.GetRelatedArticles(author),
                LatestIssue = this.GetIssue(latestIssue)
            })
            .ToList();
        }

        /// <summary>
        /// Gets the issue.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public IEnumerable<IssueViewModel> GetIssue(DynamicContent item)
        {
            IssueViewModel issue = new IssueViewModel();

            issue.Title = item.GetString("Title");
            issue.Id = item.Id;
            issue.UrlName = item.ItemDefaultUrl;
            issue.Description = item.GetString("Description");
            issue.Number = item.GetString("IssueNumber");
            issue.Cover = imageModel.GetRelatedImage(item, "IssueCover");
            issue.PrintedVersion = documentModel.GetRelatedDocument(item, "IssueDocument");
            issue.Articles = item.GetRelatedItems<NewsItem>("Articles");
            issue.FeaturedArticle = item.GetRelatedItems<NewsItem>("FeaturedArticle");

            return new List<IssueViewModel>() { issue };
        }

        /// <summary>
        /// Gets the author view model.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="currentNewsItem">The current news item.</param>
        /// <returns>Author View Moddel</returns>
        public static AuthorViewModel GetAuthorViewModel(DynamicContent obj, NewsItem currentNewsItem = null)
        {
            return new AuthorViewModel
            {
                Id = obj.Id,
                ItemDefaultUrl = obj.ItemDefaultUrl,
                Name = obj.GetString("Name"),
                JobTitle = obj.GetString("JobTitle"),
                Bio = obj.GetString("Bio"),
                Avatar = new ImageViewModel() { ImageUrl = UrlHelper.GetRelatedMediaUrl(obj, "Avatar") },
                ProviderName = obj.ProviderName,
                AuthorType = AuthorModel.AuthorType.FullName,
                RelatedArticles = AuthorModel.GetRelatedArticles(obj),
                DetailedArticle = (currentNewsItem != null) ? currentNewsItem : null
            };
        }

        /// <summary>
        /// Gets the related articles
        /// </summary>
        /// <param name="obj">The Dynamic Content.</param>
        /// <returns>IList of NewsItem</returns>
        public static IList<NewsItem> GetRelatedArticles(DynamicContent obj)
        {
            IList<NewsItem> relatedArticles = new List<NewsItem>();
            var relatedDataItems = obj.GetRelatedParentItems(typeof(NewsItem).FullName);

            foreach (var item in relatedDataItems)
            {
                NewsManager newsManager = NewsManager.GetManager();
                var newsItem = newsManager.GetNewsItem(item.Id);
                if (newsItem != null)
                    relatedArticles.Add(newsItem);
            }

            return relatedArticles;
        }

        #endregion

        #region Private fields and constants

        private readonly IImagesModel imageModel;
        private readonly IDocumenstModel documentModel;
        private IList<DynamicContent> authors = new List<DynamicContent>();
        private AuthorViewModel detailAuthor;
       
        #endregion
    }
}
