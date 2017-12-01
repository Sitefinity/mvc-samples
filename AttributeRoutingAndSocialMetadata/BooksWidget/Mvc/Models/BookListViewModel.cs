using System.Collections.Generic;
using System.Text;
using System.Web;
using Telerik.Sitefinity.Services;

namespace BooksWidget.Mvc.Models
{
    public sealed class BookListViewModel
    {
        public BookListViewModel(IEnumerable<Book> items, int pageCount, int currentPage)
        {
            this._items = items;
            this._pageCount = pageCount;
            this._currentPage = currentPage;
        }

        public IEnumerable<Book> Items
        {
            get
            {
                return this._items;
            }
        }

        public int PageCount
        {
            get
            {
                return this._pageCount;
            }
        }

        public int CurrentPage
        {
            get
            {
                return this._currentPage;
            }
        }

        public string NextPageUrl
        {
            get
            {
                return this.GetPageUrl(this.CurrentPage + 1);
            }
        }

        public string PreviousPageUrl
        {
            get
            {
                return this.GetPageUrl(this.CurrentPage - 1);
            }
        }
        
        public string ResolveAbsolutePageUrl(string relativePath)
        {
            var currentPageUrl = this.GetCurrentPageUrl();
            return string.Format("{0}/{1}", currentPageUrl, relativePath);
        }
        
        private string GetPageUrl(int page)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(this.GetCurrentPageUrl());
            sb.Append("/");
            sb.Append(page);

            return sb.ToString();
        }

        private string GetCurrentPageUrl()
        {
            StringBuilder sb = new StringBuilder();

            var currentNode = SiteMap.CurrentNode;
            if (currentNode != null)
                sb.Append(currentNode.Url);
            else
                sb.Append(VirtualPathUtility.RemoveTrailingSlash(SystemManager.CurrentHttpContext.Request.Path));

            return sb.ToString();
        }

        private readonly IEnumerable<Book> _items;
        private readonly int _pageCount;
        private readonly int _currentPage;
    }
}