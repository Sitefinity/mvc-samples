using Ninject.Modules;
using PrecompiledViewsCrawler.Crawlers;
using PrecompiledViewsCrawler.Mvc.Models;
using PrecompiledViewsCrawler.Utilities;

namespace PrecompiledViewsCrawler
{
    public class InterfaceMappings : NinjectModule
    {
        public override void Load()
        {
            this.Bind<ICrawler>().To<DefaultCrawler>();
            this.Bind<ICrawlResultViewModel>().To<CrawlResultViewModel>();
            this.Bind<ICrawlResultBuilder>().ToConstant<DefaultCrawlResultBuilder>(new DefaultCrawlResultBuilder());
            this.Bind<IJsonLogger>().To<JsonLogger>();
        }
    }
}
