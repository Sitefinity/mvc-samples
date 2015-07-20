using System.Collections.Generic;
using System.Text;
using System.Web;
using Telerik.Sitefinity.Services;

namespace BooksWidget.Mvc.Models
{
    public sealed class BooksViewModel
    {
        public BooksViewModel(IEnumerable<Book> items, int pageCount, int currentPage)
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
                return this.IndexActionUrl(this.CurrentPage + 1);
            }
        }

        public string PreviousPageUrl
        {
            get
            {
                return this.IndexActionUrl(this.CurrentPage - 1);
            }
        }

        private string IndexActionUrl(int page)
        {
            StringBuilder sb = new StringBuilder();
            var currentNode = SiteMap.CurrentNode;
            if (currentNode != null)
                sb.Append(currentNode.Url);
            else
                sb.Append(VirtualPathUtility.RemoveTrailingSlash(SystemManager.CurrentHttpContext.Request.Path));
            sb.Append("/");
            sb.Append(page);

            return sb.ToString();
        }

        private readonly IEnumerable<Book> _items;
        private readonly int _pageCount;
        private readonly int _currentPage;
    }
}