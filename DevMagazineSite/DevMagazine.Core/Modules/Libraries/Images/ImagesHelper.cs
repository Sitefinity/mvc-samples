using System;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Libraries.Model;
using Telerik.Sitefinity.RelatedData;
using DevMagazine.Core.Modules.Libraries.Images.ViewModels;

namespace DevMagazine.Core.Modules.Libraries.Images
{
    /// <summary>
    /// This class implements Sitefintiy image processing logic
    /// </summary>
    public class ImagesHelper
    {
        /// <inheritdoc />
        public static ImageViewModel GetRelatedImage(DynamicContent item, string fieldName)
        {

            if (item == null)
                throw new ArgumentNullException("item", "Item cannot be null");

            if (String.IsNullOrEmpty(fieldName))
                throw new ArgumentException(message: "Value cannot be null or empty", paramName: "fieldName");

            ImageViewModel imageModel = new ImageViewModel();

            var image = item.GetRelatedItems<Image>(fieldName).FirstOrDefault();
            if (image != null)
            {
                imageModel.Title = image.Title;
                imageModel.ThumbnailUrl = image.GetDefaultUrl();
                imageModel.ImageUrl = image.MediaUrl;
                imageModel.AlternativeText = image.AlternativeText;
            }

            return imageModel;
        }
    }
}
