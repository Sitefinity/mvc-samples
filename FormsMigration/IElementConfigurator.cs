using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Telerik.Sitefinity.Frontend.Forms.Mvc.Controllers.Base;
using Telerik.Sitefinity.Frontend.Forms.Mvc.Models.Fields;

namespace SitefinityWebApp
{
    public interface IElementConfigurator
    {
        /// <summary>
        /// Gets or sets the form identifier.
        /// </summary>
        /// <value>
        /// The form identifier.
        /// </value>
        Guid FormId { get; set; }

        /// <summary>
        /// Configures the specified webforms control.
        /// </summary>
        /// <param name="webFormsControl">The webforms control.</param>
        /// <param name="formElementController">The form element controller.</param>
        void Configure(Control webFormsControl, IFormElementController<IFormElementModel> formElementController);
    }
}