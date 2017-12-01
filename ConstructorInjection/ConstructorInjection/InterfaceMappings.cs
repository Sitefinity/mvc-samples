using ConstructorInjection.Mvc.Models;
using Ninject.Modules;

namespace ConstructorInjection
{
    public class InterfaceMappings : NinjectModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            this.Bind<IAuthorsService>().To<AuthorsService>();
        }
    }
}
