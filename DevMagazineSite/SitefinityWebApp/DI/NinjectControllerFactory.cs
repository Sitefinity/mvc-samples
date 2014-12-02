using System;
using System.Linq;
using System.Web.Mvc;
using Ninject;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers;
using DevMagazine.Issues.Mvc.Models;
using DevMagazine.Search.Mvc.Models;
using DevMagazine.Issues.Mvc.Models.Impl;
using DevMagazine.Search.Mvc.Models.Impl;

namespace SitefinityWebApp.DI
{
    /// <summary>
    /// This class extends the <see cref="FrontendControllerFactory"/> by adding dependency injection for controllers. 
    /// </summary>
    public class NinjectControllerFactory : FrontendControllerFactory
    {
        private IKernel ninjectKernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectControllerFactory"/> class.
        /// </summary>
        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }


        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            var controller =  (IController)ninjectKernel.Get(controllerType);

            return controller;
        }

        /// <summary>
        /// Adds the bindings.
        /// </summary>
        private void AddBindings()
        {
            ninjectKernel.Bind<IIssueModel>().To<IssueModel>();
            ninjectKernel.Bind<ISearchModel>().To<SearchModel>();
        }
    }
}
