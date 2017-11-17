using System;
using Telerik.Sitefinity.Model;

namespace BooksWidget.Mvc.Models
{
    public sealed class Book : IDataItem
    {
        public Book(string id, string author, string title, string description, string imageUrl)
        {
            this.Id = Guid.Parse(id);

            this._author = author;
            this._title = title;
            this._description = description;            
            this._imageUrl = string.Format("/Frontend-Assembly/BooksWidget/Images/{0}", imageUrl);            
            this._points = 0;
        }

        public string Title
        {
            get
            {
                return this._title;
            }
        }

        public string Author
        {
            get
            {
                return this._author;
            }
        }

        public string Description
        {
            get
            {
                return this._description;
            }
        }

        public string ImageUrl
        {
            get
            {
                return this._imageUrl;
            }
        }

        public int Points
        {
            get
            {
                return this._points;
            }
        }

        #region IDataItem

        public Guid Id { get; set; }

        public object Transaction { get; set; }

        public object Provider { get; set; }

        public DateTime LastModified { get; set; }

        public string ApplicationName { get; set; }

        #endregion

        public void Vote()
        {
            this._points++;
        }

        private readonly string _title;
        private readonly string _description;
        private readonly string _author;
        private readonly string _imageUrl;
        private volatile int _points;
    }
}