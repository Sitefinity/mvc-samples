using System;
using System.Linq;
using System.Web.Mvc;
using DevMagazine.Core.Modules.Libraries.Documents.Models;
using DevMagazine.Core.Modules.Libraries.Documents.Models.Impl;
using DevMagazine.Core.Modules.Libraries.Images.Models;
using DevMagazine.Core.Modules.Libraries.Images.Models.Impl;
using Ninject;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers;
using DevMagazine.Authors.Mvc.Models;
using DevMagazine.Issues.Mvc.Models;
using DevMagazine.Search.Mvc.Models;
using DevMagazine.Issues.Mvc.Models.Impl;
using DevMagazine.Authors.Mvc.Models.Impl;
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
            ninjectKernel.Bind<IAuthorModel>().To<AuthorModel>();
            ninjectKernel.Bind<IIssueModel>().To<IssueModel>();
            ninjectKernel.Bind<IImagesModel>().To<ImagesModel>();
            ninjectKernel.Bind<IDocumenstModel>().To<DocumentsModel>();
            ninjectKernel.Bind<ISearchModel>().To<SearchModel>();
        }
    }
}
