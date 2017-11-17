using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BooksWidget.Mvc.Models;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers;
using Telerik.Sitefinity.Mvc;

namespace BooksWidget.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "Books", SectionName = "Feather samples", Title = "Books")]
    public class BooksController : ContentBaseController
    {
        [RelativeRoute("{page:int:min(1)?}")]
        public ActionResult Index(int? page)
        {
            IEnumerable<Book> items = BooksController._library;

            if (page.HasValue)
                items = items.Skip((page.Value - 1) * BooksController.PageSize);

            items = items.Take(BooksController.PageSize);

            var pageCount = (int)Math.Ceiling(BooksController._library.Count / (double)BooksController.PageSize);
            var currentPage = page ?? 1;
            var viewModel = new BookListViewModel(items, pageCount, currentPage);

            return this.View(viewModel);
        }

        [RelativeRoute("Detail/{id}")]
        public ActionResult Detail(string id)
        {
            var book = BooksController._library.SingleOrDefault(b => b.Id == Guid.Parse(id));

            this.ConfigureMetadata();
            this.InitializeMetadataDetailsViewBag(book);

            var viewModel = new BookDetailViewModel(book);
            return this.View(viewModel);
        }

        [Route("web-interface/books/points/{page:int:min(1)?}")]
        public JsonResult Points(int? page)
        {
            var points = BooksController._library.Select(book => book.Points);
            if (page.HasValue)
                points = points.Skip((page.Value - 1) * BooksController.PageSize);

            points = points.Take(BooksController.PageSize);
            return this.Json(points, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, Route("web-interface/books/vote/{id}")]
        public JsonResult Vote(string id)
        {
            var book = BooksController._library.SingleOrDefault(b => b.Id == Guid.Parse(id));
            book.Vote();

            return this.Json(book.Points, JsonRequestBehavior.DenyGet);
        }

        private void ConfigureMetadata()
        {
            this.MetadataFields.OpenGraphType = "book";

            // Get the image URL from the property ImageUrl
            this.MetadataFields.OpenGraphImage = "ImageUrl";
        }

        private const int PageSize = 5;

        private static readonly List<Book> _library = new List<Book>(10)
        {
            new Book("00000000-0000-0000-0000-000000000001", "Beatrix Potter", "The Tale Of Peter Rabbit", "A British children's book written and illustrated by Beatrix Potter that follows mischievous and disobedient young Peter Rabbit as he is chased about the garden of Mr. McGregor.", "1.jpg"),
            new Book("00000000-0000-0000-0000-000000000002", "Julia Donaldson", "The Gruffalo", "A children's book by writer and playwright Julia Donaldson, illustrated by Axel Scheffler, that tells the story of a mouse, the protagonist of the book, taking a walk in a European forest.", "2.jpg"),
            new Book("00000000-0000-0000-0000-000000000003","Michael Rosen", "We're Going on a Bear Hunt", "A children's book by Michael Rosen and illustrated by Helen Oxenbury about a family going on a hunt for a bear.", "3.jpg"),
            new Book("00000000-0000-0000-0000-000000000004","Judith Kerr", "The Tiger Who Came to Tea", "A short children's story, written and illustrated by Judith Kerr, concerning a girl called Sophie, her mother, and an anthropomorphise tiger who interrupts their afternoon tea.", "4.jpg"),
            new Book("00000000-0000-0000-0000-000000000005","AA Milne", "Winnie the Pooh", "Winnie-the-Pooh, also called Pooh Bear, is a fictional anthropomorphic teddy bear created by English author A. A. Milne.", "5.jpg"),
            new Book("00000000-0000-0000-0000-000000000006","Enid Blyton", "The Enchanted Wood", "The first magical story in the Faraway Tree series by the world’s best-loved children’s author, Enid Blyton.", "6.jpg"),
            new Book("00000000-0000-0000-0000-000000000007","Jill Murphy", "The Worst Witch", "The Worst Witch is a series of children's books written and illustrated by Jill Murphy. The series are primarily boarding school and fantasy stories, with seven books already published.", "7.jpg"),
            new Book("00000000-0000-0000-0000-000000000008","Roald Dahl", "Charlie and the Chocolate Factory", "A 1964 children's novel by British author Roald Dahl that features the adventures of young Charlie Bucket inside the chocolate factory of eccentric chocolatier Willy Wonka.", "8.jpg"),
            new Book("00000000-0000-0000-0000-000000000009","Jacqueline Wilson", "The Story of Tracy Beaker", "10-years-old Tracy tells of her life in a children's home, and how much she would like a real home and a real family.", "9.jpg"),
            new Book("00000000-0000-0000-0000-000000000010","Michelle Magorian", "Goodnight Mister Tom", "Set during World War II, it features a boy abused at home in London who is evacuated to the country at the outbreak of the war. In the care of Mister Tom, an elderly recluse, he experiences a new life of loving and care.", "10.jpg")
        };
    }
}
