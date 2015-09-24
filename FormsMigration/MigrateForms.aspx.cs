using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Sitefinity;
using Telerik.Sitefinity.Fluent;
using Telerik.Sitefinity.Fluent.Forms;
using Telerik.Sitefinity.Forms.Model;
using Telerik.Sitefinity.Model.Localization;
using Telerik.Sitefinity.Modules;
using Telerik.Sitefinity.Modules.Forms;
using Telerik.Sitefinity.Pages.Model;
using Telerik.Sitefinity.Security;
using Telerik.Sitefinity.Security.Model;

namespace SitefinityWebApp
{
    public partial class MigrateForms : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.MigrateButton.Click += MigrateButton_Click;
        }

        void MigrateButton_Click(object sender, EventArgs e)
        {
            var formDescriptions = this.GetFormDescriptions();
            this.Migrate(formDescriptions);
        }

        public void Migrate(IList<FormDescription> formDescriptions)
        {
            var formsManager = FormsManager.GetManager();

            foreach (var formDescription in formDescriptions)
            {
                var formName = formDescription.Name + "_MVC";

                var duplicateForm = this.Duplicate(formDescription, formName, formsManager);
                duplicateForm.Framework = FormFramework.Mvc;
                duplicateForm.Title = formDescription.Title + "_MVC";
            }

            formsManager.SaveChanges();
        }

        public IList<FormDescription> GetFormDescriptions()
        {
            var formsManager = FormsManager.GetManager();
            var formDescriptions = formsManager.GetForms().Where(f => f.Framework != FormFramework.Mvc).ToList();

            return formDescriptions;
        }

        public virtual FormDescription Duplicate(FormDescription formDescription, string formName, FormsManager manager)
        {
            var duplicateForm = manager.CreateForm(formName);
            var thisFormMaster = manager.Lifecycle.GetMaster(formDescription);

            // Form has been unpublished
            if (thisFormMaster == null)
            {
                this.CopyFormCommonData(formDescription, duplicateForm, manager);

                // Get permissions from ParentForm, because FormDraft is no ISecuredObject
                duplicateForm.CopySecurityFrom(formDescription as ISecuredObject, null, null);
            }
            else
            {
                this.CopyFormCommonData(thisFormMaster, duplicateForm, manager);

                // Get permissions from ParentForm, because FormDraft is no ISecuredObject
                duplicateForm.CopySecurityFrom(thisFormMaster.ParentForm as ISecuredObject, null, null);
            }

            duplicateForm.Controls
                .ToList()
                .ForEach(c => c.Published = false);

            return duplicateForm;
        }

        /// <summary>
        /// Copies the data from one IFormData object to another. Note: this method only copies common draft and non-draft
        /// data.
        /// </summary>
        /// <param name="formFrom">The source object.</param>
        /// <param name="formTo">The target object.</param>
        public void CopyFormCommonData<TControlA, TControlB>(IFormData<TControlA> formFrom, IFormData<TControlB> formTo, FormsManager manager)
            where TControlA : ControlData
            where TControlB : ControlData
        {
            formTo.LastControlId = formFrom.LastControlId;
            formTo.CssClass = formFrom.CssClass;
            formTo.FormLabelPlacement = formFrom.FormLabelPlacement;
            formTo.RedirectPageUrl = formFrom.RedirectPageUrl;
            formTo.SubmitAction = formFrom.SubmitAction;
            formTo.SubmitRestriction = formFrom.SubmitRestriction;

            formTo.SubmitActionAfterUpdate = formFrom.SubmitActionAfterUpdate;
            formTo.RedirectPageUrlAfterUpdate = formFrom.RedirectPageUrlAfterUpdate;

            LocalizationHelper.CopyLstring(formFrom.SuccessMessage, formTo.SuccessMessage, null, null);

            LocalizationHelper.CopyLstring(formFrom.SuccessMessageAfterFormUpdate, formTo.SuccessMessageAfterFormUpdate, null, null);

            this.CopyControls(formFrom.Controls, formTo.Controls);

            manager.CopyPresentation(formFrom.Presentation, formTo.Presentation);
        }

        public void CopyControls<SrcT, TrgT>(IEnumerable<SrcT> source, IList<TrgT> target)
            where SrcT : ControlData
            where TrgT : ControlData
        {
            throw new NotImplementedException();
        }
    }
}