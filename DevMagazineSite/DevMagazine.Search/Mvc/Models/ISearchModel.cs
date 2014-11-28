using DevMagazine.Search.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.Services.Search;
using Telerik.Sitefinity.Services.Search.Data;

namespace DevMagazine.Search.Mvc.Models
{
    /// <summary>
    /// Classes that implement this interface could be used as view model for the Search widget.
    /// </summary>
    public interface ISearchModel
    {
        /// <summary>
        /// Builds SearchResultsViewModel based on query and page number
        /// </summary>
        /// <param name="query">The search term</param>
        /// <param name="page">The page number</param>
        /// <returns>SearchResultsViewModel used for displaying search results</returns>
        SearchResultsViewModel GetSearhResultsModel(string query, int? page);

        /// <summary>
        /// Returns a collection of documents using the given parameters.
        /// </summary>
        /// <param name="query">The search query.</param>
        /// <param name="catalogue">The search catalogue.</param>
        /// <param name="skip">Items to skip.</param>
        /// <param name="take">Items to take.</param>
        /// <param name="hitCount">Number of result documents.</param>
        /// <returns></returns>
        IEnumerable<IDocument> Search(string query, string catalogue, int skip, int take, out int hitCount);

        /// <summary>
        /// Builds a formatted query from the passed <paramref name="searchQuery" /> using the search service.
        /// The result query will be used by the search service to perform a search.
        /// Override this method if you want to build a custom query.
        /// </summary>
        /// <param name="searchQuery">The search query.</param>
        /// <returns></returns>
        ISearchQuery BuildSearchQuery(string searchQuery);

        /// <summary>
        /// Gets or sets the fields which will be used in the search.
        /// </summary>
        /// <value>The fields used in the search.</value>
        string[] SearchFields { get; set; }

        /// <summary>
        /// Gets or sets the fields that will be used when generating a search summary for particular document.
        /// </summary>
        /// <value>The fields used in search summary.</value>
        string[] HighlightedFields { get; set; }

        /// <summary>
        /// Gets or set the name of the catalog used for the search query
        /// </summary>
        /// <value>The name of the search catalog</value>
        string CatalogName { get; set; }


        /// <summary>
        /// If paging is allowed, this property specifies how many posts per page should
        /// be displayed.
        /// </summary>
        int ItemsPerPage { get; set; }
    }
}
