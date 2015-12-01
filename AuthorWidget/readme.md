**Create an Author widget (Feather)**
=====================================

The following tutorial demonstrates how to create a simple MVC Author
widget with Feather. The Author widget displays an Author with his name,
description, links to page in Sitefinity and external page (we will be
using Amazon as example) and a profile image. We will focus on building
a complex designer leveraging the built-in client components of Feather.

**PREREQUISITES: You are working with:**

1.  **.NET Framework 4.5 or higher**

2.  **Sitefinity 8.2**

Install the Author widget
=========================

1.  Clone
    the [feather-samples](https://github.com/Sitefinity/feather-samples) repository

2.  Build the AuthorWidget project

3.  Reference the AuthorWidget.dll from your Sitefinity web application

Create the Author widget
========================

Feather makes it possible to have MVC widgets stored in separate
assemblies. The following tutorial creates the Author widget in a
separate assembly.

Setup your project
------------------

1.  Create a new class library project named AuthorWidget.

2.  In Visual Studio, in the Package Manager Console, make sure
    BooksWidget project is selected as default project. Run the
    following packages to install the required packages:

    a.  Install-Package Telerik.Sitefinity.Core -Version 8.2.5900.0

    b.  Install-Package Telerik.Sitefinity.Content -Version 8.2.5900.0

    c.  Install-Package Telerik.DataAccess.Core -Version 2015.3.926.1

    d.  Install-Package Telerik.Sitefinity.Mvc -Version 1.4.360.0

    e.  Install-Package Telerik.Sitefinity.Feather.Core -Version 1.4.360.0

3.  Add the following references to your project:

    a.  System.Web

    b.  System.Configuration

    c.  System.Runtime.Serialization

4.  Tell Feather to look for widgets in your assembly by adding the
    following attribute to the AssemblyInfo.cs: *\[assembly:
    Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers.Attributes.ControllerContainer\]*

5.  Create the folder structure that you will use. You should have an
    MVC folder on root level of your project. It should contain 4 more
    folders – Controllers, Models, Scripts and Views. In the Models,
    Scripts and Views folder create another folder named Author. The
    final structure should look like this:

    a.  MVC

    b.  MVC\\Controllers

    c.  MVC\\Models

    d.  MVC\\Models\\Author

    e.  MVC\\Scripts

    f.  MVC\\Scripts\\Author

    g.  MVC\\Views

    h.  MVC\\Views\\Author

Create the model classes
------------------------

Now that we prepared our solution let’s start implementing the widget
itself. We follow a convention in which a controller calls a model that
holds the business logic, which creates the view model holding all the
information that a view needs to populate and passes it to the view
itself. It’s all described [here](http://www.asp.net/mvc).

Let’s start by creating the Model and ViewModel classes. We create
AuthorModel.cs and AuthorViewModel.cs files and place them into
MVC\\Models\\Author folder. The model has 7 properties which it uses to
populate the ViewModel’s 6 properties. It also has the GetViewModel
method and two private methods that help extract the image URL and the
Sitefinity page URL for the ViewModel. The code for the model is here,
and for the view model – here.

Create the controller
---------------------

Now we need to create the controller. We create AuthorController.cs and
place it in the MVC\\Controllers folder. It has two properties – the
model which it will use to get a view model and the widget template to
be used. In its only action (Index) it will return it’s view and pass it
the view model created by the model. The code for the controller is
here.

Create the view
---------------

Finally we need the view – this is the markup that in combination with
the view model will produce the visual part of our widget. We create
Author.Default.cshtml file and place it in the MVC\\Views\\Author
folder. The code is here.

_**NOTE that the view must be built as an embedded resource (from the
file properties) as well as every other cshtml, js or json file.**_

_**NOTE: You can create a Razor view in a class library project by
selecting HTML Page from the Add New Item dialog, and then renaming the
file extension to .cshtml. In the file properties, set the view as
Embedded Resource.**_

_**NOTE: The used Html.Raw helper method is introduced in MVC 3 and it
helps to output any HTML without encoding it.**_

At this point we should have our widget. Feather also provides us with
the default designer, enabling us to populate all the model’s and
controller’s properties when we click edit. This, however is not very
user friendly – for example we can’t easily get the Sitefinity page ID
that we want to use or the Image ID for the profile. Therefor we will
create our own custom designer, leveraging Feather’s built-in client
components.

Create the designer
-------------------

Feather designers use [AngularJS](https://angularjs.org/) so creating
your own custom designer means that you have to plugin to the existing
angular app. Don’t worry if you have little or no knowledge in angular –
Feather keeps things simple so you only need to follow the conventions
and you will be fine. We will start with creating the markup for the
Designer. We will use several Feather client components – the html field
(explained here) for the Description property, the image field
(explained [here](http://docs.sitefinity.com/feather-image-field)) for
the Profile picture property, the page selector (explained
[here](http://docs.sitefinity.com/feather-page-selector)) for the
Sitefinity page url property, the expander and the style-dropdown
component, enabling us to leverage from the Feather pre-defined styles
explained
[here](http://docs.sitefinity.com/feather-add-predefined-styles).
Following the Feather convention (explained
[here](http://docs.sitefinity.com/feather-create-custom-designer-views)),
we name our custom designer markup DesignerView.Simple.cshtml and place
it in MVC\\Views\\Author folder. The designer markup is here.

Now we need to add our json file. It is used to specify the priority of
the designer view and its dependencies. Following the
[abovementioned](http://docs.sitefinity.com/feather-create-custom-designer-views)
Feather convention we name our custom designer json file
DesignerView.Simple.json and place it in MVC\\Views\\Author folder. It
includes the priority (1 – meaning that this will be the default
designer view – the highest priority) and the used components -
"sf-html-field", "sf-image-field", "sf-page-selector", "sf-expander" and
"sf-style-dropdown". The json content is here.

Finally, as we mentioned in the beginning we need to plugin to the
existing Angular application. In few words - we need to define our
custom designer angular module and set it as a dependency for the
default “designer” module that all designers in Feather use. You can
learn more about AngularJS modules
[here](https://docs.angularjs.org/guide/module). When we define our
module we also specify the other modules it depends on. Following the
Feather convention again, we create designerview-simple.js file and
place it in MVC\\Scripts\\Author folder. There we define our custom
designer module, describe its dependencies and push it as a dependency
for the “designer” module that Feather has. Also, following the
instructions for using the Feather pre-defined styles (explained
[here](http://docs.sitefinity.com/feather-add-predefined-styles)), we
add a value to the “designer” module holding the predefined values for
css classes for our Default view. The js code is here.

_**NOTE that the designer view, json and javascript files must be built
as an embedded resource (from the file properties).**_

You can now build your library, place the produced dll into your
Sitefinity web application bin folder and test the widget and its
designer by placing it on a page.
