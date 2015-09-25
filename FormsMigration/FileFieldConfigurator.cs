using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using Telerik.Sitefinity.Frontend.Forms.Mvc.Controllers.Base;
using Telerik.Sitefinity.Frontend.Forms.Mvc.Models.Fields;
using Telerik.Sitefinity.Frontend.Forms.Mvc.Models.Fields.BackendConfigurators;
using Telerik.Sitefinity.Frontend.Forms.Mvc.Models.Fields.FileField;
using Telerik.Sitefinity.Modules.Forms.Web.UI.Fields;
using Telerik.Sitefinity.Web.UI.Fields;

namespace SitefinityWebApp
{
    internal class FileFieldConfigurator : IElementConfigurator
    {
        /// <inheritDocs/>
        public void Configure(Control webFormsControl, Controller controller)
        {
            var formFieldController = (IFormElementController<IFormElementModel>)controller;
            var fileFieldModel = (IFileFieldModel)formFieldController.Model;
            fileFieldModel.AllowMultipleFiles = (bool)webFormsControl.GetType().GetProperty("AllowMultipleAttachments").GetValue(webFormsControl, null);
            var allowedTypes = webFormsControl.GetType().GetProperty("AllowedFileTypes").GetValue(webFormsControl, null).ToString();
            fileFieldModel.AllowedFileTypes = (AllowedFileTypes)Enum.Parse(typeof(AllowedFileTypes), allowedTypes);
            fileFieldModel.MaxFileSizeInMb = (int)webFormsControl.GetType().GetProperty("MaxFileSizeInMb").GetValue(webFormsControl, null);
            fileFieldModel.MinFileSizeInMb = (int)webFormsControl.GetType().GetProperty("MinFileSizeInMb").GetValue(webFormsControl, null);
            fileFieldModel.OtherFileTypes = (Array)webFormsControl.GetType().GetProperty("OtherFileTypes").GetValue(webFormsControl, null);
        }
    }
}
