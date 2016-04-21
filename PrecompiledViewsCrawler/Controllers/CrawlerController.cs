using System;
using System.Web.Mvc;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers;
using PrecompiledViewsCrawler.Contracts;
using PrecompiledViewsCrawler.Models;
using PrecompiledViewsCrawler.Utilities;

namespace PrecompiledViewsCrawler.Controllers
{
    public class CrawlerController : Controller
    {
        public ActionResult Index()
        {
            return this.View();
        }

        public ICrawler Crawler
        {
            get
            {
                return ControllerModelFactory.GetModel<ICrawler>(this.GetType());
            }
        }

        public ActionResult Crawl()
        {
            var viewModel = new CrawlResultViewModel();

            this.Crawler.Crawl();

            JsonLogger.SaveToFile();

            viewModel.EndTime = DateTime.Now;
            viewModel.CrawlItems = CrawlResultBuilder.GetItems();

            return this.PartialView("CrawlResult", viewModel);
        }
    }
}