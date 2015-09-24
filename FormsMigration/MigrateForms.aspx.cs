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
            var formDescriptions = formsManager.GetForms().Where(f => f.Framework != FormFramework.Mvc && f.Title == "TestMe").ToList();

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

            LocalizationHelper.CopyLstring(formFrom.SuccessMessage, formTo.SuccessMessage);

            LocalizationHelper.CopyLstring(formFrom.SuccessMessageAfterFormUpdate, formTo.SuccessMessageAfterFormUpdate);

            this.CopyControls(formFrom.Controls, formTo.Controls);

            manager.CopyPresentation(formFrom.Presentation, formTo.Presentation);
        }

        public void CopyControls<SrcT, TrgT>(IEnumerable<SrcT> source, IList<TrgT> target)
            where SrcT : ControlData
            where TrgT : ControlData
        {
            var traverser = new FormControlTraverser<SrcT, TrgT>(source, target, this.CopyControl<SrcT, TrgT>);
            traverser.CopyControls("Body", "Body");
        }

        private TrgT CopyControl<SrcT, TrgT>(SrcT currentSourceControl)
            where SrcT : ControlData
            where TrgT : ControlData
        {
            var manager = FormsManager.GetManager();
            var newControl = manager.CreateControl<TrgT>();
            manager.CopyControl(currentSourceControl, newControl);

            return newControl;
        }

        private class FormControlTraverser<SrcT, TrgT>
            where SrcT : ControlData
            where TrgT : ControlData
        {
            public FormControlTraverser(IEnumerable<SrcT> source, IList<TrgT> target, Func<SrcT, TrgT> copyControlDelegate)
            {
                this._source = source;
                this._target = target;
                this._copyControlDelegate = copyControlDelegate;
            }

            public void CopyControls(string sourcePlaceholder, string targetPlaceholder)
            {
                var controlsToCopy = this._source.Where(c => c.PlaceHolder == sourcePlaceholder).ToDictionary(c => c.SiblingId);

                var currentSourceSiblingId = Guid.Empty;
                var currentTargetSiblingId = Guid.Empty;
                while (controlsToCopy.Count > 0)
                {
                    if (!controlsToCopy.ContainsKey(currentSourceSiblingId))
                        break;

                    var currentSourceControl = controlsToCopy[currentSourceSiblingId];
                    var newControl = this._copyControlDelegate(currentSourceControl);
                    newControl.PlaceHolder = targetPlaceholder;
                    newControl.SiblingId = currentTargetSiblingId;

                    this._target.Add(newControl);
                    controlsToCopy.Remove(currentSourceSiblingId);

                    currentSourceSiblingId = currentSourceControl.Id;
                    currentTargetSiblingId = newControl.Id;

                    if (currentSourceControl.PlaceHolders != null && newControl.PlaceHolders != null)
                    {
                        for (var i = 0; i < currentSourceControl.PlaceHolders.Length; i++)
                        {
                            this.CopyControls(currentSourceControl.PlaceHolders[i], newControl.PlaceHolders[Math.Min(i, newControl.PlaceHolders.Length - 1)]);
                        }
                    }
                }
            }

            private readonly IEnumerable<SrcT> _source;
            private readonly IList<TrgT> _target;
            private readonly Func<SrcT, TrgT> _copyControlDelegate;
        }
    }
}