*Customize the Content block toolbar*
=====================================

The content block widget in Feather uses the Kendo editor
([link](http://demos.telerik.com/kendo-ui/editor/index)). It enables you
to specify which tools are available in the editor toolbar. The
Feather’s Content block leverages this and sets a predefined list of
tools. You are able to alter this list of tools in several ways,
depending on the scope of the changes you wish to achieve.

You can change the Content Block toolbar globally (for the default
Feather Content Block widget) by replacing the default view that Feather
uses. You need to create a folder structure in your Sitefinity web
application on root level with path
**client-components\\fields\\html-field** and inside of it create a file
named ***sf-html-field.sf-cshtml**.* This way you override the default
view for the html field which is used by the Feather Content Block
widget designer. The current file content is
[here](https://github.com/Sitefinity/feather/blob/master/Telerik.Sitefinity.Frontend/client-components/fields/html-field/sf-html-field.sf-cshtml).
As you can see all of the available tools are listed in the **k-tools**
attribute value. Altering, removing or adding additional tools there
will reflect in the Feather Content Block widget designer.

You can also decide that you don’t want to alter the default Feather
Content Block widget designer but create your own with different set of
tools in the toolbar. You can achieve that by creating your custom
widget (explained
[here](http://docs.sitefinity.com/feather-create-widgets)) or use our
github sample
([here](https://github.com/Sitefinity/feather-samples/tree/master/SimpleContentBlock))
as a start.
