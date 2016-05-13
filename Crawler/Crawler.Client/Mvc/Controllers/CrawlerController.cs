using System.Web.Mvc;
using Crawler.Core;

namespace Crawler.Client.Mvc.Controllers
{
    /// <summary>
    /// MVC Controller for the Crawler
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class CrawlerController : Controller
    {
        public CrawlerController()
        {
            this.crawler = new CrawlerFacade();
        }

        /// <summary>
        /// Renders the main view.
        /// </summary>
        /// <returns>The main view.</returns>
        public ActionResult Index()
        {
            return this.View();
        }


        /// <summary>
        /// Renders the partial view for each crawl cycle.
        /// </summary>
        /// <returns>The partial view.</returns>
        public ActionResult Crawl()
        {
            var visitInfo = this.crawler.Run();
            return this.PartialView("CrawlResult", visitInfo);
        }

        private readonly CrawlerFacade crawler;
    }
}