using System;
using System.Web;
using System.Linq;
using Telerik.Sitefinity.Web;
using Telerik.Sitefinity.RelatedData;
using Telerik.Sitefinity.Modules.Libraries;
using Telerik.Sitefinity.DynamicModules.Model;

namespace DevMagazine.Core.Content
{
    /// <summary>
    /// This class provides functionality for resolving
    /// <see cref="Telerik.Sitefinity.GenericContent.Model.Content"/> items relative Url
    /// </summary>
    public static class UrlHelper
    {
        /// <summary>
        /// Gets the relative Url from content item's default Url
        /// </summary>
        /// <param name="itemDefaultUrl">The  content item's default Url.</param>
        /// <returns>Content item's relative Url</returns>
        public static string GetRelativeUrl(string itemDefaultUrl, string pageUrlName = null)
        {
            string itemRelativeUrl;

            if (string.IsNullOrWhiteSpace(pageUrlName))
            {
                var node = Telerik.Sitefinity.Web.SiteMapBase.GetActualCurrentNode();
                itemRelativeUrl = string.Concat(VirtualPathUtility.RemoveTrailingSlash(node.UrlWithoutExtension), itemDefaultUrl);
            }
            else
            {
                itemRelativeUrl = string.Format("~/{0}{1}", pageUrlName, itemDefaultUrl);
            }

            var resolvedUrl = UrlPath.ResolveUrl(itemRelativeUrl);
            return resolvedUrl;
        }

        /// <summary>
        /// Gets the related media URL.
        /// </summary>
        /// <param name="item">The DynamicContent.</param>
        /// <returns>The media's relative Url</returns>
        public static string GetRelatedMediaUrl(DynamicContent item, string fieldName)
        {
            var relatedItem = item.GetRelatedItems(fieldName).FirstOrDefault();

            if (relatedItem != null)
            {
                var imageId = relatedItem.Id;
                LibrariesManager manager = LibrariesManager.GetManager();
                Telerik.Sitefinity.Libraries.Model.Image image = manager.GetImage(imageId);
                if (image != null)
                    return image.MediaUrl;
            }

            return null;
        }
    }
}