Github repo example:
<https://github.com/Sitefinity/feather-packages/tree/nstefano/master>

1.  Introduce \[Feather\] for the project

2.  Do “npm install” at
    \~\\SitefinityWebApp\\ResourcePackages\\Bootstrap

3.  Create *jsfiles.json* in the same directory as *gruntfile.js*. In this
    file will be defined the order for concating and uglifing your
    javascript files. As result there will be one javascript file named
    “*output.min.js*” (*you can rename it in *gruntfile.js* in the
    initialization of “uglify”*) located in
    \~\\ResourcePackages\\Bootstrap\\assets\\dist\\js\\

***jsfiles.json* should looks like this: **

```
{

	"concatJsFiles": [

		"path/to/your/javascript/files/.js",

		" path/to/your/javascript/files/.js"

	]

}
 
```

1.  Create a folder for your custom resources in
    \~\\ResourcePackages\\Bootstrap\\assets\\src

**Custom javaScript**

Add “js” folder in your custom resources folder.

Now to use the custom javascript file located in the “assets\\dist\\js”
simply add this piece of code in the \*.cshtml template, right before
the closing &lt;/body&gt; tag.

```
	@Html.Section("bottom")

	@Html.Script(Url.Content"~/ResourcePackages/Bootstrap/assets/dist/js/output.min.js"),
	"bottom")

	(Name may differ if you change it in gruntfile.js)
```

If you want to use it in all templates based on the “default” one, then
just add the code in *default.cshtml* file.

**Custom stylesheet \[SASS preprocessor\]**

Add “sass” folder in your custom resources folder.

Create a single \*.scss file where all of the SASS files will be
included (*the name for this file it is not important*)

It is **very important** to be only one \*.scss file in the “sass”
folder.

Create as many as you like subfolders with files and then @import those
files in your custom stylesheet.

Add custom styles as following in the \*.cshtml template, right before
the closing &lt;/head&gt; tag

```
	@Html.Section("head")

	@Html.StyleSheet(Url.Content("~/ResourcePackages/Bootstrap/assets/dist/css/oranges.min.css"),
	"head")

	(Name may differ. It will be the same as the one created in your “sass”
	folder)
```

If you want to use it in all templates based on the “default” one, then
just add the code in default.cshtml file.

**Custom layout templates {placeholders}**

To add custom layout templates {placeholders} that can be used in “Page
editor” add a \*.html file in
\~\\ResourcePackages\\Bootstrap\\GridSystem\\Templates\\

e.g. This create a simple placeholder for &lt;section&gt; you can use
this code:

```
	<section class="section" data-sf-element="Section"
	data-placeholder-label="Section"></section>;
```

***Works only on Bootstrap based page templates***

**Custom View template**

Add a custom \*.cshtml file in \~\\SitefinityWebApp\\Mvc\\Views\\ and
follow the conventions as in \~\\ResourcePackages\\Bootstrap\\MVC\\Views
(*The folder containing the templates for widgets is named singular*)

e.g. If you want to make a custom navigation template:

- add a folder named “Navigation” in \~\\SitefinityWebApp\\Mvc\\Views\\

- create file starting with
NavigationView.\[the-name-of-your-template\].cshtml or
SocialShare\[name-for-template\].cshtml

**Custom View template for Dynamic content**

http://docs.sitefinity.com/feather-create-custom-designer-views-for-dynamic-content-widgets

Create a folder named as your dynamic module in singular in
\~\\SitefinityWebApp\\Mvc\\Views\\

Then create a \*.cshtml files as follows:

- for list view
\~/MVC/Views/\[DynamicContentNameSingular\]/List.\[NameForTheTemplate\].cshtml

- for detailed view
\~/MVC/Views/\[DynamicContentNameSingular\]/List.\[NameForTheTemplate\].cshtml

*Hint: You can find current used templates as you navigate to Design
&gt; Widget Templates from Sitefinity Backend.*

**Grunt tasks:**

Currently grunt tasks are executed with the following command:


```
 grunt –-target=single target or targets, separated by comma
 
```

Targets can be one of these there:

-   Sitefinity – builds package resources and doesn’t include Bootstrap
    in the generated files

-   sitefinityBootstrap - builds package resources and includes
    Bootstrap in the generated files

-   userAssets – builds user specific resources, located in the user
    folder under \~\\ResourcePackages\\Bootstrap\\assets\\src. This
    folder should be specified in the gruntfile, assigned to the
    userAssetsFolder variable

Example: 

```
 grunt –-target=sitefinityBootstrap,userAssets
 
```

This will build package resources with Bootstrap, as well as user
resources and will activate Watch task for those files

If target isn’t set, the default one is sitefinityBootstrap
