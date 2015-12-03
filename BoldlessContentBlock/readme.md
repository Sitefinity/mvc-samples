*Remove the "Bold" button from Content block*
=====================================

The content block widget in Feather uses the Kendo editor
([link](http://demos.telerik.com/kendo-ui/editor/index)). It enables you
to specify which tools are available in the editor toolbar. The
Feather’s Content block leverages this and sets a predefined list of
tools.

You can change the Content Block toolbar globally (for the default
Feather Content Block widget) by replacing the default view that Feather
uses. You need to create a folder structure in your Sitefinity web
application on root level with path
**client-components\\fields\\html-field** and inside of it create a file
named ***sf-html-field.sf-cshtml**.* This way you override the default
view for the html field which is used by the Feather Content Block
widget designer. The default file content is
[here](https://github.com/Sitefinity/feather/blob/master/Telerik.Sitefinity.Frontend/client-components/fields/html-field/sf-html-field.sf-cshtml).
As you can see all of the available tools are listed in the **k-tools**
attribute value.

In order to remove the Bold button, we only need to delete the "bold" tool. The resulting file is [TODO]() and you should have the same folder structure as in this repository.

To run this sample, get the sf-html-field.sf-cshtml file from this SitefinityWebApp/client-components/fields/html-field folder in this repository and place it in client-components\fields\html-field folder of your Sitefinity web application.
