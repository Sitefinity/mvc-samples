using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.Services.Search.Data;

namespace DevMagazine.Search.Mvc.ViewModels
{
    /// <summary>
    /// A view model for representing the search result items
    /// </summary>
    public class SearchResultsViewModel
    {
        #region Constructors

        public SearchResultsViewModel()
        {
            this.Term = String.Empty;
            this.Results = new List<IDocument>();
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the searched term
        /// </summary>
        public string Term { get; set; }

        /// <summary>
        /// Gets or sets the number of search result hits
        /// </summary>
        public int Hits { get; set; }

        /// <summary>
        /// Gets or sets the current page nymber
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets the total number of pages
        /// </summary>
        public int TotalPagesCount { get; set; }

        /// <summary>
        /// Gets or sets the returned documents
        /// </summary>
        public IEnumerable<IDocument> Results { get; set; }

        #endregion
    }
}
