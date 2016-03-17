**Create a custom widget with multiple selectors**
=====================================

The following tutorial demonstrates how to create a simple MVC Author
widget with Feather. The Author widget displays an Author with his name,
description, links to page in Sitefinity and external page (we will be
using Amazon as example) and a profile image. We will focus on building
a complex designer leveraging the built-in client components of Feather.

**PREREQUISITES: You are working with:**

1.  **.NET Framework 4.5 or higher**

2.  **Sitefinity 8.2 and above**

Install the Author widget
=========================

Installing the widget is an alternative to building it yourself. Instead of building the widget step by step, you can get it from the repository and use it directly.

1.  Clone
    the [feather-samples](https://github.com/Sitefinity/feather-samples) repository

2.  Check if version of Feather nugets refernced in **AuthorWidget** project is the same as the version that you have in your project. It they are different make sure to upgrade the **AuthorWidget** project to your version.

3.  Build the AuthorWidget project

4.  Reference the AuthorWidget.dll from your Sitefinity web application

Create the Author widget
========================

Feather makes it possible to have MVC widgets stored in separate
assemblies. The following tutorial creates the Author widget in a
separate assembly.

Setup your project
------------------

1.  Create a new class library project named AuthorWidget.

2.  In Visual Studio, in the Package Manager Console, make sure
    AuthorWidget project is selected as default project. Install Telerik.Sitefinity.Feather.Core.StandAlone NuGet package using the following command:

    ````
    Install-Package Telerik.Sitefinity.Feather.Core.StandAlone
    ````
    
    NOTE: Make sure that you have set the Feather package source as explained in Feather: [Get started](http://docs.sitefinity.com/feather-get-started)

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

Now that we prepared our solution let’s start implementing the widget
itself. We follow a convention in which a controller calls a model that
holds the business logic, which creates the view model holding all the
information that a view needs to populate and passes it to the view
itself. It’s all described in the [ASP.NET MVC website](http://www.asp.net/mvc).

Create the model classes
------------------------

Let’s start by creating the Model and ViewModel classes. We create
AuthorModel.cs and AuthorViewModel.cs files and place them into
MVC\\Models\\Author folder. The model has 7 properties which it uses to
populate the ViewModel’s 6 properties. It also has the GetViewModel
method and two private methods that help extract the image URL and the
Sitefinity page URL for the ViewModel. 

The code for the model is the following:

````C#
using System;
using System.Web;
using Telerik.Sitefinity.Modules.Blogs;
using Telerik.Sitefinity.Modules.GenericContent;
using Telerik.Sitefinity.Modules.Libraries;
using Telerik.Sitefinity.Services;
using Telerik.Sitefinity.Web;

namespace AuthorWidget.MVC.Models.Author
{
    public class AuthorModel
    {
        public Guid PageId { get; set; }

        public string AmazonUrl { get; set; }

        public string ImageProviderName { get; set; }

        public Guid ImageId { get; set; }

        [DynamicLinksContainer]
        public string Description { get; set; }

        public string Name { get; set; }

        public string CssClass { get; set; }

        public AuthorViewModel GetViewModel()
        {
            return new AuthorViewModel()
            {
                PageUrl = this.PageUrl(),
                Description = this.Description,
                ImageUrl = this.GetImageUrl(),
                AmazonUrl = this.AmazonUrl,
                Name = this.Name,
                CssClass = this.CssClass
            };
        }

        private string PageUrl()
        {
            if (this.PageId != Guid.Empty)
	        {
                var siteMap = SitefinitySiteMap.GetCurrentProvider();

                SiteMapNode node;
                var sitefinitySiteMap = siteMap as SiteMapBase;
                if (sitefinitySiteMap != null)
                    node = sitefinitySiteMap.FindSiteMapNodeFromKey(this.PageId.ToString(), false);
                else
                    node = siteMap.FindSiteMapNodeFromKey(this.PageId.ToString());

                if (node != null)
                    return UrlPath.ResolveUrl(node.Url, true);
	        }

            return string.Empty;
        }

        private string GetImageUrl()
        {
            if (this.ImageId != Guid.Empty)
            {
                var image = LibrariesManager.GetManager(this.ImageProviderName).GetImage(this.ImageId);
                if (image != null)
                {
                    return image.Url;
                }
            }

            return string.Empty;
        }
    }
}
````

The code for the view model is the following:

````C#
namespace AuthorWidget.MVC.Models.Author
{
    public class AuthorViewModel
    {
        public string PageUrl { get; set; }

        public string AmazonUrl { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public string CssClass { get; set; }
    }
}

````

Create the controller
---------------------

Now we need to create the controller. We create AuthorController.cs and
place it in the MVC\\Controllers folder. It has two properties – the
model which it will use to get a view model and the widget template to
be used. In its only action (Index) it will return it’s view and pass it
the view model created by the model. 

The code for the controller is the following:

````C#
using System;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using AuthorWidget.MVC.Models.Author;
using Telerik.Sitefinity.Mvc;

namespace AuthorWidget.MVC.Controllers
{
    [ControllerToolboxItem(Name = "Author", Title = "Author", SectionName = "Custom")]
    public class AuthorController : Controller
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public AuthorModel Model
        {
            get
            {
                if (this.model == null)
                    this.model = new AuthorModel();

                return this.model;
            }
        }

        public string Template
        {
            get { return this.template; }
            set { this.template = value; }
        }

        public ActionResult Index()
        {
            return this.View("Author." + this.Template, this.Model.GetViewModel());
        }

        private AuthorModel model;
        private string template = "Default";
    }
}

````

Create the view
---------------

Finally we need the view – this is the markup that in combination with
the view model will produce the visual part of our widget. We create
Author.Default.cshtml file and place it in the MVC\\Views\\Author
folder. 

The code for the view is the following:

````C#
@model AuthorWidget.MVC.Models.Author.AuthorViewModel

<div class="@Model.CssClass">
    <img src="@Model.ImageUrl" alt="@Model.Name" width="100" height="100"/>
    <h3>@Model.Name</h3>
    <a href="@Model.PageUrl">View Page</a>
    <br />
    <a href="@Model.AmazonUrl">Visit Amazon</a>
    <p>@Model.Description</p>
</div>
````

_**NOTE that when we create widgets outside of the SitefinityWebApp the view must be built as an embedded resource (from the
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
Designer. We will use several Feather client components - the image field
(detailed information [here](http://docs.sitefinity.com/feather-image-field)) for
the Profile picture property, the page selector (detailed information
[here](http://docs.sitefinity.com/feather-page-selector)) for the
Sitefinity page url property, the expander (detailed information [here](https://github.com/Sitefinity/feather-docs/wiki/Expander-directive)) and the style-dropdown
component, enabling us to leverage from the Feather pre-defined styles
(detailed information
[here](http://docs.sitefinity.com/feather-add-predefined-styles)).
Following the Feather convention (detailed information [here](http://docs.sitefinity.com/feather-priorities-for-resolving-views)),
we name our custom designer markup DesignerView.Simple.cshtml and place
it in MVC\\Views\\Author folder. 

The designer markup is the following:

````C#
@using Telerik.Sitefinity.Frontend.Mvc.Helpers;

<div class="form-group">
    <label for="nameInput">Name</label>
    <div class="row">
        <div class="col-xs-11">
            <input class="form-control" id="nameInput" type="text" ng-model="properties.Name.PropertyValue" />
        </div>
    </div>

    <label for="descriptionInput">Description</label>
    <div class="row">
        <div class="col-xs-11">
            <input class="form-control" id="descriptionInput" type="text" ng-model="properties.Description.PropertyValue" />
        </div>
    </div>
</div>

<expander expander-title="Links">
    <label class="full-width">Page url</label>
    <sf-list-selector
        sf-page-selector
        sf-sortable="true"
        sf-selected-item-id="properties.PageId.PropertyValue">
    </sf-list-selector>
    <br />
    <div class="form-group">
        <label for="amazonLink">Amazon url</label>
        <div class="row">
            <div class="col-xs-11">
                <input class="form-control" id="amazonLink" type="text" ng-model="properties.AmazonUrl.PropertyValue" />
            </div>
        </div>
    </div>
</expander>

<expander expander-title="Image">
    <sf-image-field
        sf-model="properties.ImageId.PropertyValue" 
        sf-provider="properties.ImageProviderName.PropertyValue"
        class="sf-Media--info modal-settings">
    </sf-image-field>
</expander>

<expander expander-title="MoreOptions">
    <style-dropdown selected-class="properties.CssClass.PropertyValue" view-name="properties.Template.PropertyValue"></style-dropdown>

    <div class="form-group">
        <label for="templateName">Template</label>
        <div class="row">
            <div class="col-xs-6">
                <select id="templateName" ng-model="properties.Template.PropertyValue" class="form-control">
                    @foreach (var viewName in Html.GetViewNames("Author", @"Author\.(?<viewName>[\w\s]*)$"))
                    {
                        <option value="@viewName">@viewName</option>
                    }
                </select>
            </div>
        </div>
    </div>

    <div class="form-group">
        <label for="imageCssClass">CssClasses</label>
        <input type="text" id="imageCssClass" ng-model="properties.CssClass.PropertyValue" class="form-control" />
    </div>
</expander>
````

Now we need to add our json file. It is used to specify the priority of
the designer view and its dependencies. Following the Feather convention we name our custom designer json file
DesignerView.Simple.json and place it in MVC\\Views\\Author folder. It
includes the priority (1 – meaning that this will be the default
designer view – the highest priority) and the used components - "sf-image-field", "sf-page-selector", "sf-expander" and
"sf-style-dropdown". 
This step can be skipped if you are using Feather version >= 1.4.410.0, since it resolves json dependencies automatically.

The json content is the following:

````JSON
{
	"priority": 1,
	"components": ["sf-image-field", "sf-page-selector", "sf-expander", "sf-style-dropdown"]
}
````

Finally, as we mentioned in the beginning we need to plugin to the
existing Angular application. In few words - we need to define our
custom designer angular module and set it as a dependency for the
default “designer” module that all designers in Feather use. When we define our
module we also specify the other modules it depends on. Following the
Feather convention again, we create designerview-simple.js file and
place it in MVC\\Scripts\\Author folder. There we define our custom
designer module, describe its dependencies and push it as a dependency
for the “designer” module that Feather has. Also, following the
instructions for using the Feather pre-defined styles (explained
[here](http://docs.sitefinity.com/feather-add-predefined-styles)), we
add a value to the “designer” module holding the predefined values for
css classes for our Default view. 

The js code is the following:

````JS
(function ($) {
    var simpleViewModule = angular.module('simpleViewModule', ['expander', 'designer', 'sfFields', 'sfSelectors']);
    angular.module('designer').requires.push('simpleViewModule');
    angular.module('designer').value('cssClasses', {
        'Default': [
            { 'value': 'blue', 'title': 'Blue box Default' },
            { 'value': 'red', 'title': 'Red box Default' }
        ],
        'djvadjva': [
            { 'value': 'blue', 'title': 'Blue box djvadjva' },
            { 'value': 'red', 'title': 'Red box djvadjva' }
        ]
    });
})(jQuery);
````

_**NOTE that when we create widgets outside of the SitefinityWebApp the designer view, json and javascript files must be built
as an embedded resource (from the file properties).**_

You can now build your library, place the produced dll into your
Sitefinity web application bin folder and test the widget and its
designer by placing it on a page or simply build your Sitefinity web application 
if it is already refferencing your library.
The widget will appear in your page toolbox.
