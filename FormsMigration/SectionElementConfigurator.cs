using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using Telerik.Sitefinity.Frontend.Forms.Mvc.Controllers.Base;
using Telerik.Sitefinity.Frontend.Forms.Mvc.Models.Fields;
using Telerik.Sitefinity.Frontend.Forms.Mvc.Models.Fields.SectionHeader;
using Telerik.Sitefinity.Modules.Forms.Web.UI.Fields;

namespace SitefinityWebApp
{
    internal class SectionElementConfigurator: IElementConfigurator
    {
        /// <inheritDocs/>
        public void Configure(Control webFormsControl, Controller controller)
        {
            var formElementController = (IFormElementController<IFormElementModel>)controller;
            var sectionControl = (FormSectionHeader)webFormsControl;
            var sectionElementModel = (ISectionHeaderModel)formElementController.Model;
            sectionElementModel.Text = sectionControl.Title;
            sectionElementModel.HeadingType = (HeadingType)Enum.Parse(typeof(HeadingType), sectionControl.WrapperTag.ToString().ToLowerInvariant());
        }
    }
}
