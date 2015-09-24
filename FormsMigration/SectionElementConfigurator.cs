using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public Guid FormId
        {
            get;
            set;
        }

        /// <inheritDocs/>
        public void Configure(Control webFormsControl, IFormElementController<IFormElementModel> formElementController)
        {
            var sectionControl = (FormSectionHeader)webFormsControl;
            var sectionElementModel = (ISectionHeaderModel)formElementController.Model;
            sectionElementModel.Text = sectionControl.Title;
            sectionElementModel.HeadingType = (HeadingType)Enum.Parse(typeof(HeadingType), sectionControl.WrapperTag.ToString().ToLowerInvariant());
        }
    }
}
