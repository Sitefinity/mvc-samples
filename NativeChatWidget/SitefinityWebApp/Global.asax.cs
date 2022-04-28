using System;
using NativeChatWidget.Client;
using NativeChatWidget.Configuration;
using NativeChatWidget.Renderer;
using Progress.Sitefinity.Renderer.Designers;
using Telerik.Microsoft.Practices.Unity;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Configuration;

namespace SitefinityWebApp
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            Bootstrapper.Bootstrapped += Bootstrapper_Bootstrapped;
        }

        void Bootstrapper_Bootstrapped(object sender, EventArgs e)
        {
            ObjectFactory.Container.RegisterType(typeof(IPropertyConfigurator), typeof(ExternalPropertyConfigurator), typeof(ExternalPropertyConfigurator).Name);
            ObjectFactory.Container.RegisterType<INativeChatClient, NativeChatClient>();
            Config.RegisterSection<NativeChatConfig>();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}