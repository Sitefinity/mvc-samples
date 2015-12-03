Create custom designer views for dynamic content widgets
=====================================

All Sitefinity and Feather widgets have designers where you configure widget properties. 
This tutorial demonstrates how to create custom designer views for dynamic content MVC widgets. 
For more information on creating custom designer view for content controls, 
see [Feather: Create custom designer views](http://docs.sitefinity.com/feather-create-custom-designer-views).

Create the template file
------------------------

To create a custom view for your widget designer, you need to create a new template file. 
The file name of this template must have the following naming convention:

`
~/Mvc/Views/<DynamicContentTypeName>/DesignerView.<ViewName>.<extension>
`

As `<DynamicContentTypeName>` you must enter the name of your dynamic content type.

__*NOTE: If you have created a dynamic type with name Countries, the folder name must be named with singular form - Country instead of Countries.*__

`<ViewName>` is the name of your custom designer view. For example, `MyView`

The extension of the template file depends on your view engine – for Razor the valid extension is `.cshtml`, for WebForms the extension is `.aspx`.

The following image illustrates the folder structure of a custom designer view for a dynamic content type with name Country:

![img](http://docs.sitefinity.com/sf-images/default-source/feather/customDynamicDesignerView2.JPG)

To run this sample, get the DesignerView.MyView.cshtml file from this SitefinityWebApp/MVC/Views/Country folder in this repository and place it in the MVC\Views\Country folder of your Sitefinity web application.
