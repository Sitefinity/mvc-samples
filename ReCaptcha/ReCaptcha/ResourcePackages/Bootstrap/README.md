# Bootstrap package

This package is based on the [Bootstrap 3 framework](http://bootstrapdocs.com/v3.3.6/docs/). To use the package, you simply add it to the `ResourcePackages folder` of your project. In case the `ResourcePackages` folder does not contain any packages, widget templates are loaded from *Embedded resources* or from the **MVC** folder of **SitefinityWebApp** folder only if it contains files with names, matching the naming convention. The priority for loading templates is as follows (in descending order):
1. Templates from the resource package
2. Templates in the **MVC** folder of **SitefinityWebApp**
3. Templates from the *Embedded resources* source

## Grunt
### Install
**PREREQUISITES:** You installed Grunt. For more information, see [Grunt Getting statred guide](https://gruntjs.com/getting-started).
Grunt automates running tasks, whereas Npm is the package manager for JavaScript that enables you to assemble packages of reusable code. For more information, visit the  [npm](https://www.npmjs.com/) website.
To install the *Bootstrap 3* package, simply run:
```
> npm install
> grunt
```
`grunt` executes the default Grunt tasks and subsequently watches for any changes in the files after.

## Package structure

The *Bootstrap 3* package contains frontend assets, widget templates, grid widget templates, and Grunt configuration. Following is a hierarchical list of the major folders and files that come with the *Bootstrap 3* package:
 - **assets** - contains frontend files such as CSS, JS, images, and  fonts
 	- **dist** - contains the processed ready-to-use frontend assets
 		- **css** - contains the processed CSS files
 			- **main.css** - this is output of the processed `main.scss` from `assets/src/project/sass`. This file contains the CSS files for Sitefinity CMS, Bootstrap, and the project itself
 			- **main.min.css** - this is the minified version of the `main.css` file. The 'main.min.css' file is the distributed CSS file that is linked in the `MVC/Views/Layouts/default.cshtml` Razor layout file of the package.
        - **fonts** - contains the Sitefinity CMS and the project icon fonts.
        - **images** - contains compressed images from the project's SRC folder. These images are usually used as background images in the CSS.
        - **js** - contains the minified JavaScript file, which is a concatenation of the JavaScript files, listed in the `jsfiles.json` file. To use this file, add a reference to it in the **MVC/Views/Layouts/default.cshtml** Razor layout file.
 	- **src** - contains the source frontend files that are processed to the **dist** folder
        - **sitefinity** - contains the SCSS files for Sitefinity CMS styling
 		- **project** - add your non-Sitefinity CMS frontend assets in this folder
 			- **fonts** - containts files for the project's icons font
 			- **icons** - add **SVG** files in this folder that need to be added to the icon font once Grunt is run.
 			- **images** - add images in this folder. These images will be compressed and output to the `assets/dist/images` folder.
 			- **js** - add JavaScript files in this folder and list them in the **jsfiles.json** file. All listed JavaScript files are concatenated and uglified to the **assets/dist/js/project.min.js** folder.
 			- **sass** - create subfolders in this folder to add your SCSS files
 				- **main.scss** - import all your SCSS files here. This file will be processed to the **assets/dist/css/main.min.css** folder.
        ```

Following is an example of how to structure files hierarchically:
        
        | ResourcePackages
        |-- Bootstrap
        |---- assests
        |------ dist
        |------ src
        |-------- sitefinity
        |-------- project
        |---------- fonts
        |---------- icons
        |---------- images
        |---------- js
        |---------- sass
        |------------ main.scss
        ```

**IMPORTANT:** We do **not** recommend renaming the subfolders since they are used in `grunt.js` file.

 - **MVC folder** - contains all widget templates (categorized by widget) and the Razor layout file
 	- `Layouts/default.cshtml` - Razor layout file
 - **GridSystem** - contains grid widget templates
 - `csslint.json` - contains CSSlint options and globals
 - `grunt.js` file - contains the configuration and definition of the Grunt tasks, as well as the load Grunt plugins
 - `jsfiles.json` - contains a list of JavaScript files to be automatically concatenated and uglified once Grunt is run
 - `package.json` - stores metadata that the project requires for Grunt and Grunt plugins

## Create and modify widget templates
By default, all widget templates are included in the *Bootstrap 3* package. To modify a widget template, simply open the respective template, say, *Pills* navigation template. To do so, navigate to `/ResourcePackages/Bootstrap/MVC/Views/Navigation`, open the `NavigationView.Pills.cshtml` file, and make your changes.

Creating a new template is just as simple:
1. Duplicate an existing template.
2. Name the new file according to the following convention: `NavigationView.XXXXXX.cshtml`.
**NOTE:** For widgets that have list and details views, the structure should be `List.XXXXXX.cshtml` or `Detail.XXXXXX.cshtml`, respectively.

As a result, the new template is displayed in widget designer for this widget in the list with templates.

To create a new widget template for dynamic content:

1. Create a folder with a name that corresponds to the name of the dynamic content module (in singular).
In the folder, you organize the project widget templates, located in either `/Mvc/Views/` or `/ResourcePackages/Bootstrap/MVC/Views`.
2. Create a `*.cshtml` file, according to the following convention: `List.XXXXXX.cshtml` for list view and `Detail.XXXXXX.cshtml` for details view.
3. Write the markup of the template.

When you create a new Dynamic module, list and details widget templates for this module are automatically created for this module in `Design > Widget Templates`. 

## Create and edit grid widget templates
By default, the most popular column combinations are included in the *Bootstrap 3* package as grid widgets. To modify a grid widget template, for example, a grid widget with two equal columns, simply navigate to `/ResourcePackages/Bootstrap4/GridSystem/Templates`, open `grid-6+6.html` and make your changes.

You can:
- Add an extra CSS class to a column
- Change the label of the field for adding CSS classes for this column in the grid widget designer
- Change the name of a column placeholder name in the Page editor.

**Example:**
```
<div class="row" data-sf-element="Row">
    <div class="sf_colsIn col-md-6 **EXTRA_CSS_CLASS**" data-sf-element="**NEW_COLUMN_1_CSS_CLASS_FIELD_LABEL**" data-placeholder-label="**NEW_COLUMN_1_PLACEHOLDER_NAME**">
    </div>
    <div class="sf_colsIn col-md-6" data-sf-element="Column 2" data-placeholder-label="Column 2">
    </div>
</div>
```
**IMPORTANT:** Do **not** remove `sf_colsIn` since it is a system CSS class that indicates where a placeholder is created.

To create a new grid widget is just as simple:
1. Duplicate an existing grid widget template.
2. Name the new file, so that the new grid widget is displayed in the list of grid widgets in the *Layout* tab in the Page editor.

**Example:** To create a simple placeholder with `<section>` tag, create `section.html` in `/ResourcePackages/Bootstrap/GridSystem/Templates/`
```
	<section class="section" data-sf-element="Section" data-placeholder-label="Section">
	</section>
```

## Recommendations for managing widget templates

### Modify widget templates

To make upgrades easier, we recommend not to change the default widget templates. If you need to make changes to a default template, we recommend that you create a new one by duplicating the existing template you want to modify. You then make the changes on the newly created template.

## Responsive design ##

The responsiveness of widgets, including grid widgets, relies solely on the Bootstrap 3 framework.

The only exception is the Navigation widget. You have the option to transform the navigation into a `<select>`.

To do this, in the Navigation widget template file, for example, `/ResourcePackages/Bootstrap/MVC/Views/Navigation/NavigationView.Horizontal.cshtml`, you need to use the `@Html.Action("GetView", new { viewName = "Dropdown",  model= Model})` helper method.
The helper method renders the `<select>`. It is commented out by default but you can use it in combination with the responsive utility classes of Bootstrap.
To use the helper method, modify its markup in the `/ResourcePackages/Bootstrap/MVC/Views/Navigation/Dropdown.cshtml` file.

## Location of project frontend assets
All project-specific frontend assets like SCSS, images, JavaScript files, fonts, and so on need to be placed in the `assets/src/project` folder. When the Grunt task is run, all source files are processed and moved to the `assets/dist` folder, from which they are used in the project.

### SCSS
Place all your SCSS files in the `assets/src/project/sass` folder. We recommend that you create subfolders to organize the project files and only then import the files in `main.scss`
**Example:**
```
File structure
|- sass
|-- settings
|--- _colors.scss
|--- _typography.scss
...
|-- base
|--- _link.scss
|--- _typography.scss
...

main.scss
//Import Bootstrap from npm
@import "../../../../node_modules/bootstrap-sass/assets/stylesheets/bootstrap.scss";
@import "../../../../node_modules/magnific-popup/src/css/main.scss";

// Sitefinity
@import "../../sitefinity/sass/components/icons/sf-icon-font";
@import "../../sitefinity/sass/widgets/socialShare/sf-sprite";
@import "../../sitefinity.sass/sitefinity.svss";

//Import .scss files here
...
```
When you run Grunt, all SCSS files imported in `assets/src/project/sass/main.scss` are processed and output in `assets/dist/css/main.css`
If you do not want to include Sitefinity CMS or Bootstrap CSS, or you want to use another Bootstrap version, change the import rule `@import "../../sitefinity/sass/sitefinity.scss";` or `@import "../../../../node_modules/bootstrap-sass/assets/stylesheets/bootstrap.scss";` in `assets/src/project/sass/main.scss`.
### Images
Place all images in `assets/src/project/images`. After Grunt is run, all images from this folder will be compressed and moved to `assets/dist/images`.

### JavaScript
Place all your JavaScript files in the `assets/src/project/js` file. We recommend that you load one JavaScript file at a time to reduce server requests and speed up your site.  To do this, you leverage the mechanism for concatenating and uglifying all project JavaScript files into one single file.
In the `jsfiles.json` file, define the order in which the project's JavaScript files are concatenated and uglified. After you run Grunt, all JavaScript files, listed in the `jsfiles.json` file are processed and output to the **assets/dist/js/project.min.js** folder.

`jsfiles.json` file example:
```
{
	"concatJsFiles": [
    	"assets/src/project/js/project-file-1.js",
        "assets/src/project/js/project-file-2.js"
    ]
}
```

**Example**
To load `project.min.js` file, open the project's Razor layout file (`/MVC/Views/Layouts/default.cshtml`) and add a reference in the file:
```
	@Html.Script(Url.Content("~/ResourcePackages/Bootstrap/assets/dist/js/project.min.js"), "bottom", true)
```

### Icons

Place all SVG files that you need to use via an icon font in the `assets/src/project/icons` folder. Once you run Grunt, the icon font is created. If you add new SVG files, you need to run the Grunt task `grunt webfont` manually or rerun the default Grunt task.
For each icon, two CSS are generated. For example, if the name of the SVG file is **logo.svg**, the names of the CSS classes will be as follows:
- `icon-logo` - the icon is displayed before the company name
`<span class="icon-logo">Company name</span>`
- `icon-item-logo` - the icon is displayed after the company name
`<span class="icon-item-logo">Company name</span>`

## Upgrade recommendations
If you work on a copy of Bootstrap, for example, BootstrapCustom:
	- Upgrade the Bootstrap package
	- Merge manually all changes from Bootstrap to BootstrapCustom
If you work directly on the Bootstrap package:
	- Before upgrading, make a copy of the Bootstrap package to a different location
	- Upgrade the Bootstrap package
	- Merge conflicts:
		- If you use source control, merge the changes using source control
		- If you do not use source control, merge manually all changes from the copy of the Bootstrap package to the upgraded Bootstrap package

