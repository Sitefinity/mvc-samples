Custom widget for displaying a single dynamic content type
======

The following tutorial demonstrates how to create custom MVC widget for displaying single dynamic content item based on the Feather UI framework. The *Single Dynamic Content* widget provide the option to change the dynamic content type and the dynamic content item displayed in it in the designer through build-in [sfDynamicItemsSelector](http://docs.sitefinity.com/feather-dynamic-items-selector) client component.

# Install the *Single Dynamic Content* widget

1.	Clone the feather-samples repository.
2.	Open the SingleDynamicContent project in Visual Studio.
3.      Check if version of Feather nugets referenced in SingleDynamicContent project is the same as the version that you have in your project. It they are different make sure to upgrade the SingleDynamicContent project to your version.
4.      Build SingleDynamicContent project.
5.	Reference the SingleDynamicContent.dll from your Sitefinity’s web application.
6.      Build your Sitefinity web application.

# Create the *Single Dynamic Content* widget

Feather makes it possible to have MVC widgets that are stored in separate assemblies. The following sample creates the *Single Dynamic Content* widget in a separate assembly. In fact you can create *Single Dynamic Content* widget following the common conception of [creating widgets ](http://docs.sitefinity.com/feather-create-widgets) so we describe shortly how to create the custom widget and will go in more details about using client components in widget's custom designer.

Perform the following:

1. Create a new class library and name it SingleDynamicContent.

2.	Install Telerik.Sitefinity.Feather.Core.StandAlone NuGet package using the following command:

    ````

    Install-Package Telerik.Sitefinity.Feather.Core.StandAlone
    ````

    NOTE: Make sure that you have set the Feather package source as explained in Feather: [Get started](http://docs.sitefinity.com/feather-get-started)
3.	Modify the AssemblyInfo.cs of the SingleDynamicContent by adding the following snippet:

   ````
 using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers.Attributes;
 [assembly: ControllerContainer]
 
    ````
 
4.	Create the following folders:

•	MVC

•	MVC\Views

•	MVC\Views\SingleDynamicContent

•	MVC\Controllers

•	MVC\Models

•	MVC\Scripts\SingleDynamicContent

Now when the project structure is ready we can continue with the implementation of the widget itself.

# Create the controller

Perform the following:

1. In folder MVC/Controllers, create a new class that derives from the System.Web.Mvc.Controller class and name it SingleDynamicContentController.
2. Add the following content to the SingleDynamicContentController class:

````C#

[ControllerToolboxItem(Name = "SingleDynamicContent", Title = "Single Dynamic Content", SectionName = "MvcWidgets")]
    public class SingleDynamicContentController : Controller
    {
        #region Properties

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public SingleDynamicContentModel Model
        {
            get
            {
                if (this.model == null)
                    this.model = new SingleDynamicContentModel(); ;

                return this.model;
            }
        }

        public string ItemType
        {
            get
            {
                return this.itemType;
            }

            set
            {
                this.itemType = value;
            }
        }   

        #endregion

        #region Actions

        /// <summary>
        /// This is the default Action.
        /// </summary>
        public ActionResult Index()
        {
            try
            {
                this.Model.Populate(this.ItemType);

                return View("Default", this.Model);
            }
            catch (ArgumentException e)
            {
                return Content("Type {0} doesn't exists!".Arrange(this.ItemType));
            }
        }

        #endregion

        #region Private fields and constants

        private string itemType = "Telerik.Sitefinity.DynamicTypes.Model.Athletes.Athlete";
        private SingleDynamicContentModel model;

        #endregion
    }

````


NOTE: As you can see the Controller exposes public property for ItemType. By editing this property value you can change the type of dynamic content that will be displayed inside the widget. Current sample sets for default itemType **Telerik.Sitefinity.DynamicTypes.Model.Athletes.Athlete** still we are going to expose option in the widget designer to change this type during the setup of the widget. This way the backend user will be able to edit the ItemType by opening the widget for edit and changing the value directly in the designer.

# Create the view

Create a Default view, used by the SingleDynamicContentController. To do this, you must create a new Razor view named Default and to place it in the MVC/Views/SingleDynamicContent folder. 
To create the Default view, use the following code:

````
@model SingleDynamicContent.Mvc.Models.SingleDynamicContentModel

<h3>
    Output some data from the model here:
	
    @if(Model.Item != null){
        @Html.Raw(Model.Item.Id);
    }
    else{
	@Html.Raw("No items found for the selected type!");
    }
</h3>

````

You can modify this view to output the values of all fields for the specified dynamic content type.
**NOTE:** You can create a Razor view in a class library project by selecting HTML Page from the Add New Item dialog, and then renaming the file extension to .cshtml. In the file properties, set the view as Embedded Resource.

# Create the model

In Mvc/Models folder create file named SingleDynamicContentModel.cs used to define the model's interface:

````C#
    public class SingleDynamicContentModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Reference object.
        /// </summary>
        public DynamicContent Item
        {
            get
            {
                return this.item;
            }
            set
            {
                this.item = value;
            }
        }

        /// <summary>
        /// Gets or sets the Reference object.
        /// </summary>
        public string SelectedItemId
        {
            get
            {
                return this.selectedItemId;
            }
            set
            {
                this.selectedItemId = value;
            }
        }

        #endregion

        #region Public Methods

        public void Populate(string itemType)
        {
            var dynamicModuleManager = DynamicModuleManager.GetManager();
            Type referenceType = TypeResolutionService.ResolveType(itemType);

            var query = dynamicModuleManager.GetDataItems(referenceType).Where(s => s.Status == Telerik.Sitefinity.GenericContent.Model.ContentLifecycleStatus.Live);

            Guid itemId;
            if (!string.IsNullOrWhiteSpace(this.SelectedItemId) && Guid.TryParse(this.SelectedItemId, out itemId))
                this.Item = query.SingleOrDefault(n => n.Id == itemId);
            else
                this.Item = query.FirstOrDefault();
        }

        #endregion

        #region Private fields

        private DynamicContent item;
        private string selectedItemId;

        #endregion
    }

````

# Create the designer

Now we are going to create designer which uses Feather's [sfDynamicItemsSelector component](http://docs.sitefinity.com/feather-dynamic-items-selector) for selecting the dynamic content item that will be displayed in the widget. 

1. Create DesignerView.Simple.cshtml file inside Mvc/Views/SingleDynamicContent and embed it in the project. Now add definition of sf-dynamic-items-selector client component in it:

   ````

<div class="form-group">
    <label>Choose dynamic content type:</label>
    <input type="text" ng-model="properties.ItemType.PropertyValue" class="form-control" />
</div>

<div class="form-group">
    <label>Choose dynamic content item from the specified type:</label>
    <div class="row">
        <div class="col-xs-6">
            <sf-list-selector sf-dynamic-items-selector
                sf-item-type="properties.ItemType.PropertyValue"
                sf-selected-item-id="properties.SelectedItemId.PropertyValue"
                sf-provider="properties.ProviderName.PropertyValue" />
        </div>
    </div>
</div>
   
   ````

   Note that we bound ng-model of input and the sf-item-type of the dynamic item selector to the same property and by doing this the content of the selector will change it you type different dynamic content type. Also sf-selected-item-id attribute is bound to dynamic content item id, and when you choose dynamic item through the selector and save the designer the *SelectedItemId* property of the model will be automatically persisted in the data base with the new value. 

2. [Define required dependencies](http://docs.sitefinity.com/feather-use-components-to-resolve-script-dependencies) for the designer through json file. Create DesignerView.Simple.json file inside Mvc/View/SingleDynamicContent and set it to embedded resource. At the following content in it:

   ````Json
{
    "priority": 1,
    "components" : ["sf-dynamic-items-selector"]
}

   ````

3. Now inject the angular modules required by the sfDynamicItemsSelector component- create designerview-simple.js file inside the Mvc/Scripts/SingleDynamicContent folder, embed it in the project and add the following content in it:

    ````JavaScript
	
     angular.module('designer').requires.push('sfSelectors');
	 
    ````

More information about widget designer framework that Feather provides could be found [here](http://docs.sitefinity.com/feather-widget-designers-framework).


Now the single dynamic content widget is ready so you can build its project and add reference to the SingleDynamicContent.dll from your Sitefinity’s web application. 

The widget will appear in your page toolbox.
