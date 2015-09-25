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
using Telerik.Sitefinity.Frontend.Forms.Mvc.Models.Fields.CheckboxesField;
using Telerik.Sitefinity.Modules.Forms.Web.UI.Fields;
using Telerik.Sitefinity.Web.UI.Fields;

namespace SitefinityWebApp
{
    internal class CheckboxesFieldConfigurator : IElementConfigurator
    {
        /// <inheritDocs/>
        public void Configure(Control webFormsControl, Controller controller)
        {
            var formFieldController = (IFormElementController<IFormElementModel>)controller;
            var checkboxesControl = (FormCheckboxes)webFormsControl;
            var checkboxesFieldModel = (ICheckboxesFieldModel)formFieldController.Model;
            var initialChoices = new List<string>();

            foreach (var choice in checkboxesControl.Choices)
            {
                initialChoices.Add(choice.Value);
            }

            checkboxesFieldModel.SerializedChoices = JsonConvert.SerializeObject(initialChoices);
        }
    }
}
