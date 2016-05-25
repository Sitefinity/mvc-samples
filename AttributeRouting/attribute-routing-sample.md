#Create the attribute routed Books widget

The following sample demonstrates how to create a simple MVC Books widget while leveraging the [attribute routing](http://blogs.msdn.com/b/webdev/archive/2013/10/17/attribute-routing-in-asp-net-mvc-5.aspx) feature of MVC 5 and the Feather UI framework. The Books widget displays a list of books on the front-end and tracks points for each book that are retrived on the client. The goal of the current tutorial is demonstrate how attribute routing can be used in the context of Sitefinity widgets.

###  Prerequisites
- .NET Framework 4.5 or higher
- Sitefinity 8.1 or later
- [Feather](https://github.com/Sitefinity/feather/wiki/Getting-Started)

### Installing the Books widget
1. Clone the [feather-samples](https://github.com/Sitefinity/feather-samples) repository.
2. Build the **BooksWidget** project. 
3. Reference the **BooksWidget.dll** from your Sitefinity’s web application.

# Creating the Books widget
Project Feather makes it possible to have MVC widgets that are stored in separate assemblies. In the following sample Books widget will be created in a separate assembly. In order to do that you need to perform the following actions:
* Create a new class library named **BooksWidget** using .NET Framework 4.5.
* Run these commands in the Package Manager Console to install the required Nuget packages:

*NOTE*: Make sure that you have set the Feather package source as explained in [Feather prerequisites](https://github.com/Sitefinity/feather/wiki/Getting-Started#prerequisites). Also 
make sure that the BooksWidget project is selected as default project in the package manager console.

```
   Install-Package Telerik.Sitefinity.Core
   Install-Package Telerik.Sitefinity.Mvc
   Install-Package Telerik.Sitefefinity.Feather.Core
```

* Modify the `AssemblyInfo.cs` of the **BooksWidget** by adding the following snippet:
```csharp 
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers.Attributes;
[assembly: ControllerContainer]
```

* Create a **MVC** folder, **MVC/Views** folder, **MVC/Views/Books** folder, **MVC/Controllers** folder, **MVC/Scripts** folder.

## Creating the Model classes
Create a new class named `Book` and place it in the **MVC/Models** folder. It should have Author, Title and Points properties. Also a Vote method to increment the points. The Book class should look like this:
```csharp
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
```

Create a new class named `BooksViewModel` and place it the **MVC/Models** folder. It should have the properties needed for the Index view: Items, PageCount, CurrentPage, NextPageUrl, PreviousPageUrl. It should look like this:
```csharp
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
```

## Creating the Controller
Create a new class named `BooksController` that derives from the` System.Web.Mvc.Controller` class and place it in the **MVC/Controllers** folder.

Add the following fields to the `BooksController` class for test data:
```csharp
     private const int PageSize = 5;

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
```

Now create an Index action with route relative to the current page that accept page number as an argument: 
```csharp
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
```

Create Points and Vote actions with a direct route so they can be accessed with ajax calls:
```csharp
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
```

Last but not least you should add `ControllerToolboxItem `attribute to the `BooksController` class. This way Sitefinity will automatically add the Books widget in the toolbox. Here is a sample usage of the `ControllerToolboxItem `attribute:
```csharp
   [ControllerToolboxItem(Name = "Books", SectionName = "Feather samples", Title = "Books")]
```

## Creating the View
You only need to create an Index view as this is the only view that is used by the `BooksController`. In order to do that you must create a new Razor view named Index and to place it in the **MVC/Views/Books** folder. Here is the implementation of the newly created Index view:
```razor
   @model BooksWidget.Mvc.Models.BooksViewModel
   
   @using Telerik.Sitefinity.Modules.Pages;
   @using Telerik.Sitefinity.Frontend.Mvc.Helpers;
   
   @Html.Script(ScriptRef.JQuery, "top")
   
   <div data-role="books-widget">
       @foreach (var book in Model.Items)
       { 
           <div>
               <p>@book.Author, @book.Title</p>
               <p>Points: <span data-role="points">@book.Points</span> <a href="" data-role="vote-link">[Vote]</a></p>
           </div>
       }
   
       @if (Model.CurrentPage > 1)
       {
           <a href='@Url.Content(Model.PreviousPageUrl)'>[Previous]</a>
       }
   
       @Model.CurrentPage / @Model.PageCount 
   
       @if (Model.CurrentPage < Model.PageCount)
       {
           <a href='@Url.Content(Model.NextPageUrl)'>[Next]</a>
       }
   
       <input data-role="current-page" type="hidden" value="@Model.CurrentPage" />
   </div>

   @Html.Script(Url.WidgetContent("Mvc/Scripts/books-widget.js"), "bottom")
```

*Note*: You can create a Razor view in a class library project by first choosing **HTML Page** from the **Add New Item** dialog, and then renaming the file to an extension of `.cshtml`.

Be sure to mark the view as an Embedded Resource in the file properties.

## Creating the client-side script
Create a JavaScript file in the **MVC/Scripts** folder named **books-widget.js**. The script will be responsible for retrieving and updating the points of the books by calling the JSON actions.
 ```js
    ; (function ($) {
       var initializeBooksWidget = function (element) {
           var widget = $(element);
           var currentPage = widget.find('input[data-role=current-page]').val();
           var pointSpans = widget.find('span[data-role=points]');
   
           $.get(sf_appPath + 'web-interface/books/points/' + currentPage, function (data) {
               for (var i = 0; i < pointSpans.length && i < data.length; i++) {
                   $(pointSpans[i]).html(data[i]);
               }
           });
   
           widget.find('a[data-role=vote-link]').each(function (index, value) {
               var link = $(value);
               var id = index + (currentPage - 1) * 5; // PageSize is a constant 5
               var idx = index;
               link.click(function () {
                   $.post(sf_appPath + 'web-interface/books/vote/' + id, function (data) {
                       $(pointSpans[idx]).html(data);
                   });
   
                   return false;
               });
           });
       };
   
       $(function () {
           $('div[data-role=books-widget]').each(function (index, value) {
               initializeBooksWidget(value);
           });
       });
})(jQuery);
 ```

Be sure to mark the script as an Embedded Resource in the file properties.

Now you can build the project and test the result by dragging the Books widget control from the toolbox of the page.
