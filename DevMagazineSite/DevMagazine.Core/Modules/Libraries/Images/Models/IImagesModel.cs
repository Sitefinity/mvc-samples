using System;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;
using DevMagazine.Core.Modules.Libraries.Images.ViewModels;

namespace DevMagazine.Core.Modules.Libraries.Images.Models
{
    /// <summary>
    /// Classes that implement this interface will assit in Sitefinity image processing logic
    /// </summary>
    public interface IImagesModel
    {
        /// <summary>
        /// Gets related image of a given instance of the DynamicContent type
        /// </summary>
        /// <param name="item">the instance of the DynamicContent type</param>
        /// <param name="fieldName">The name of the RelatedMedia field, which has the image</param>
        /// <returns>The ImageViewModel that represents the Image or an empty one if there's none</returns>
        ImageViewModel GetRelatedImage(DynamicContent item, string fieldName);
    }
}