using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Sitefinity.Forms.Model;
using Telerik.Sitefinity.Frontend.ContentBlock.Mvc.Controllers;
using Telerik.Sitefinity.Frontend.Forms.Mvc.Controllers;
using Telerik.Sitefinity.Frontend.Forms.Mvc.Controllers.Base;
using Telerik.Sitefinity.Frontend.Forms.Mvc.Models.Fields;
using Telerik.Sitefinity.Frontend.Forms.Mvc.Models.Fields.BackendConfigurators;
using Telerik.Sitefinity.Model.Localization;
using Telerik.Sitefinity.Modules.Forms;
using Telerik.Sitefinity.Modules.Forms.Web.UI.Fields;
using Telerik.Sitefinity.Mvc.Proxy;
using Telerik.Sitefinity.Pages.Model;
using Telerik.Sitefinity.Security;
using Telerik.Sitefinity.Security.Model;
using Telerik.Sitefinity.Utilities.TypeConverters;
using Telerik.Sitefinity.Web.UI.Fields;

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
            this.Migrate();
        }

        public void Migrate()
        {
            var formsManager = FormsManager.GetManager();
            var formDescriptions = formsManager.GetForms().Where(f => f.Framework != FormFramework.Mvc).ToList();

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
                this.CopyFormCommonData(formDescription, duplicateForm, formDescription.Id, manager);

                // Get permissions from ParentForm, because FormDraft is no ISecuredObject
                duplicateForm.CopySecurityFrom(formDescription as ISecuredObject, null, null);
            }
            else
            {
                this.CopyFormCommonData(thisFormMaster, duplicateForm, formDescription.Id, manager);

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
        public void CopyFormCommonData<TControlA, TControlB>(IFormData<TControlA> formFrom, IFormData<TControlB> formTo, Guid formId, FormsManager manager)
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

            this.CopyControls(formFrom.Controls, formTo.Controls, manager, formId);

            manager.CopyPresentation(formFrom.Presentation, formTo.Presentation);
        }

        public void CopyControls<SrcT, TrgT>(IEnumerable<SrcT> source, IList<TrgT> target, FormsManager manager, Guid formId)
            where SrcT : ControlData
            where TrgT : ControlData
        {
            var traverser = new FormControlTraverser<SrcT, TrgT>(source, target, this.CopyControl<SrcT, TrgT>, manager);
            traverser.CopyControls("Body", "Body");
        }

        private TrgT CopyControl<SrcT, TrgT>(SrcT sourceControl, FormsManager manager)
            where SrcT : ControlData
            where TrgT : ControlData
        {
            TrgT migratedCotnrolData;
            if (!sourceControl.IsLayoutControl)
            {
                var migratedControl = this.ConfigureFormControl(sourceControl, sourceControl.ContainerId, manager);

                // Placeholder is updated later.
                migratedCotnrolData = manager.CreateControl<TrgT>(migratedControl, "Body");
            }
            else
            {
                migratedCotnrolData = manager.CreateControl<TrgT>();
                manager.CopyControl(sourceControl, migratedCotnrolData);
            }

            migratedCotnrolData.Caption = sourceControl.Caption;

            return migratedCotnrolData;
        }

        /// <summary>
        /// Prepares a Form control for display in the backend.
        /// </summary>
        /// <param name="formControl">The form control.</param>
        /// <param name="formId">Id of the form that hosts the field.</param>
        /// <returns>The configured form control.</returns>
        public Control ConfigureFormControl(ControlData formControlData, Guid formId, FormsManager manager)
        {
            Control control = manager.LoadControl(formControlData, null);

            var controlType = control.GetType();

            ElementConfiguration elementConfiguration = null;
            foreach (var pair in this.fieldMap)
            {
                if (pair.Key.IsAssignableFrom(controlType))
                {
                    elementConfiguration = pair.Value;
                    if (pair.Value == null)
                        return null;

                    break;
                }
            }

            if (elementConfiguration == null)
                elementConfiguration = this.fieldMap[typeof(FormTextBox)];


            var mvcProxy = new MvcControllerProxy();
            mvcProxy.ControllerName = elementConfiguration.BackendFieldType.FullName;

            var newController = Activator.CreateInstance(elementConfiguration.BackendFieldType);

            var elementController = newController as IFormElementController<IFormElementModel>;
            var formField = control as IFormFieldControl;

            if (elementConfiguration.ElementConfigurator != null)
            {
                elementConfiguration.ElementConfigurator.FormId = formId;
                elementConfiguration.ElementConfigurator.Configure(control, (Controller)elementController);
            }


            var fieldController = elementController as IFormFieldController<IFormFieldModel>;

            if (fieldController != null && formField != null)
            {
                var fieldControl = formField as FieldControl;
                if (fieldControl != null)
                {
                    fieldController.MetaField = formField.MetaField;
                    fieldController.MetaField.Title = fieldControl.Title;
                    fieldController.Model.CssClass = fieldControl.CssClass;
                    fieldController.Model.ValidatorDefinition = fieldControl.ValidatorDefinition;
                }

                mvcProxy.Settings = new ControllerSettings((Controller)fieldController);
            }
            else if (elementController != null)
            {
                elementController.Model.CssClass = ((WebControl)control).CssClass;

                mvcProxy.Settings = new ControllerSettings((Controller)elementController);
            }

            return (Control)mvcProxy;
        }

        private static readonly Type formFileUploadType = TypeResolutionService.ResolveType("Telerik.Sitefinity.Modules.Forms.Web.UI.Fields.FormFileUpload");
        private readonly Dictionary<Type, ElementConfiguration> fieldMap = new Dictionary<Type, ElementConfiguration>()
            {
                { typeof(FormCheckboxes), new ElementConfiguration(typeof(CheckboxesFieldController), new CheckboxesFieldConfigurator()) },
                { typeof(FormDropDownList), new ElementConfiguration(typeof(DropdownListFieldController), new DropdownFieldConfigurator()) },
                { typeof(FormMultipleChoice), new ElementConfiguration(typeof(MultipleChoiceFieldController), new MultipleChoiceFieldConfigurator()) },
                { typeof(FormParagraphTextBox), new ElementConfiguration(typeof(ParagraphTextFieldController), null) },
                { typeof(FormTextBox), new ElementConfiguration(typeof(TextFieldController), null) },
                { MigrateForms.formFileUploadType, new ElementConfiguration(typeof(FileFieldController), null) },
                { typeof(FormSubmitButton), new ElementConfiguration(typeof(SubmitButtonController), null) },
                { typeof(FormCaptcha),  new ElementConfiguration(typeof(CaptchaController), null) },
                { typeof(FormSectionHeader),  new ElementConfiguration(typeof(SectionHeaderController), new SectionElementConfigurator()) },
                { typeof(FormInstructionalText),  new ElementConfiguration(typeof(ContentBlockController), new ContentBlockConfigurator()) }
            };

        private class FormControlTraverser<SrcT, TrgT>
            where SrcT : ControlData
            where TrgT : ControlData
        {
            public FormControlTraverser(IEnumerable<SrcT> source, IList<TrgT> target, Func<SrcT, FormsManager, TrgT> copyControlDelegate, FormsManager manager)
            {
                this._source = source;
                this._target = target;
                this._copyControlDelegate = copyControlDelegate;
                this._manager = manager;
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
                    var newControl = this._copyControlDelegate(currentSourceControl, this._manager);
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
            private readonly Func<SrcT, FormsManager, TrgT> _copyControlDelegate;
            private readonly FormsManager _manager;
        }
    }
}