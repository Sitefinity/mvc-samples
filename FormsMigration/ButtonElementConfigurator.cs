using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using Telerik.Sitefinity.Frontend.Forms.Mvc.Controllers.Base;
using Telerik.Sitefinity.Frontend.Forms.Mvc.Models.Fields;
using Telerik.Sitefinity.Frontend.Forms.Mvc.Models.Fields.SubmitButton;
using Telerik.Sitefinity.Modules.Forms.Web.UI.Fields;

namespace SitefinityWebApp
{
    internal class ButtonElementConfigurator : IElementConfigurator
    {
        /// <inheritDocs/>
        public Guid FormId
        {
            get;
            set;
        }

        /// <inheritDocs/>
        public void Configure(Control webFormsControl, Controller controller)
        {
            var formElementController = (IFormElementController<IFormElementModel>)controller;
            var submitButtonControl = (FormSubmitButton)webFormsControl;
            var submitButtonModel = (ISubmitButtonModel)formElementController.Model;
            submitButtonModel.Label = submitButtonControl.Text;
        }
    }
}
