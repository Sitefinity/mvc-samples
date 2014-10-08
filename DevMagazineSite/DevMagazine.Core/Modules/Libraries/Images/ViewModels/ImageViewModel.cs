using System;
using System.Linq;
using DevMagazine.Core.Modules.Shared.ViewModels;

namespace DevMagazine.Core.Modules.Libraries.Images.ViewModels
{
    /// <summary>
    /// A view model for representing the Images content type
    /// </summary>
    public class ImageViewModel : ViewModelBase
    {
        /// <summary>
        /// The title of the image
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The description of the image
        /// </summary>
        public string AlternativeText { get; set; }

        /// <summary>
        /// The image Url
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// The thumbnail Url
        /// </summary>
        public string ThumbnailUrl { get; set; }
    }
}
