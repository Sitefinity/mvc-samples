# Bootstrap package

This package is based on the [Bootstrap 4 framework](http://getbootstrap.com/). To use the package, you simply add it to the `ResourcePackages folder` of your project. In case the `ResourcePackages` folder does not contain any packages, widget templates are loaded from *Embedded resources* or from the **MVC** folder of **SitefinityWebApp** folder only if it contains files with names, matching the naming convention. The priority for loading templates is as follows (in descending order):
1. Templates from the resource package
2. Templates in the **MVC** folder of **SitefinityWebApp**
3. Templates from the *Embedded resources* source

## Npm
### Install
Npm is the package manager for JavaScript that enables you to assemble packages of reusable code. For more information, visit [Node.js](https://nodejs.org/) and [npm](https://www.npmjs.com/) websites. Currently *Bootstrap 4* package supports Node.js LTS version 10.14.0.
```
> npm install
> npm start
```
`npm start` executes the default **npm** scritps and subsequently watches for any changes in the files after.

## Package structure

The Bootstrap package contains frontend assets, widget templates, and grid widget templates. Following is a hierarchical list of the major folders and files that come with the *Bootstrap 4* package:
 - **assets** - contains frontend files such as CSS, JS, images, and  SVG sprites
 	- **dist** - contains the processed ready-to-use frontend assets
 		- **css** - contains the processed CSS files
 			- **main.css** - this is output of the processed `main.scss` from `assets/src/project/sass`. This file contains the CSS files for Sitefinity CMS, Bootstrap, and the project itself
 			- **main.min.css** - this is the minified version of the `main.css` file. The 'main.min.css' file is the distributed CSS file that is linked in the `MVC/Views/Layouts/default.cshtml` Razor layout file of the package.
        - **sprites** - contains SVG sprites for the Sitefinity CMS icons. These are [Font Awesome](https://fontawesome.com/) SVG sprites.
        - **images** - contains compressed images from the project's SRC folder. These images are usually used as background images in the CSS.
        - **js** - contains the JavaScript files `bootstrap.min.js` and `poper.min.js`. These files are required for the proper use of the Bootstrap framework. The files are linked in the `MVC/Views/Layouts/default.cshtml` Razor layout file of the package.
 	- **src** - contains the source frontend files that are processed to the **dist** folder
	    - **vendors** - contains imports of third-party vendors like Bootstrap and Magnific popup
        - **sitefinity** - contains the SCSS files for Sitefinity CMS styling
 		- **project** - add your non-Sitefinity CMS frontend assets in this folder
 			- **sprites** - add sprite files in this folder that need to be moved to the `assets/dist/sprites` folder. To do this, you need to extend or replace the file paths listed in `package.json` file in `config.copySprites` section.
 			- **images** - add images in this folder. These images will be compressed and output to the `assets/dist/images` folder.
 			- **js** - add JavaScript files in this folder that need to be moved to the `assets/dist/js` folder. You will need to extend/replace the file paths listed in `package.json` file in `config.copyJs` section.
 			- **sass** - create subfolders in this folder to add your SCSS files
 				- **main.scss** - import all your SCSS files here. This file will be processed to the `assets/dist/css/main.min.css` folder.

Following is an example of how to structure files hierarchically:
```     
| ResourcePackages
|-- Bootstrap
|---- assests
|------ dist
|------ src
|-------- sitefinity
|-------- project
|---------- sprites
|---------- images
|---------- js
|---------- sass
|------------ main.scss
```

**IMPORTANT:** We do **not** recommend renaming the subfolders since they are used in npm scripts

 - **MVC folder** - contains all widget templates (categorized by widget) and the Razor layout file
 	- **Layouts/default.cshtml** - Razor layout file
 - **GridSystem** - contains grid widget templates
 - **package.json** - stores metadata that the project requires and all the **npm** scripts that are used for the build

## Create and modify widget templates
By default, all widget templates are included in the Bootstrap 4 package. To modify a widget template, simply open the respective template, say, *Countries* navigation template. To do so, navigate to `/ResourcePackages/Bootstrap4/MVC/Views/Navigation`, open the `NavigationView.Countries.cshtml` file, and make your changes.

Creating a new template is just as simple:
1. Duplicate an existing template.
2. Name the new file according to the following convention: `NavigationView.XXXXXX.cshtml`.
**NOTE:** For widgets that have list and details views, the structure should be `List.XXXXXX.cshtml` or `Detail.XXXXXX.cshtml`, respectively.

As a result, the new template is displayed in widget designer for this widget in the list with templates.

To create a new widget template for dynamic content:

1. Create a folder with a name that corresponds to the name of the dynamic content module (in singular).
In this folder, you organize the project widget templates, located in either `/Mvc/Views/` or `/ResourcePackages/Bootstrap4/MVC/Views`.
2. Create a `*.cshtml` file, according to the following convention: `List.XXXXXX.cshtml` for list view and `Detail.XXXXXX.cshtml` for details view.
3. Write the markup of the template.

When you create a new Dynamic module, list and details widget templates for this module are automatically created in for this module in `Design > Widget Templates`. 

## Create and edit grid widget templates
By default, the most popular column combinations are included in the Bootstrap 4 package as grid widgets. To modify a grid widget template, for example, a grid widget with two equal columns, simply navigate to `/ResourcePackages/Bootstrap4/GridSystem/Templates`, open `grid-6+6.html` and make your changes.

You can:
- Add an extra CSS class to a column
- Change the label of the field for adding CSS classes for this column in the grid widget designer
- Change the name of a column placeholder name in the Page editor.

**Example:**
```
<div class="row" data-sf-element="Row">
    <div class="sf_colsIn col-lg-6 **EXTRA_CSS_CLASS**" data-sf-element="**NEW_COLUMN_1_CSS_CLASS_FIELD_LABEL**" data-placeholder-label="**NEW_COLUMN_1_PLACEHOLDER_NAME**">
    </div>
    <div class="sf_colsIn col-lg-6" data-sf-element="Column 2" data-placeholder-label="Column 2">
    </div>
</div>
```
**IMPORTANT:** Do **not** remove `sf_colsIn` since it is a system CSS classe that indicates where a placeholder is created.

To create a new grid widget is just as simple:
1. Duplicate an existing grid widget template
2. Name the new file, so that the new grid widget is displayed in the list of grid widgets in the Layout tab in the Page editor.

**Example:** To create a simple placeholder with `<section>` tag, create `section.html` in `/ResourcePackages/Bootstrap4/GridSystem/Templates/`
```
<section class="section" data-sf-element="Section" data-placeholder-label="Section"></section>
```

## Recommendations for managing widget templates

### Modify widget templates

To make upgrades easier, we recommend not to change the default widget templates. If you need to make changes to a default template, we recommend that you create a new one by duplicating the existing template you want to modify. You then make the changes on the newly created template.

### Location of project templates

If you want to have separate locations for the project widget templates and the default widget templates, you can place project templates in the MVC folder in the root `SitefinityWebApp` folder of your project `/Mvc/Views/`. This makes it easier to navigate and manage only the project-specific files you need. 

You can apply the same separation for grid widget templates as well. You can move the project-specific grid widget templates to the `/GridSystem/Templates` folder in the root `SitefinityWebApp` folder of the project.

## Responsive design ##

The responsiveness of widgets, including grid widgets, relies solely on the Bootstrap framework.

The only exception is the Navigation widget. You have the option to transform the navigation into a `<select>`.

To do this, in the Navigation widget template file, for example, `/ResourcePackages/Bootstrap4/MVC/Views/Navigation/NavigationView.Horizontal.cshtml`, you need to use the `@Html.Action("GetView", new { viewName = "Dropdown",  model= Model})` helper method.
The helper method renders the `<select>`. It's commented out by default but you can use it in combination with the responsive utility classes of Bootstrap.
To use the helper method, modify its markup in the `/ResourcePackages/Bootstrap4/MVC/Views/Navigation/Dropdown.cshtml` file.

## Location of project frontend assets
All project-specific frontend assets like SCSS, images, JavaSCript files, sprites, and so on need to be placed in the `assets/src/project` folder. When the `npm start` is run, all source files are processed and moved to the `assets/dist` folder, from which there are used in the project.

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
@import "../../../../node_modules/bootstrap/scss/bootstrap";
@import "../../../../node_modules/magnific-popup/src/css/main";

// Sitefinity
@import "../../sitefinity/sass/sitefinity";

//Import .scss files here
...
```
When you run `npm start`, all SCSS files imported in `assets/src/project/sass/main.scss` are processed and output in `assets/dist/css/main.css`
If you do not want to include Sitefinity CMS or Bootstrap CSS, or you want to use another Bootstrap version, change the import rule `@import "../../sitefinity/sass/sitefinity.scss";` or `@import "../../../../node_modules/bootstrap-sass/assets/stylesheets/bootstrap.scss";` in `assets/src/project/sass/main.scss`.
### Images
Place all images in `assets/src/project/images`. After `npm start` is run, all images from this folder will be compressed and moved to `assets/dist/images`.

### Javascript
In the `packages.json -> config.copyJs` section, list all your JavaScript files (separated by space) that you want to be moved to `assets/dist/js`.

**packages.json**
```
...,
"config": {
    "copyJs": "... assets/src/project/js/project-file-1.js assets/src/project/js/project-file-2.js"
}
```

**Example:** To load `project-file-1.js` open the project Razor layout file (`/ResourcePackages/Bootstrap4/MVC/Views/Layouts/default.cshtml`) and add a reference there.
```
@Html.Script(Url.Content("~/ResourcePackages/Bootstrap4/assets/dist/js/project-file-1.js"), "bottom", true)
```

### SVG sprites
According to the [Bootstrap 4 framework](https://getbootstrap.com/docs/4.1/migration/#components) best practices, Sitefinity CMS uses [Font Awesome](https://fontawesome.com/) SVG sprites for our icons. After `npm start` is run, these sprites are located in the `assets/dist/sprites` folder. If you need to load more sprites, list them (separated by space) in the `package.json -> config.copySprits` section in a manner, similar to the following:

**packages.json**
```
...,
"config": {
    "copySprites": "... assets/src/project/sprites/sprite-1.svg assets/src/project/sprites/sprite-2.svg ",
}
```

**Example:** To use SVG icon in widget template add a reference there like this: (`solid.svg` is the sprite and `#book` is the [icon](https://fontawesome.com/icons/book?style=solid))
```
<svg>
    <use class="sf-icon-right" xlink:href="~/ResourcePackages/Bootstrap4/assets/dist/sprites/solid.svg#book"></use>
</svg>
```