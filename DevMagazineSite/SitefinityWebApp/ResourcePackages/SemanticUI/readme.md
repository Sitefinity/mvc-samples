# SemanticUI package #

This package is based on the [SemanticUI framework](http://semantic-ui.com/) . In order to use it you need to add the package to the ResourcePackages folder of your project. If the ResourcePackages folder doesn't contain any packages, widget templates will be loaded from Feather or from the MVC folder of SitefinityWebApp (if this folder contains files with names, matching the naming convention). Templates from the source of Feather have lowest loading priority. Templates in the MVC folder of SitefinityWebApp are with higher priority, and templates from a package have highest loading priority.

## Package structure ##

The SemanticUI package contains 3 folders and several files. 
 - assets folder - this contains CSS, JS, images and fonts.
 - MVC folder - contains all widget templates categorized by widget
 - GridSystem - contains all layout/grid templates
 - gruntfile.js and other files in the root - these are files we use in our GruntJS set up.


## Editing and creating a widget
By default we include all widget templates in every package. Modifying a template is super easy. If you want to modify the Pills navigation template, just go to `/ResourcePackages/SemanticUI/MVC/Views/Navigation`, open the `NavigationView.Pills.cshtml` file and make your changes. 

Creating a new template is just as easy. 
Duplicate an existing template, gve a name to the new file, keeping in mind the following structure - NavigationView.XXXXXX.cshtml. Then the new template will appear in the list of templates for this widget.

For widgets that have list and details views, the structure should be List.XXXXXX.cshtml or Detail.XXXXXX.cshtml, respectively.

## Responsive design ##

The responsiveness of our widgets and layout controls rely solely on the front end frameworks we're supporting. Our goal was to integrate the frameworks in  the best possible way so that everyone can take advantage of the responsive features of the frameworks. 

We tried to write as less custom CSS as possible and rely mainly on what the frameworks already provide. This way a person who is familiar with Bootstrap, Foundation and SemanticUI shouldn't learn new classes and conventions.

We've made an exception for the Navigation widget. We give you the possibility to transform the navigation into a `<select>`. 

In the navigation widget templates (e.g. `/SemanticUI/MVC/Views/Navigation/NavigationView.Horizontal.cshtml`) there is a helper method 
`@Html.Action("GetView", new { viewName = "Dropdown",  model= Model})`
which renders the `<select>`. It's commented out by default, but if you want to, you can use it with combination with the responsive utility classes of the framework of your choice.
If you decide to use it, you can modify its markup in the `/SemanticUI/MVC/Views/Navigation/Dropdown.cshtml` file.
