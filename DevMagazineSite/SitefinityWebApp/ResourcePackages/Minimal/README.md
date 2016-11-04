# Minimal package #

This is a basic package, which contains all default templates of the frontend widgets, such as News, Blogs, and Images, etc. as plain HTML, minimal CSS, and a basic grunt configuration. It can be used as a foundation for custom Feather packages, as well as packages that are based on frontend frameworks of your choice. In order to use it you need to add the package to the `ResourcePackages folder` of your project. If the `ResourcePackages` folder doesn't contain any packages, widget templates will be loaded from Feather or from the MVC folder of SitefinityWebApp (if this folder contains files with names, matching the naming convention). Templates from the source of Feather have lowest loading priority. Templates in the MVC folder of SitefinityWebApp are with higher priority, and templates from a package have highest loading priority.

## Package structure ##

The Minimal package contains minimal front-end assets, widget template, grid widget templates and grunt configuration.  Below are listed some of the folders and files.
 - **assets** - contains front-end files such as CSS, JS, images and fonts
    - **dist** - contains the processed ready-to-use front-end assets
        - **images** - contains compressed images from src folder which are usually used as background images in the css
        - **js** - contains a minified js file which is a concatenation of js files listed in `jsfiles.json`. To use this file add a reference to it in the package Razor layout file `MVC/Views/Layouts/default.cshtml`
        - **css** - contains the processed css files
            - **main.css** - this is output of the processed `main.scss` from `assets/src/project/sass`. This file contains Sitefinity and project css
            - **main.min.css** - this is the same as `main.css` but minified. This is the distributed css file which is linked in the package Razor layout file `MVC/Views/Layouts/default.cshtml`
            - **sitefinity.css** - this is output of the processed sitefinity.scss from assets/src/sitefinity/sass. This files contain css added by Sitefinity
            - **sitefinity.min.css** - minified sitefinity.css
    - **src** - contains the source front-end files which are processed via grunt to dist folder
        - **project** - add your non-sitefinity front-end assets here
            - **images** - add images here. Images added in this folder will be compressed and output to `assets/dist/images`
            - **js** - add js files here and list them in `jsfiles.json`. All js files listed in `jsfiles.json` will be concatenated and uglified to `assets/dist/js/project.min.js`
            - **sass** - create subfolders in this folder and add your scss files here
                - **main.scss** - import all your scss files here. This file will be processed to `assets/dist/css/main.min.css`
        - **sitefinity** - contains css and images for tags and social share widgets
 - **MVC** - contains all widget templates categorized by widget and the Razor layout file
 	- **Layouts/default.cshtml** - Razor layout file
 - **GridSystem** - contains grid widget templates
    - **Templates/container.html** - contains one placeholder
    - **Templates/one-column.html** - contains one column widget - row + column
    - **Templates/two-columns.html** - contains two column widget (columns are display as rows one below the other, it is up to you to style the columns to be displayed one side by side)
 - **csslint.json** - contains csslint options and globals
 - **gruntfile.js** - used to configure and define tasks as well as load grunt plugins
 - **jsfiles.json** - contains a list of js files to be automatically concatenated and minified when grunt is run
 - **package.json** - stores metadata grunt and grunt plugins that the project needs

## Editing and creating a widget
By default all default widget templates are included in Minimal package. Modifying a template is super easy. If you want to modify the Horizontal navigation template, just go to `/ResourcePackages/Minimal/MVC/Views/Navigation`, open the `NavigationView.Horizontal.cshtml` file and make your changes.

Creating a new template is just as easy. Duplicate an existing template, give a name to the new file, keeping in mind the following structure - `NavigationView.XXXXXX.cshtml`. Then the new template will appear in the list of templates for this widget in the widget designer in Page editor.

For widgets that have list and details views, the structure should be `List.XXXXXX.cshtml` or `Detail.XXXXXX.cshtml`, respectively.

To create a new widget template for Dynamic content first create a folder with the name of the dynamic module in singular where you have organized the project widget templates (`/Mvc/Views/` or `/ResourcePackages/Minimal/MVC/Views`). After that create a `*.cshtml` file having in mind the structure described above `List.XXXXXX.cshtml` for list view and `Detail.XXXXXX.cshtml` for details view and write the markup of the template.
When you create a new Dynamic module, list and details widget templates are automatically created for this module in `Design > Widget Templates` in Sitefinity Backend. As a starting point, you can use the template from Sitefinity Backend and make the desired changes on the file system.

