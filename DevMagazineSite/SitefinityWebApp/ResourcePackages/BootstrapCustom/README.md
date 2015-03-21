Bootstrap package
================

This package is based on the [Bootstrap framework](http://getbootstrap.com/) . In order to use it you need to add the package to the ResourcePackages folder of your project. If the ResourcePackages folder doesn't contain any packages, widget templates will be loaded from Feather or from the MVC folder of SitefinityWebApp (if this folder contains files with names, matching the naming convention). Templates from the source of Feather have lowest loading priority. Templates in the MVC folder of SitefinityWebApp are with higher priority, and templates from a package have highest loading priority.

The MVC folder of the Bootstrap package contains a Views folder. In it are the:

* Layouts folder - containing cshtml files, corresponding to master pages in Sitefinity;
* Grids folder - containing the grid widgets, that are the Bootsrap version of Sitefinity layout widgets;
* All other folders contain designer templates and templates for the front end of the corresponding widget.

To create a custom template for a widget, you need to add to the Views folder of your package a cshtml file with the same name as the name of the Feather template you would like to replace. This will automatically replace the default template and force Feather to use your custom one (this is not valid for grids widgets, because the feature hasn't been implemented yet. However, we're planning to deliver it soon).

Responsive design
-----------------

The Navigation widget is responsive by default. In addition, we can hide the navigation or change it with a `<select>` dropdown with transformation classes. These classes are based on the number of breakpoints that Bootstrap offers by default. They can be set either directly in the .cshtml files or in the options of every dragged navigation widget (`Edit > More options > CSS Classes`).

The CSS classes have the following structure:

`.nav-xs-hidden` - where:

 - `nav` is the **element** that is transformed
 - `xs` is the resolution **breakpoint** for which the transformation will be applied and
 - `hidden` is the **transformation** that will be applied.

#### Elements
All of Bootstraps navigation share the `.nav` class, so you can apply classes such as `.nav-md-dropdown` on every navigation template/variation.

#### Breakpoints
Here are all resolution breakpoints:

 - `xs` - screens that are less than 767px wide
 - `sm` - between 768px and 991px
 - `md` - between 992px and 1199px
 - `lg` - more than 1200px

#### Transformations
 - `hidden` - Hides the navigation.
 - `dropdown` - Hides the navigation and shows a <select> element instead.

Using this convention, you can use classes such as `nav-xs-hidden` and `nav-sm-dropdown` to define what happens with the navigation on different resolutions. In our example, the navi will be hidden on the smallest resolution breakpoint, and it will be replaced with a `<select>` on the next resolution breakpoint.


#### Sitemap
The sitemap has two additional classes you can use. Their names are pretty self-explaining:

 - `.nav-sitemap-BREAKPOINT-one-col` - makes the sitemap display in one column
 - `.nav-sitemap-BREAKPOINT-two-cols` - makes the sitemap display in two columns
