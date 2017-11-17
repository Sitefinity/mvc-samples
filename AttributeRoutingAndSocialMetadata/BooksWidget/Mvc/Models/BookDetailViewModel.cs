using System;

namespace BooksWidget.Mvc.Models
{
    public sealed class BookDetailViewModel
    {
        public BookDetailViewModel(Book book)
        {
            this.Id = book.Id;
            this.Title = book.Title;
            this.Description = book.Description;
            this.Author = book.Author;
            this.ImageUrl = book.ImageUrl;
            this.Points = book.Points;
        }

        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Author { get; set; }

        public string ImageUrl { get; set; }

        public int Points { get; set; }
    }
}
