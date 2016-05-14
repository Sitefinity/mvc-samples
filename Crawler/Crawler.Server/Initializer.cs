using System.Web.Mvc;
using System.Web.Routing;
using Crawler.Server.Mvc.Configurations;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Data;

namespace Crawler.Server
{
    /// <summary>
    /// This class provides methods for initializing the crawler.
    /// </summary>
    public class Initializer
    {
        /// <summary>
        /// Attaches the crawler initialize method to the Bootstrapper.Initialized event.
        /// </summary>
        public static void Initialize()
        {
            Bootstrapper.Initialized += Bootstrapper_Initialized;
        }

        /// <summary>
        /// Method that initializes the crawler.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ExecutedEventArgs"/> instance containing the event data.</param>
        private static void Bootstrapper_Initialized(object sender, ExecutedEventArgs e)
        {
            if (e.CommandName == "Bootstrapped")
            {
                CrawlerConfig.RegisterCrawler(GlobalFilters.Filters, RouteTable.Routes);
            }
        } 
    }
}
