Create custom Image widget
======

The following tutorial demonstrates how to create custom MVC Image widget based on the Feather UI framework. The CustomImage widget displays an image and provide the option to upload the image through designer using build-in [sfImageFiled](http://docs.sitefinity.com/feather-image-field) component.

**PREREQUISITES: You are working with:**

1.  **.NET Framework 4.5 or higher**

2.  **Sitefinity 8.2** and above

# Install the custom Image widget

Installing the widget is an alternative to building it yourself.
Instead of building the widget step by step, you can get it from the repository and use it directly.

1.	Clone the feather-samples repository.
2.	Build the CustomImageWidget project.
3.	Reference the CustomImageWidget.dll from your Sitefinity’s web application.

# Create the Custom Image widget

Feather makes it possible to have MVC widgets that are stored in separate assemblies. The following sample creates the simple Image widget in a separate assembly. In fact you can create Image widget following the common conception of [creating widgets ](http://docs.sitefinity.com/feather-create-widgets) so we describe shortly how to create the custom widget and will go in more details about using client components in widget's custom designer.

Perform the following:

1. Create a new class library and name it CustomImageWidget.

2.	Install Telerik.Sitefinity.Feather.Core.StandAlone NuGet package using the following command:

    ````
    Install-Package Telerik.Sitefinity.Feather.Core.StandAlone
    ````

    NOTE: Make sure that you have set the Feather package source as explained in Feather: [Get started](http://docs.sitefinity.com/feather-get-started)
3.	Modify the AssemblyInfo.cs of the CustomImageWidget by adding the following snippet:

   ````
 using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers.Attributes;
 [assembly: ControllerContainer]
 
    ````
 
4.	Create the following folders:

•	MVC

•	MVC\Views

•	MVC\Views\CustomImage

•	MVC\Controllers

•	MVC\Models

•	MVC\Scripts\CustomImage

# Create the controller

Perform the following:

1. In folder MVC/Controllers, create a new class that derives from the System.Web.Mvc.Controller class and name it CustomImageController.
2. Add the following properties to the CustomImageController class:

````C#
    [ControllerToolboxItem(Name = "CustomImage_MVC", Title = "Custom Image", SectionName = "CustomWidgets", ModuleName = "Libraries")]
    public class CustomImageController : Controller
    {
        /// <summary>
        /// Gets the Image widget model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public virtual CustomImageModel Model
        {
            get
            {
                if (this.model == null)
                    this.model = new CustomImageModel();

                return this.model;
            }
        }

        #region Actions

        /// <summary>
        /// Renders appropriate view.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult" />.
        /// </returns>
        public ActionResult Index()
        {
            var viewModel = this.Model.GetViewModel();
            
            return View("Default", viewModel);
        }

        #endregion

        #region Private fields

        private CustomImageModel model;

        #endregion
    }

````

# Create the view

Create a Default view, used by the CustomImageController. To do this, you must create a new Razor view named Default and to place it in the MVC/Views/CustomImage folder. 
To create the Default view, use the following code:

````
@model CustomImageWidget.Mvc.Models.CustomImageViewModel

<div>
    @if (!string.IsNullOrEmpty(Model.SelectedSizeUrl))
    {
        <img src="@Model.SelectedSizeUrl"  title="@Model.ImageTitle" alt="@Model.ImageAlternativeText"/>
    }
</div>

````

**NOTE:** You can create a Razor view in a class library project by selecting HTML Page from the Add New Item dialog, and then renaming the file extension to .cshtml. In the file properties, set the view as Embedded Resource.

# Create the model and view models

1. In Mvc/Models folder create file named CustomImageModel.cs used to define the model's logic:

    ````C#

    public class CustomImageModel
    {
        /// <summary>
        /// Gets or sets the image identifier.
        /// </summary>
        /// <value>
        /// The image identifier.
        /// </value>
        public Guid ImageId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the image provider.
        /// </summary>
        /// <value>
        /// The name of the image provider.
        /// </value>
        public string ImageProviderName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <returns></returns>
        public CustomImageViewModel GetViewModel()
        {
            var viewModel = new CustomImageViewModel();
            SfImage image;
            if (this.ImageId != Guid.Empty)
            {
                image = this.GetImage();
                if (image != null)
                {
                    viewModel.SelectedSizeUrl = this.GetSelectedSizeUrl(image);
                    viewModel.ImageAlternativeText = image.AlternativeText;
                    viewModel.ImageTitle = image.Title;
                }
            }

            return viewModel;
        }

        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <returns></returns>
        protected virtual SfImage GetImage()
        {
            LibrariesManager librariesManager = LibrariesManager.GetManager(this.ImageProviderName);
            return librariesManager.GetImages().Where(i => i.Id == this.ImageId).Where(PredefinedFilters.PublishedItemsFilter<Telerik.Sitefinity.Libraries.Model.Image>()).FirstOrDefault();
        }

        /// <summary>
        /// Gets the selected size URL.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        protected virtual string GetSelectedSizeUrl(SfImage image)
        {
            if (image.Id == Guid.Empty)
                return string.Empty;

            string imageUrl;

            var urlAsAbsolute = Config.Get<SystemConfig>().SiteUrlSettings.GenerateAbsoluteUrls;
            var originalImageUrl = image.ResolveMediaUrl(urlAsAbsolute);
            imageUrl = originalImageUrl;

            return imageUrl;
        }
    }

    ````

2. Crete view model class named CustomImageViewModel.cs inside Mvc/Models folder and paste the following inside:


    ````C#
    public class CustomImageViewModel
    {
        /// <summary>
        /// Gets or sets the image title.
        /// </summary>
        public string ImageTitle { get; set; }

        /// <summary>
        /// Gets or sets the image alternative text.
        /// </summary>
        public string ImageAlternativeText { get; set; }

        /// <summary>
        /// Gets or sets the selected size image URL.
        /// </summary>
        public string SelectedSizeUrl { get; set; }
    }

    ````

# Create the designer

Now we are going to create designer which uses Feather's [Image Field component](http://docs.sitefinity.com/feather-image-field) for uploading actual image to the widget. 

1. Create DesignerView.Simple.cshtml file inside Mvc/Views/CustomImage and embed it in the project (set it's build action to Embedded resource from file properties). Now add definition of sf-image-field client component in it:

   ````
    @using Telerik.Sitefinity.Mvc;
    @using Telerik.Sitefinity.Frontend.Mvc.Helpers;
    
    <form>
        <div class="form-group">
            <label for="image-field" class="m-top-sm">@Html.Resource("Image")</label>
            <sf-image-field class="sf-Media--info modal-settings modal-settings-space"
                sf-model="properties.ImageId.PropertyValue"
                sf-image="selectedImage"
                sf-provider="properties.ImageProviderName.PropertyValue"
                id="image-field"></sf-image-field>
        </div>
    </form>   
   ````

   Note that sf-model attribute is bound to image id, and if you choose and upload image through the selector and save the designer the *ImageId* property of the model will be automatically persisted in the data base. 

2. [Define required dependencies](http://docs.sitefinity.com/feather-use-components-to-resolve-script-dependencies) for the designer through json file. Create DesignerView.Simple.json file inside Mvc/View/CustomImage and set it to embedded resource. At the following content in it:

   ````
    {
	   "priority": 1,
	   "components": ["sf-image-field"]
    }
   ````

3. Now inject the angular modules required by the Image Field component- create designerview-simple.js file inside the Mvc/Scripts/CustomImage folder, embed it in the project and add the following content in it:

    ````JavaScript
     var designerModule = angular.module('designer');
     angular.module('designer').requires.push('sfFields');
     angular.module('designer').requires.push('sfSelectors');
    ````

More information about widget designer framework that Feather provides could be found [here](http://docs.sitefinity.com/feather-widget-designers-framework).
The last two steps can be skipped if you are using Feather version >= 1.4.410.0, since it resolves json and js dependencies automatically.

You can now build your library, place the produced dll into your
Sitefinity web application bin folder and test the widget and its
designer by placing it on a page or simply build your Sitefinity web application 
if it is already refferencing your library.
The widget will appear in your page toolbox.
