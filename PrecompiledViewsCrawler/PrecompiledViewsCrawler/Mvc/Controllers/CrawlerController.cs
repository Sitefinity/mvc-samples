using System.Web.Mvc;
using PrecompiledViewsCrawler.Mvc.Models;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers;

namespace PrecompiledViewsCrawler.Mvc.Controllers
{
    public class CrawlerController : Controller
    {
        public CrawlerController()
        {
            this.crawlResultViewModel = ControllerModelFactory.GetModel<ICrawlResultViewModel>(this.GetType());
        }

        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult Crawl()
        {
            this.crawlResultViewModel.BuildCrawlItems();
            return this.PartialView("CrawlResult", this.crawlResultViewModel);
        }

        private readonly ICrawlResultViewModel crawlResultViewModel;
    }
}