*Add custom styles to Content block widget*
=====================================

The content block widget in Feather uses the Kendo editor
([link](http://demos.telerik.com/kendo-ui/editor/index)). It enables you
to specify which tools are available in the editor toolbar. The
Feather’s Content block leverages this and sets a predefined list of
tools. One of those tools is "formatting". Without specifiyng details, 
the "formatting" tool gives you some default options - "Paragraph", "Quotation" and "Heading" 1,2,3,4,5 and 6.

You can change the Content Block "formatting" tool globally (for the default
Feather Content Block widget) by replacing the default view that Feather
uses. You need to create a folder structure in your Sitefinity web
application on root level with path
**client-components\\fields\\html-field** and inside of it create a file
named ***sf-html-field.sf-cshtml**.* This way you override the default
view for the html field which is used by the Feather Content Block
widget designer. The default file content is
[here](https://github.com/Sitefinity/feather/blob/master/Telerik.Sitefinity.Frontend/client-components/fields/html-field/sf-html-field.sf-cshtml).

In order to change the default "formatting" tool options, we need to specify explicitly the items it has. 
	
	{ 
	name: "formatting", 
	items: [
		{ text: "Paragraph", value: "p" },
		{ text: "Quotation", value: "blockquote" },
		{ text: "Heading 1", value: "h1" },
		{ text: "Heading 2", value: "h2" },
		{ text: "Heading 3", value: "h3" },
		{ text: "Heading 4", value: "h4" },
		{ text: "Heading 5", value: "h5" },
		{ text: "Heading 6", value: "h6" },
		{ text: "Highlight Error", value: ".hlError" },
		{ text: "Highlight OK", value: ".hlOK" },
		{ text: "Inline Code", value: ".inlineCode" }
	]}
	
The above object has 11 items - the first 8 are the default ones. The last 3 ("Highlight Error", "Highlight OK" and "Inline Code")
add a class (respectfully "hlError", "hlOK" and "inlineCode") to the selection.