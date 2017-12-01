using System.Collections.Generic;

namespace KendoBooksWidget.Mvc.Models
{
    public sealed class BooksViewModel
    {
        public BooksViewModel()
        {
            this._items = new List<Book>();
        }

        public BooksViewModel(IEnumerable<Book> items, int pageCount, int currentPage)
        {
            this._items = items;
        }

        public IEnumerable<Book> Items
        {
            get
            {
                return this._items;
            }
        }

        private readonly IEnumerable<Book> _items;
    }
}