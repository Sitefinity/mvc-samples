using System;
using System.Collections.Generic;
using System.Linq;
using DevMagazine.Authors.Mvc.ViewModels;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Data;

namespace DevMagazine.Authors.Mvc.Models
{
    /// <summary>
    /// Classes that implement this interface could be used as view model for the Authors widget.
    /// </summary>
    public interface IAuthorModel : ICacheDependable
    {
        /// <summary>
        /// Gets or sets the name of the provider.
        /// </summary>
        /// <value>
        /// The name of the provider.
        /// </value>
        string ProviderName { get; set; }

        /// <summary>
        /// Gets or sets the detail page URL.
        /// </summary>
        /// <value>
        /// The detail page URL.
        /// </value>
        string DetailPageUrl { get; set; }

        /// <summary>
        /// Gets the authors.
        /// </summary>
        /// <returns></returns>
        IList<DynamicContent> Authors { get; }

        /// <summary>
        /// Gets or sets the detail author
        /// </summary>
        /// <returns></returns>
        AuthorViewModel DetailAuthor { get; set; }

        /// <summary>
        /// Populates the authors.
        /// </summary>
        /// <param name="page">The page.</param>
        void PopulateAuthors(int? page);

        /// <summary>
        /// Gets the detail URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        string GetDetailUrl(string url);

        /// <summary>
        /// Gets the authors view model.
        /// </summary>
        /// <param name="authorsModel">The authors model.</param>
        /// <returns>Author view models</returns>
        IList<AuthorViewModel> GetAuthorsViewModel();
    }
}
