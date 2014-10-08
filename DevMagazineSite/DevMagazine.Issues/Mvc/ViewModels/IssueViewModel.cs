using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.News.Model;
using DevMagazine.Core.Modules.Libraries.Documents.ViewModels;
using DevMagazine.Core.Modules.Shared.ViewModels;
using DevMagazine.Core.Modules.Libraries.Images.ViewModels;

namespace DevMagazine.Issues.Mvc.ViewModels
{
    /// <summary>
    /// A view model for representing the Issues dynamic type
    /// </summary>
    public class IssueViewModel : ViewModelBase
    {
        #region Properties

        /// <summary>
        /// The title of the Issue
        /// </summary>
        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                this.title = value;
            }
        }

        /// <summary>
        /// The issue number, i.e. "Special Christmas Issue" or "November, 2014"
        /// </summary>
        public string Number
        {
            get
            {
                return this.number;
            }

            set
            {
                this.number = value;
            }
        }

        /// <summary>
        /// The description of the issue
        /// </summary>
        public string Description
        {
            get
            {
                return this.description;
            }

            set
            {
                this.description = value;
            }
        }

        /// <summary>
        /// The cover photo of the issue
        /// </summary>
        public ImageViewModel Cover {
            get
            {
                if (this.cover == null)
                {
                    this.cover = new ImageViewModel();
                }
                return this.cover;
            }

            set
            {
                this.cover = value;
            }
        }

        /// <summary>
        /// The printed version of the issue
        /// </summary>
        public DocumentViewModel PrintedVersion
        {
            get
            {
                if (this.printedVersion == null)
                {
                    this.printedVersion = new DocumentViewModel();
                }
                return this.printedVersion;
            }

            set
            {
                this.printedVersion = value;
            }
        }

        /// <summary>
        /// Represents the collection of articles associated with the issue
        /// </summary>
        public IQueryable<NewsItem> Articles
        {
            get
            {
                if (this.articles == null)
                {
                    this.articles = new List<NewsItem>().AsQueryable();
                }
                return this.articles;
            }

            set
            {
                this.articles = value;
            }
        }

        /// <summary>
        /// Represents the featured article for that issue
        /// </summary>
        public IQueryable<NewsItem> FeaturedArticle
        {
            get
            {
                if (this.featuredArticle == null)
                {
                    this.featuredArticle = new List<NewsItem>().AsQueryable();
                }
                return this.featuredArticle;
            }

            set
            {
                this.featuredArticle = value;
            }
        }

        #endregion

        #region Private fields

        private string title;
        private string number;
        private string description;
        private ImageViewModel cover;
        private DocumentViewModel printedVersion;
        private IQueryable<NewsItem> articles;
        private IQueryable<NewsItem> featuredArticle;

        #endregion
    }
}
