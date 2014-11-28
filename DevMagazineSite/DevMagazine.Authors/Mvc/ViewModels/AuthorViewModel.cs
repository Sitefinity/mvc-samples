using DevMagazine.Core.Content;
using DevMagazine.Core.Modules.Libraries.Images.ViewModels;
using DevMagazine.Core.Modules.Shared.ViewModels;
using DevMagazine.Issues.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.News.Model;

namespace DevMagazine.Authors.Mvc.ViewModels
{
    /// <summary>
    /// View model class used by the Authors widget.
    /// </summary>
    public class AuthorViewModel : ViewModelBase
    {
        public DynamicContent Author { get; set; }

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
        /// Gets or sets the type of the author.
        /// </summary>
        /// <value>
        /// The type of the author.
        /// </value>
        public string AuthorType
        {
            get
            {
                return this.authorType;
            }
            set
            {
                this.authorType = value;
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

        #region Private fields

        private string name;
        private ImageViewModel avatar;
        private string jobTitle;
        private string bio;
        private string authorType;
        private IList<NewsItem> relatedArticles;
        private NewsItem detailedArticle;
        private IssueViewModel latestIssue;

        #endregion
    }
}
