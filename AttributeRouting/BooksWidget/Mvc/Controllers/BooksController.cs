using BooksWidget.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace BooksWidget.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "Books", SectionName = "Samples", Title = "Books")]
    public class BooksController : Controller
    {
        [RelativeRoute("{page:int:min(1)?}")]
        public ActionResult Index(int? page)
        {
            IEnumerable<Book> items = BooksController._library;
            if (page.HasValue)
                items = items.Skip((page.Value - 1) * BooksController.PageSize);

            items = items.Take(BooksController.PageSize);

            var viewModel = new BooksViewModel(items, (int)Math.Ceiling(BooksController._library.Count / (double)BooksController.PageSize), page ?? 1);
            return this.View(viewModel);
        }

        [Route("web-interface/books/points/{page:int:min(1)?}")]
        public JsonResult Points(int? page)
        {
            IEnumerable<int> points = BooksController._library.Select(book => book.Points);
            if (page.HasValue)
                points = points.Skip((page.Value - 1) * BooksController.PageSize);

            points = points.Take(BooksController.PageSize);
            return this.Json(points, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, Route("web-interface/books/vote/{id:int:min(0):max(9)}")]
        public JsonResult Vote(int id)
        {
            var book = BooksController._library[id];
            book.Vote();

            return this.Json(book.Points, JsonRequestBehavior.DenyGet);
        }

        private const int PageSize = 5;

        private static List<Book> _library = new List<Book>(10)
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
