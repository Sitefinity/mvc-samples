using DevMagazine.Search.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Telerik.Sitefinity.Services;
using Telerik.Sitefinity.Services.Search;
using Telerik.Sitefinity.Services.Search.Data;
using Telerik.Sitefinity.Utilities.TypeConverters;
using Telerik.Sitefinity.Services.Search;

namespace DevMagazine.Search.Mvc.Models.Impl
{
    /// <inheritdoc />
    public class SearchModel : ISearchModel
    {
        #region Constructors

        public SearchModel()
        {
            this.searchService = ServiceBus.ResolveService<ISearchService>();
            this.ItemsPerPage = 10;
            this.SearchFields = new[] { "Title", "Content" };
            this.HighlightedFields = new[] { "Title", "Content" };
        }

        #endregion

        #region Public properties

        /// <inheritdoc />
        public string CatalogName { get; set; }

        /// <inheritdoc />
        [TypeConverter(typeof(StringArrayConverter))]
        public string[] SearchFields { get; set; }

        /// <inheritdoc />
        [TypeConverter(typeof(StringArrayConverter))]
        public string[] HighlightedFields { get; set; }

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

        #endregion

        #region Public methods

        /// <inheritdoc />
        public SearchResultsViewModel GetSearhResultsModel(string query, int? page)
        {
            SearchResultsViewModel resultsModel = new SearchResultsViewModel();
            int pageNum = page ?? 1;
            int hits = 0;
            resultsModel.Term = query;
            resultsModel.Results = this.Search(query, this.CatalogName, (pageNum - 1) * this.ItemsPerPage, ItemsPerPage, out hits);
            resultsModel.Hits = hits;
            resultsModel.CurrentPage = pageNum;
            resultsModel.TotalPagesCount = (int)Math.Ceiling((double)hits / this.ItemsPerPage);

            // return the model
            return resultsModel;
        }


        /// <inheritdoc />
        public IEnumerable<IDocument> Search(string query, string catalog, int skip, int take, out int hitCount)
        {
            var compiledQuery = this.BuildSearchQuery(query);

            IResultSet result = searchService.Search(
                catalog,
                compiledQuery,
                this.HighlightedFields,
                skip,
                take,
                null);

            hitCount = result.HitCount;
          
            return result.SetContentLinks();
        }

        /// <inheritdoc />
        public string BuildSearchQuery(string searchQuery)
        {
            var compiledQuery = searchService.BuildQuery(searchQuery, this.SearchFields, SystemManager.CurrentContext.AppSettings.Multilingual);
            return compiledQuery;
        }

        #endregion

        #region Private fields

        private readonly ISearchService searchService;
        private int itemsPerPage = 10;

        #endregion
    }
}
