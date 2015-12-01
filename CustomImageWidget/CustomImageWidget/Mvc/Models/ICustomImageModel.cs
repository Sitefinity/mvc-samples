using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomImageWidget.Mvc.Models
{
    public interface ICustomImageModel
    {
        /// <summary>
        /// Gets or sets the image identifier.
        /// </summary>
        /// <value>
        /// The image identifier.
        /// </value>
        Guid ImageId { get; set; }

        /// <summary>
        /// Gets or sets the name of the image provider.
        /// </summary>
        /// <value>
        /// The name of the image provider.
        /// </value>
        string ImageProviderName { get; set; }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <returns></returns>
        CustomImageViewModel GetViewModel();
    }
}
