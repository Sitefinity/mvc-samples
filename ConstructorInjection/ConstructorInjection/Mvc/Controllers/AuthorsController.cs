using ConstructorInjection.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace ConstructorInjection.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "AuthorWidget", Title = "AuthorWidget", SectionName = "MvcWidgets")]
    public class AuthorsController : Controller
    {
        #region Constructors

        public AuthorsController(IAuthorsService authorsModel)
        {
            this.model = authorsModel;
        }

        #endregion

        //Your actions logic goes here
        public ActionResult Index()
        {
            return this.View("Default", model.GetAuthors());
        }

        IAuthorsService model;
    }
}
