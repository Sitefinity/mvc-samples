*Use content selectors outside of widget designer views Edit*
=====================================

You can use the client components that Feather provides in the frontend of your application. This sample shows how to display dynamic content selector on the forntend. 
Full documentation about the tutorial can be found [here](http://docs.sitefinity.com/feather-use-content-selectors-outside-of-widget-designer-views).

To run the sample perform the following: 
 - Add all files inside your SitefinityWebApp and include them in the project. 
 - Edit *Mvc/Scripts/App/app.js* by replacing the name of your dynamic content type in the following line:
 
 ````
 $scope.dynamicType = "Telerik.Sitefinity.DynamicTypes.Model.TestModule.SomeType";
 ````
 - Build your SitefinityWebApp.