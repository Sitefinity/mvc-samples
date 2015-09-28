Automatic Forms migration to MVC
=======

This repository contains script which will automatically migrate your existing forms to new MVC based forms.

The new forms will be named just like the existing ones and will have "_MVC" suffix at the end of their title.
The newly created MVC forms use new Feather MVC fields in the place of the existing WebForms equivalent, and the settings and configuration that you made over the fields are migrated too.

Furthermore the layouts that you have in your existing forms will be migrated, to their Feather Grid equivalent, so the positions of the widgets will be preserved. Still we don't migrate modifications that you have made over the existing layout widget, so we recommend you check the final look of your form after the migration.
Please have in mind that styles that you use in the form will not be loaded since the new MVC forms are based on the Feather methodology of using [Resource packages](http://docs.sitefinity.com/feather-resource-packages), therefore you may need to style them additionally after the migration.

Form responses and subscriptions are also migrated. The responses are not copied, they are linked to the new form instead. The original form will have no responses after the migration.

###  Prerequisites
- .NET Framework 4.5 or higher
- Sitefinity 8.2 or later
- [Feather](https://github.com/Sitefinity/feather/wiki/Getting-Started)

### Setup and run migration script
1. Clone the [feather-samples](https://github.com/Sitefinity/feather-samples) repository.
2. Add all files from **FormsMigration** folder inside your Sitefinity WebApp project. 
3. Build your Sitefinity’s web application.
4. Navigate your website and log on.
5. Navigate your website on the frontend to http://[*mywebsite.com*]/MigrateForms.aspx, where [*mywebsite.com*] is the name of your domain.
6. Click the button *Duplicate forms to MVC* and wait.

### How do we map existing fields?

*FormCheckboxes*        -> *CheckboxesFieldController* 

*FormDropDownList*      -> *DropdownListFieldController* 

*FormMultipleChoice*    -> *MultipleChoiceFieldController* 

*FormParagraphTextBox*  -> *ParagraphTextFieldController* 

*FormTextBox*           -> *TextFieldController* 

*FormFileUpload*        -> *FileFieldController* 

*FormCaptcha*           -> *CaptchaController* 

*FormSectionHeader*     -> *SectionHeaderController* 

*FormInstructionalText* -> *ContentBlockController* 

*FormSubmitButton*      -> *SubmitButtonController* 

default                 -> *TextFieldController* 

### How do we map existing layouts to grid widgets?

**Layout title**       -> **Grid widget**


*100%*             -> *grid-12* 

*25% + 75%*        -> *grid-3+9* 

*33% + 67%*        -> *grid-4+8* 

*50% + 50%*        -> *grid-6+6* 

*67% + 33%*        -> *grid-8+4* 

*75% + 25%*        -> *grid-9+3* 

*33% + 34% + 33%*  -> *grid-4+4+4* 

*"25% + 50% + 25%* -> *grid-3+6+3* 

*4 x 25%*          -> *grid-3+3+3+3* 

*5 x 20%*          -> *grid-2+3+2+3+2* 

### What will happen to my custom fields?

By default your custom fields are migrated to TextField. In case you want to change this you can create your own custom MVC field and drag it to the forms manually. Another option is to add mapping for your custom fields in the upgrade script. 
For this purpose you can edit MigrateForms.aspx.cs file:

1. Open *MigrateForms.aspx.cs*  and find *fieldMap* collection in it.
2. Add record in the collection like this:
{ typeof(*[MyCustomWebFormsField]*),  new ElementConfiguration(typeof(*[MVCFormsControllerName]*), new *[MyCustomFieldConfigurator]*()) }

- *[MyCustomWebFormsField]* - is the name of the existing custom field that you need to migrate.
- *[MVCFormsControllerName]* - is the name of the controller that you will use to display your custom field in the MVC form. In case you find relevant equivalent between the existing MVC fields you can use it.
- *[MyCustomFieldConfigurator]*  - class that implements *IElementConfigurator*; in the *Configure* method you can manually migrate the settings of the field to the new one. If there are no settings that you need to migrate you can leave this as *null*.
