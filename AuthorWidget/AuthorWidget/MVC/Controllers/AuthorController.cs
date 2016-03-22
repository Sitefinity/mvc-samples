using System;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using AuthorWidget.MVC.Models.Author;
using Telerik.Sitefinity.Mvc;

namespace AuthorWidget.MVC.Controllers
{
    [ControllerToolboxItem(Name = "Author", Title = "Author", SectionName = "Feather samples")]
    public class AuthorController : Controller
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public AuthorModel Model
        {
            get
            {
                if (this.model == null)
                    this.model = new AuthorModel();

                return this.model;
            }
        }

        public string Template
        {
            get { return this.template; }
            set { this.template = value; }
        }

        public ActionResult Index()
        {
            return this.View("Author." + this.Template, this.Model.GetViewModel());
        }

        private AuthorModel model;
        private string template = "Default";
    }
}
