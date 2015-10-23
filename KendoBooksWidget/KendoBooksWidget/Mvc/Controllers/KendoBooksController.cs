using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using KendoBooksWidget.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace KendoBooksWidget.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "KendoBooks", SectionName = "Feather samples", Title = "Kendo Books")]
    public class KendoBooksController : Controller
    {
        public ActionResult Index()
        {
            return this.View(new BooksViewModel());
        }

        [HttpPost, Route("web-interface/books/")]
        public ActionResult Books([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<Book> books = KendoBooksController._library;
            books = books.Skip((request.Page - 1) * request.PageSize);

            books = books.Take(request.PageSize);
            var dataSourceResult = new DataSourceResult()
            {
                Data = books.ToList(),
                Total = KendoBooksController._library.Count
            };

            return Json(dataSourceResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, Route("web-interface/books/vote/{title}")]
        public JsonResult Vote(string title)
        {
            var matchingBooks = KendoBooksController._library.Where(b=>b.Title == title);

            foreach (var book in matchingBooks)
            {
                book.Vote();
            }

            return this.Json(matchingBooks.First().Points, JsonRequestBehavior.DenyGet);
        }

        private static readonly List<Book> _library = new List<Book>(10)
            {
                new Book("Beatrix Potter", "The Tale Of Peter Rabbit"),
                new Book("Julia Donaldson", "The Gruffalo"),
                new Book("Michael Rosen", "We're Going on a Bear Hunt"),
                new Book("Judith Kerr", "The Tiger Who Came to Tea"),
                new Book("AA Milne", "Winnie the Pooh"),
                new Book("Enid Blyton", "The Enchanted Wood"),
                new Book("Jill Murphy", "The Worst Witch"),
                new Book("Roald Dahl", "Charlie and the Chocolate Factory"),
                new Book("Jacqueline Wilson", "The Story of Tracy Beaker"),
                new Book("Michelle Magorian", "Goodnight Mister Tom")
            };
    }
}
