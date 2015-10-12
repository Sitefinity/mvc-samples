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
using Telerik.Sitefinity.Frontend.Forms.Mvc.Models.Fields.ParagraphTextField;
using Telerik.Sitefinity.Modules.Forms.Web.UI.Fields;
using Telerik.Sitefinity.Web.UI.Fields;

namespace SitefinityWebApp
{
    internal class ParagraphFieldConfigurator : IElementConfigurator
    {
        /// <inheritDocs/>
        public void Configure(Control webFormsControl, Controller controller)
        {
            var formFieldController = (IFormFieldController<IFormFieldModel>)controller;
            var paragraphControl = (FormParagraphTextBox)webFormsControl;
            var paragraphFieldModel = (IParagraphTextFieldModel)formFieldController.Model;

            paragraphFieldModel.MetaField.DefaultValue = paragraphControl.DefaultStringValue;
            paragraphFieldModel.MetaField.Description = paragraphControl.Example;
        }
    }
}
