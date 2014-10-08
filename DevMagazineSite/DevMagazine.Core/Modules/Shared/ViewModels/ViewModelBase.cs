using System;
using System.Linq;

namespace DevMagazine.Core.Modules.Shared.ViewModels
{
    /// <summary>
    /// A simple base class for all ViewModels
    /// </summary>
    public class ViewModelBase
    {
        #region Properties

        /// <summary>
        /// The Id of the item represented by the ViewModel
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Url name of the item represented by the ViewModel
        /// </summary>
        public string UrlName { get; set; }

        /// <summary>
        /// Gets or sets the item default URL presented by the ViewModel
        /// </summary>
        public string ItemDefaultUrl { get; set; }

        /// <summary>
        /// Gets or sets the name of the provider.
        /// </summary>
        public string ProviderName { get; set; }

        #endregion
    }
}
