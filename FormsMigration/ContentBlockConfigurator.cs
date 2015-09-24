using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using Telerik.Sitefinity.Frontend.ContentBlock.Mvc.Controllers;
using Telerik.Sitefinity.Frontend.ContentBlock.Mvc.Models;
using Telerik.Sitefinity.Frontend.Forms.Mvc.Controllers.Base;
using Telerik.Sitefinity.Frontend.Forms.Mvc.Models.Fields;
using Telerik.Sitefinity.Modules.Forms.Web.UI.Fields;

namespace SitefinityWebApp
{
    internal class ContentBlockConfigurator: IElementConfigurator
    {
        /// <inheritDocs/>
        public Guid FormId
        {
            get;
            set;
        }

        /// <inheritDocs/>
        public void Configure(Control webFormsControl, Controller formElementController)
        {
            var contentBlockController = (ContentBlockController)formElementController;
            var instructionalTextControl = (FormInstructionalText)webFormsControl;

            var contentBlockModel = typeof(ContentBlockController).GetProperty("Model", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(contentBlockController, null);
            ((ContentBlockModel)contentBlockModel).Content = instructionalTextControl.Html;
        }
    }
}
