using DevMagazine.Core.Content;
using DevMagazine.Core.Modules.Libraries.Images.ViewModels;
using DevMagazine.Core.Modules.Shared.ViewModels;
using SitefinityWebApp.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.News.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.RelatedData;
using DevMagazine.Core.Modules.Libraries.Images;
using DevMagazine.Core.Modules.Libraries.Documents;
using Telerik.Sitefinity.Utilities.TypeConverters;
using Telerik.Sitefinity.Modules.News;

namespace SitefinityWebApp.Mvc.ViewModels
{
    /// <summary>
    /// View model class used by the Authors widget.
    /// </summary>
    public class AuthorViewModel : ViewModelBase
    {
        #region Properties
        public DynamicContent Author 
        {
            get
            {
                return this.author;
            }
            set
            {
                this.author = value;
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        /// <summary>
        /// Gets or sets the avatar URL.
        /// </summary>
        /// <value>
        /// The avatar URL.
        /// </value>
        public ImageViewModel Avatar
        {
            get
            {
                if (this.avatar == null)
                {
                    this.avatar = new ImageViewModel()
                        {
                            ImageUrl = UrlHelper.GetRelatedMediaUrl(this.Author, "Avatar")
                        };
                }

                return this.avatar;
            }
            set
            {
                this.avatar = value;
            }
        }

        /// <summary>
        /// Gets or sets the job title.
        /// </summary>
        /// <value>
        /// The job title.
        /// </value>
        public string JobTitle
        {
            get
            {
                return this.jobTitle;
            }
            set
            {
                this.jobTitle = value;
            }
        }

        /// <summary>
        /// Gets or sets the bio.
        /// </summary>
        /// <value>
        /// The bio.
        /// </value>
        public string Bio
        {
            get
            {
                return this.bio;
            }
            set
            {
                this.bio = value;
            }
        }        

        /// <summary>
        /// Gets or sets the related articles.
        /// </summary>
        /// <value>
        /// The related articles.
        /// </value>
        public IList<NewsItem> RelatedArticles
        {
            get
            {
                if (this.relatedArticles == null)
                {
                    this.relatedArticles = new List<NewsItem>();
                }

                return this.relatedArticles;
            }
            set
            {
                this.relatedArticles = value;
            }
        }

        /// <summary>
        /// Gets or sets the detailed article.
        /// </summary>
        /// <value>
        /// The detailed article.
        /// </value>
        public NewsItem DetailedArticle
        {
            get
            {
                if (this.detailedArticle != null)
                {
                    return this.detailedArticle;
                }
                else
                {
                    return new NewsItem();
                }
            }
            set
            {
                this.detailedArticle = value;
            }
        }

        /// <summary>
        /// Gets or sets the latest issue.
        /// </summary>
        /// <value>
        /// The latest issue.
        /// </value>
        public IssueViewModel LatestIssue
        {
            get
            {
                return this.latestIssue;
            }
            set
            {
                this.latestIssue = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of the author.
        /// </summary>
        /// <value>
        /// The type of the author.
        /// </value>
        public static Type AuthorType
        {
            get
            {
                return TypeResolutionService.ResolveType("Telerik.Sitefinity.DynamicTypes.Model.Authors.Author");
            }
        }
        #endregion

        #region Static Methods
        /// <summary>
        /// Gets the latest issue.
        /// </summary>
        /// <returns></returns>
        public static IssueViewModel GetLatestIssue()
        {
            var dynamicManager = DynamicModuleManager.GetManager();

            var latestIssue = dynamicManager.GetDataItems(IssueViewModel.IssueType)
              .Where(di => di.Status == ContentLifecycleStatus.Live)
              .Where(di => di.Visible == true)
              .OrderByDescending(di => di.PublicationDate)
              .FirstOrDefault();

            return IssueViewModel.GetIssue(latestIssue);
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
                Author = obj,
                ItemDefaultUrl = obj.ItemDefaultUrl,
                Name = obj.GetString("Name"),
                JobTitle = obj.GetString("JobTitle"),
                Bio = obj.GetString("Bio"),
                Avatar = new ImageViewModel() { ImageUrl = UrlHelper.GetRelatedMediaUrl(obj, "Avatar") },
                ProviderName = obj.ProviderName,
                RelatedArticles = AuthorViewModel.GetRelatedArticles(obj),
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

        #region Private fields

        private string name;
        private ImageViewModel avatar;
        private string jobTitle;
        private string bio;
        private IList<NewsItem> relatedArticles;
        private NewsItem detailedArticle;
        private IssueViewModel latestIssue;
        private DynamicContent author;

        #endregion
    }
}
