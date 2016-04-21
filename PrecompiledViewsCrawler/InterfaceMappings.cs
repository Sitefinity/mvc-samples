using Ninject.Modules;
using PrecompiledViewsCrawler.Contracts;

namespace PrecompiledViewsCrawler
{
    public class InterfaceMappings : NinjectModule
    {
        public override void Load()
        {
            this.Bind<ICrawler>().To<DefaultCrawler>();
        }
    }
}
