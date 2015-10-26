using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace SitefinityWebApp
{
    public interface IElementConfigurator
    {
        /// <summary>
        /// Configures the specified webforms control.
        /// </summary>
        /// <param name="webFormsControl">The webforms control.</param>
        /// <param name="controller">The form element controller.</param>
        void Configure(Control webFormsControl, Controller controller);
    }
}