## Editing and creating a grid widget
By default we include three grid widgets. We don't include CSS to style the grid widgets. It is up to you to add a grid system and make the columns appear side by side.
Modifying a grid widget template is super easy.  If you want to modify the grid widget with two columns go to `/ResourcePackages/Minimal/GridSystem/Templates`, open `two-columns.html` and make your changes.
You can add an extra CSS class to a column, change the label of the field for adding CSS classes for this column in the grid widget designer in Page editor or change the name of a column's placeholder name in Page editor .

**Example:**
```
<div class="" data-sf-element="Row">
    <div class="sf_colsIn EXTRA_CSS_CLASS" data-sf-element="NEW_COLUMN_1_CSS_CLASS_FIELD_LABEL" data-placeholder-label="NEW_COLUMN_1_PLACEHOLDER_NAME">
    </div>
    <div class="sf_colsIn" data-sf-element="Column 2" data-placeholder-label="Column 2">
    </div>
</div>
```

Don't remove `sf_colsIn`. It is a system css classed which indicates where a placeholder will be created.

Creating a new grid widget is just as easy.
Duplicate an existing grid widget template, give a name to the new file. Then the new grid widget will appear in the list of grid widgets in the Layout tab in Page editor.

**Example:** To create a simple placeholder with &lt;section&gt; tag create `section.html` in `/ResourcePackages/Minimal/GridSystem/Templates/`
```
<section class="section" data-sf-element="Section" data-placeholder-label="Section">
</section>
```

## Widget templates management recommendations

### Modifying widget templates

To make upgrades easier we recommend not to change default widget templates. If you need to make changes to a default template it is better to create a new one by duplicating the one you want to change and make the changes on the new one.

### Where to put project templates

If you want to have the project's widget templates separated from the default widget templates you can place them in the MVC folder in the root `SitefinityWebApp` folder of your project `/Mvc/Views/`. It is easier to navigate and manage less files. The same can be done with grid widget templates. Project specific grid widget templates can be moved to `/GridSystem/Templates` in the root `SitefinityWebApp` folder of the project.

## Grunt
### Install
Prerequisites: If you have not installed grunt yet refer to [Grunt gettings started documentation](http://gruntjs.com/getting-started) for details.
```
> npm install
> grunt
```
`grunt` executes the default grunt tasks and watches for any changes in the files after that.

## Where to put project front-end assets
All project specific front-end assets like scss, images, js, fonts, etc. should be placed in `assets/src/project`. When the default grunt task is run all source files are processed and moved to `assets/dist` from where there are used in the project.

### Scss
Place all your scss files in `assets/src/project/sass`. We recommend to create subfolders to organize the project's files and then import them in `main.scss`
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
// Sitefinity CSS
@import "../../sitefinity/sass/sitefinity";

//Import .scss files here
@import "setting/colors";
@import "setting/typography";
...
@import "base/link";
@import "base/typography";
...
```
When you run grunt all scss files imported in `assets/src/project/sass/main.scss` will be processed and output in `assets/dist/css/main.css`
If you don't want to include Sitefinity CSS remove the import rule `@import "../../sitefinity/sass/sitefinity";` from `assets/src/project/sass/main.scss`.

### Images
Place all images in `assets/src/project/images`. After grunt is run all images from this folder will be compressed and moved to `assets/dist/images`.

### Javascript
Place all your js files in `assets/src/project/js`. It is always best to load one js file to reduce requests to the server and speed up your site. Therefore we provide a mechanism to easily concatenate and uglify all project js files into one file.

In `jsfiles.json` define the order in which the project's js files will be concatenated and uglified. After you run grunt all js files listed in `jsfiles.json` will be processed and output to `assets/dist/js/project.min.js`.

**jsfiles.json**
```
{
	"concatJsFiles": [
        "assets/src/project/js/project-file-1.js",
        "assets/src/project/js/project-file-2.js"
	]
}
```

**Example:** To load `project.min.js` open the project Razor layout file (`MVC/Views/Layouts/default.cshtml`) and add a reference there.
```
	@Html.Script(Url.Content"\~/ResourcePackages/Minimal/assets/dist/js/project.min.js"), "bottom")
```

## Upgrade recommendations
- If you work on a copy of Minimal, e.g. MinimalCustom
    - Upgrade Minimal package
    - Merge changes from Minimal to MinimalCustom manually
- If you work on Minimal package directly
	- Before upgrade make a copy of Minimal package to another location
	- Upgrade Minimal package
	- Merge conflicts
		- If you use a source control, merge the changes using the source control
		- If you don't use a source control, merge the changes from the copy of the Minimal package to the Minimal package manually
