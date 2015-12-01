Create custom image widget
======

The following tutorial demonstrates how to create a simple MVC Image widget based on the Feather UI framework. The CustomImage widget displays an image and  provide the option to upload the image through designer using build-in sfImageFiled.

# Install the Custom Image widget

1.	Clone the feather-samples repository.
2.	Build the CustomImageWidget project.
3.	Reference the CustomImageWidget.dll from your Sitefinity’s web application.

# Create the Custom Image widget

Feather makes it possible to have MVC widgets that are stored in separate assemblies. The following sample creates the simple Image widget in a separate assembly.

Perform the following:

1. Create a new class library and name it CustomImageWidget.

2.	Install Telerik.Sitefinity.Feather.Core.StandAlone NuGet package using the following command:

    Install-Package Telerik.Sitefinity.Feather.Core.StandAlone

    NOTE: Make sure that you have set the Feather package source as explained in Feather: Get started
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
    public class CustomImageController: Controller
    {
        /// <summary>
        /// Gets the Image widget model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public virtual ICustomImageModel Model
        {
            get
            {
                if (this.model == null)
                    this.model = ControllerModelFactory.GetModel<ICustomImageModel>(this.GetType());

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

        private ICustomImageModel model;

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

1. In Mvc/Models folder create file named ICustomImageModel.cs used to define the models's interface:

````C#
    public interface ICustomImageModel
    {
        /// <summary>
        /// Gets or sets the image identifier.
        /// </summary>
        /// <value>
        /// The image identifier.
        /// </value>
        Guid ImageId { get; set; }

        /// <summary>
        /// Gets or sets the name of the image provider.
        /// </summary>
        /// <value>
        /// The name of the image provider.
        /// </value>
        string ImageProviderName { get; set; }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <returns></returns>
        CustomImageViewModel GetViewModel();
    }

````

2. In Mvc/Models folder create file named CustomImageModel.cs used to define the models's logic:

````C#

    public class CustomImageModel : ICustomImageModel
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

3. Crete view model class named CustomImageViewModel.cs inside Mvc/Models folder and paste the following inside:


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

4. Now you need to map the model's class to its interface in Ninject, so it can be resolved correctly. To do this create class named InterfaceMappings.cs on root level of your class library project and paste the following inside:

````C#
public class InterfaceMappings : NinjectModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<ICustomImageModel>().To<CustomImageModel>();
        }
    }

````
