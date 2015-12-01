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
1.	Create a new class library and name it CustomImageWidget.
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
•	MVC\Views\CustomImageWidget
•	MVC\Controllers
•	MVC\Models
•	MVC\Scripts\CustomImageWidget

# Create the controller

````

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