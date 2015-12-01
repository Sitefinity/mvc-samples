using CustomImageWidget.Mvc.Models;
using Ninject.Modules;

namespace CustomImageWidget
{
    public class InterfaceMappings : NinjectModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<ICustomImageModel>().To<CustomImageModel>();
        }
    }
}
