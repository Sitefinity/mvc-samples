using PARAGAssistantWidget.Client;
using PARAGAssistantWidget.Configuration;
using PARAGAssistantWidget.OperationProviders;
using System;
using System.Web.Http;
using Telerik.Microsoft.Practices.Unity;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Web.Services.Contracts.Operations;

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
            Config.RegisterSection<PARAGConfig>();
            ObjectFactory.Container.RegisterType<IPARAGAssistantClient, PARAGAssistantClient>(new ContainerControlledLifetimeManager());
            ObjectFactory.Container.RegisterType(typeof(IOperationProvider), typeof(PARAGOperationProvider), typeof(PARAGOperationProvider).Name);
                    
            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                name: "parag",
                routeTemplate: "parag/{action}",
                defaults: new
                {
                    controller = "AgenticRag",
                });
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