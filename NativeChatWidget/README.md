Custom NativeChat widget
======

The following sample demonstrates how to create a custom MVC widget for displaying a NativeChat chat on your Sitefinity pages. The widget is similar to the one that is built in Sitefinity. You can choose to alter the existing NativeChat widget or build your own from scratch. In both cases this sample can be used as a reference.

# Install the *NativeChat* widget

1. Clone the [feather-samples](https://github.com/Sitefinity/feather-samples) repository.
2. Check if the version of the Sitefinity nugets referenced in it is the same as the version of your project. It they are different make sure to upgrade the nugets in **NativeChatWidget** project to match your version.
3. Build the **NativeChatWidget** project.
4. Reference the **NativeChatWidget.dll** from your Sitefinity’s web application.
5. Create a **Global.asax.cs** file in your Sitefinity web application if you don't have such.
6. Modify its content as shown in the sample **NativeChatWidget/SitefinityWebApp/Global.asax.cs**
7. Build your Sitefinity web application.

The widget will appear in your page toolbox.
