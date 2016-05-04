using Ninject.Modules;
using PrecompiledViewsCrawler.Crawlers;
using PrecompiledViewsCrawler.Mvc.Models;
using PrecompiledViewsCrawler.Utilities;

namespace PrecompiledViewsCrawler
{
    /// <summary>
    /// This class is used to describe the bindings which will be used by the Ninject container when resolving classes.
    /// </summary>
    /// <seealso cref="Ninject.Modules.NinjectModule" />
    public class InterfaceMappings : NinjectModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            this.Bind<ICrawler>().To<DefaultCrawler>();
            this.Bind<ICrawlResultViewModel>().To<CrawlResultViewModel>();
            this.Bind<ICrawlResultBuilder>().ToConstant<DefaultCrawlResultBuilder>(new DefaultCrawlResultBuilder());
            this.Bind<IJsonLogger>().To<JsonLogger>();
        }
    }
}
