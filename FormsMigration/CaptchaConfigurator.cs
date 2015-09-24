using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using Telerik.Sitefinity.Frontend.Forms.Mvc.Controllers.Base;
using Telerik.Sitefinity.Frontend.Forms.Mvc.Models.Fields;
using Telerik.Sitefinity.Frontend.Forms.Mvc.Models.Fields.Captcha;
using Telerik.Sitefinity.Modules.Forms.Web.UI.Fields;

namespace SitefinityWebApp
{
    internal class CaptchaConfigurator: IElementConfigurator
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
            var formCaptcha = (FormCaptcha)webFormsControl;
            var captchaModel = (ICaptchaModel)formElementController.Model;
            captchaModel.DisplayOnlyForUnauthenticatedUsers = formCaptcha.DisplayOnlyForUnauthenticatedUsers;
        }
    }
}
