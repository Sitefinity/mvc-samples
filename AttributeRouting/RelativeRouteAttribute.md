## RelativeRoute attribute

With the integration of the ASP.NET MVC 5 you can use [attribute routing](http://blogs.msdn.com/b/webdev/archive/2013/10/17/attribute-routing-in-asp-net-mvc-5.aspx) for defining routes for controller actions.

The **RelativeRouteAttribute** is Feather's take on the attribute routing in the context of Sitefinity widgets.

RelativeRoute has the same constructors and properties as the stock Route attribute. You **do not need to call the [MapMvcAttributeRoutes](https://msdn.microsoft.com/en-us/library/dn314850%28v=vs.118%29.aspx) method**. Feather automatically maps all routes that are defined using attributes for both RelativeRoute and Route attributes.

### How is RelativeRouteAttribute different from RouteAttribute?
*RouteAttribute* describes routes relative to the application path. *RelativeRouteAttribute* describes routes relative to the page node where the widget is placed.

For example, let's have a widget with the following implementation:
```csharp
public class SampleController : Controller
{
    [RelativeRoute("my-sample-path")]
    public ActionResult Action1()
    {
    	return this.Content("This is Action1");
    }
		
    [Route("my-sample-path")]
    public ActionResult Action2()
    {
    	return this.Content("This is Action2");
    }
}
```

If this widget is added to a page with URL _~/my-page_ then you get two routes:
1. 	~/my-page/my-sample-path : Displays the page and renders "This is Action1" where the widget is placed.
2. 	~/my-sample-path : Renders "This is Action2" as plain text.
	
**Note:** If there are any RelativeRoute attributes on your controller then the default [Sitefinity routing for MVC widgets](http://docs.sitefinity.com/for-developers-autorouting-in-pure-and-hybrid-mode) is ignored. The [convention-based routes](https://github.com/Sitefinity/feather/wiki/How-to-implement-a-Master-Detail-content-controller) of Feather are also ignored.