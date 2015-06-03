
namespace BooksWidget.Mvc.Models
{
    public sealed class Book
    {
        public Book(string author, string title)
        {
            this._author = author;
            this._title = title;
            this._points = 0;
        }

        public string Author
        {
            get
            {
                return this._author;
            }
        }

        public string Title
        {
            get
            {
                return this._title;
            }
        }

        public int Points
        {
            get
            {
                return this._points;
            }
        }

        public void Vote()
        {
            this._points++;
        }

        private readonly string _title;
        private readonly string _author;
        private volatile int _points;
    }
}