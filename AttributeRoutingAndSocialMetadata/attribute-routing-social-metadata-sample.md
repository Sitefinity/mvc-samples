# Create the attribute routed Books widget

The following sample demonstrates a simple MVC Books widget displaying a list of books and book details on the front-end, and tracks points for each book that are retrieved, on the client.
The goal of this sample is to demonstrate:
1. How [attribute routing](http://blogs.msdn.com/b/webdev/archive/2013/10/17/attribute-routing-in-asp-net-mvc-5.aspx) can be used in the context of Sitefinity widgets.
2. How to leverage the Sitefinity CMS SEO and OpenGraph functionality with Feather widgets for displaying richer content in social media.

###  Prerequisites
- .NET Framework 4.5 or higher
- Sitefinity 10.2 or later
- [Feather](https://github.com/Sitefinity/feather/wiki/Getting-Started)
- [OpenGraph for MVC widgets](https://docs.sitefinity.com/open-graph-settings)
- [SEO for MVC widgets](https://docs.sitefinity.com/seo-automatic-generation-of-metadata-for-mvc-widgets)

### Installing the Books widget
1. Clone the [feather-samples](https://github.com/Sitefinity/feather-samples) repository.
2. Build the **BooksWidget** solution. 
3. Include the BooksWidget sample in your Sitefinity web application project:
   a) Add a reference to the **BooksWidget.dll**
   b) Add the **BooksWidget.dll** in the **bin** folder

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

* The **BooksWidget** assembly should be decorated with a `ControllerContainer` attribute in the `AssemblyInfo.cs` file so that Feather knows that widget controllers are contained in this assembly.

    The end result should look similar to [AssemblyInfo.cs](https://github.com/Sitefinity/feather-samples/tree/master/AttributeRouting/BooksWidget/Properties/AssemblyInfo.cs)

* Create the following folder structure at the root of your project:
```bash
├───Images
├───Mvc
│   ├───Controllers
│   ├───Models
│   ├───Scripts
│   └───Views
│       └───Books
```

## Creating the Model classes
First, we need to define the data model:

* Create a new class named `Book` and place it in the **Mvc/Models** folder.
This class must implement the `IDataItem` interface in order to leverage the Feather widgets social metadata functionality.
Apart form the memebers this interface defines, it should have `Author`, `Title`, `Description`, `ImageUrl` and `Points` properties. Also a `Vote()` method to increment the points.

    The end result should look similar to [Book.cs](https://github.com/Sitefinity/feather-samples/tree/master/AttributeRouting/BooksWidget/Mvc/Models/Book.cs)

The widget will have 2 views - Master and Detail. For this purpose we need to create 2 view model classes:

* Create a new class named `BookListViewModel` and place it the **Mvc/Models** folder.
It should define the properties needed for the Master view (`Index.cshtml`): `Items`, `PageCount`, `CurrentPage`, `NextPageUrl`, `PreviousPageUrl`,
and a few helper methods for constructing the page URLs for the pager and the Detail URLs for each item.

    The end result should look similar to [BookListViewModel.cs](https://github.com/Sitefinity/feather-samples/tree/master/AttributeRouting/BooksWidget/Mvc/Models/BookListViewModel.cs)

* Create a new class named `BookDetailViewModel` and place it the **Mvc/Models** folder.
It should define the properties needed for the Detail view (`Detail.cshtml`): `Id`, `Title`, `Description`, `Author`, `ImageUrl` and `Points`.

    The end result should look similar to [BookDetailViewModel.cs](https://github.com/Sitefinity/feather-samples/tree/master/AttributeRouting/BooksWidget/Mvc/Models/BookDetailViewModel.cs)

## Creating the Controller
Create a new class named `BooksController` and place it in the **Mvc/Controllers** folder.. It should inherit the`ContentBaseController` class located in the `Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers` namespace.
This base class contains all the necessary logic to leverage the Sitefinity CMS SEO and OpenGraph functionality with Feather widgets.
All action methods of this controller will demonstrate how to utilize attribute routing.
It should contain first and foremost the two action methods for displaying the Master and Detail views - `Index()` and `Detail()` respectively.
Both actions should be decorated with the `RelativeRoute` attribute provided by Feather.
The route pattern for the `Index()` action should accept a single integer parameter to denote the requested page number.
The route pattern for the `Detail()` action should also accept a single parameter to denote the requested item's ID.
For simplicity, the class should define a static list of books, i.e. this is going to be our test library, which is going to be used to query data and change it (by voting).
The `Index()` action should just return a new `BookListViewModel` containing the static library items taking into account the requested page.
The `Detail()` action should return a new `BookDetailViewModel` taking into consideration the request book's ID. This is the action which is usually used to specify special page metadata -
SEO and OpenGraph - that enables social networks to display richer content. In order to leverage this functionality a base class method - `InitializeMetadataDetailsViewBag()`,
that accepts an `IDataItem` object (the requested book in this case), the properties from which are used to extract the necessary metadata content.
This widget controller should also define actions for getting the current points for a given book and voting for a current book, both by its ID - `Points()` and `Vote()` respectively.
Both these actions are decorated with the MVC 5 `Route` attribute with a pattern that should accept a single parameter to denote the requested book's ID.
They are going to be called in the widget javascript via AJAX requests.

Last but not least you should decorate the `BooksController` class with a `ControllerToolboxItem `attribute. This way Sitefinity will automatically add the Books widget in the toolbox.
You should specify `Name`, `Title` and `SectionName`.

The end result should look similar to [BooksController.cs](https://github.com/Sitefinity/feather-samples/tree/master/AttributeRouting/BooksWidget/Mvc/Controllers/BooksController.cs)

## Creating the Views
This sample widget has 2 views - Master (`Index.cshtml`) and Detail (`Details.cshtml`). Both views have special `data` attributes that are going to be used in the widget javascript.

* Create a new Razor view named `Index.cshtml` and to place it in the **Mvc/Views/Books** folder.
This Master view is going to display the list of books (their Title, Author and Points)
and will provide the functionality to vote for a particular book, as well as contain a link to the Detail view for each book.

    The end result should look similar to [Index.cshtml](https://github.com/Sitefinity/feather-samples/tree/master/AttributeRouting/BooksWidget/Mvc/Views/Books/Index.cshtml)

* Create a new Razor view named `Detail.cshtml` and to place it in the **Mvc/Views/Books** folder.
This Detail view is going to display details for a book (its Title, Author, Description, Points and the associated image with it).
and will provide voting functionality.

    The end result should look similar to [Detail.cshtml](https://github.com/Sitefinity/feather-samples/tree/master/AttributeRouting/BooksWidget/Mvc/Views/Books/Detail.cshtml)


*Note*: You can create a Razor view in a class library project by first choosing **HTML Page** from the **Add New Item** dialog, and then renaming the file to an extension of `.cshtml`.

**Important:** Be sure to mark the views as an Embedded Resource from the file properties.

## Creating the client-side script
Create a JavaScript file in the **Mvc/Scripts** folder named **books-widget.js**. The script will be responsible for retrieving and updating the points of the books by calling the widget JSON controller actions mentioned earlied using AJAX requests.

The end result should look similar to [books-widget.js](https://github.com/Sitefinity/feather-samples/tree/master/AttributeRouting/BooksWidget/Mvc/Scripts/books-widget.js)

**Important:** Be sure to mark the script as an Embedded Resource from the file properties.

## Adding images for each book
Sample images should be added in order to observe the `og:image` metatag in action when in Detail view of a book.
The images should be marked as an Embedded Resource. You can find how to construct and use the URLs in [Book.cs](https://github.com/Sitefinity/feather-samples/tree/master/AttributeRouting/BooksWidget/Mvc/Models/Book.cs)
and [Detail.cshtml](https://github.com/Sitefinity/feather-samples/tree/master/AttributeRouting/BooksWidget/Mvc/Views/Books/Detail.cshtml) respectively.

Now you can test the result after following steps 2. and 3. given in [Installing the Books widget](#installing-the-books-widget).
Drag the *Books* widget, which should be located in a *Feather samples* section in the control toolbox, on the page when editing it.
