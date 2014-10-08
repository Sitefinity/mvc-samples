using System;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Configuration;
using Telerik.Sitefinity.Libraries.Model;
using Telerik.Sitefinity.Modules.Libraries;
using DevMagazine.Core.Modules.Libraries.Images.ViewModels;

namespace DevMagazine.Core.Mvc.Helpers
{
    /// <summary>
    /// Contains Html Helper extensions for razor
    /// </summary>
    public static class MediaHtmlHelperExtensions
    {
        #region Public methods

        /// <summary>
        /// Renders the image.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="image">The image.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="height">The height.</param>
        /// <param name="width">The width.</param>
        /// <returns>Html helper for image</returns>
        public static IHtmlString RenderImage(this HtmlHelper helper, Image image, string className = "", string height = "", string width = "")
        {
            if (image == null)
            {
                var libManager = LibrariesManager.GetManager();

                image = libManager.GetImages().Where(i => i.Title == ConfigurationManager.AppSettings["defaultImageTitle"]).First();
            }

            return new HtmlString(string.Format(
                                        "<img src=\"{0}\" alt=\"{1}\" class=\"{2}\" heigth=\"{3}\" width=\"{4}\" />",
                                        image.MediaUrl,
                                        image.AlternativeText,
                                        className,
                                        height,
                                        width
                                        ));
        }

        /// <summary>
        /// Renders the image.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="image">The image.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="height">The height.</param>
        /// <param name="width">The width.</param>
        /// <returns>Html helper for image</returns>
        public static IHtmlString RenderImage(this HtmlHelper helper, ImageViewModel image, string className = "", string height = "", string width = "")
        {
            if (image == null)
            {
                var libManager = LibrariesManager.GetManager();

                var libImage = libManager.GetImages().Where(i => i.Title == ConfigurationManager.AppSettings["defaultImageTitle"]).First();

                image = new ImageViewModel
                {
                    ImageUrl = libImage.MediaUrl,
                    AlternativeText = libImage.AlternativeText
                };
            }

            return new HtmlString(string.Format(
                                            "<img src=\"{0}\" alt=\"{1}\" class=\"{2}\" heigth=\"{3}\" width=\"{4}\" />",
                                            image.ImageUrl,
                                            image.AlternativeText,
                                            className,
                                            height,
                                            width
                                            ));
        }

        #endregion
    }
}
