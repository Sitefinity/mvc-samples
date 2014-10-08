using System;
using System.Web.Mvc;
using System.Linq;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Services;
using Telerik.Sitefinity.ContentLocations;
using Telerik.Sitefinity.Web;
using DevMagazine.Core.Modules.Shared.ViewModels;

namespace DevMagazine.Core.Mvc.Helpers
{
    /// <summary>
    /// Contains Html Helper extensions for razor
    /// </summary>
    public static class UrlHtmlHelperExtensions
    {
        #region Public methods

        /// <summary>
        /// Gets the relative URL from item's default URL
        /// </summary>
        /// <param name="helper">The Html helper</param>
        /// <param name="itemDefaultUrl">The item's default URL.</param>
        /// <param name="pageUrlName"></param>
        /// <returns>Item's relative URL</returns>
        public static string GetRelativeUrl(this HtmlHelper helper, string itemDefaultUrl, string pageUrlName = null)
        {
            return DevMagazine.Core.Content.UrlHelper.GetRelativeUrl(itemDefaultUrl, pageUrlName);
        }

        /// <summary>
        /// Returns the item content item defautl Url usign the location service
        /// </summary>
        /// <param name="helper">The Html helper</param>
        /// <param name="item">The IDataItem for which the defautl Url will be searched</param>
        /// /// <param name="alternativeUrl">Alternative Url of the dataItem. It will be used if the default Url is not resolved</param>
        /// <returns>The default Url of the item</returns>
        public static string GetItemDefaultUrl(this HtmlHelper helper, IDataItem item, string alternativeUrl = null)
        {
            // first we need to get an instance of the location service
            var contentService = SystemManager.GetContentLocationService();

            // use the location service to get the item location
            var defaultLocation = contentService.GetItemDefaultLocation(item);

            return UrlHtmlHelperExtensions.GetItemDefaultUrl(helper, defaultLocation, alternativeUrl);
        }

        /// <summary>
        /// Returns the item content item defautl Url usign the location service
        /// </summary>
        /// <param name="helper">The Html helper</param>
        /// <param name="itemType">Type of the item.</param>
        /// <param name="viewModel">The base view model</param>
        /// /// <param name="alternativeUrl">Alternative Url of the dataItem. It will be used if the default Url is not resolved</param>
        /// <returns>The default Url of the item</returns>
        public static string GetItemDefaultUrl(this HtmlHelper helper, Type itemType, ViewModelBase viewModel, string alternativeUrl = null)
        {
            return UrlHtmlHelperExtensions.GetItemDefaultUrl(helper, itemType, viewModel.ProviderName, viewModel.Id, alternativeUrl);
        }



        /// <summary>
        /// Returns the item content item defautl Url usign the location service
        /// </summary>
        /// <param name="helper">The Html helper</param>
        /// <param name="itemType">Type of the item.</param>
        /// <param name="itemProvider">The item provider.</param>
        /// <param name="itemId">The item id.</param>
        /// /// <param name="alternativeUrl">Alternative Url of the dataItem. It will be used if the default Url is not resolved</param>
        /// <returns>The default Url of the item</returns>
        public static string GetItemDefaultUrl(this HtmlHelper helper, Type itemType, string itemProvider, Guid itemId, string alternativeUrl = null)
        {
            //first we need to get an instance of the location service
            var contentService = SystemManager.GetContentLocationService();

            // use the location service to get the item location
            var defaultLocation = contentService.GetItemDefaultLocation(itemType, itemProvider, itemId);

            return UrlHtmlHelperExtensions.GetItemDefaultUrl(helper, defaultLocation, alternativeUrl);
        }

        /// <summary>
        /// Gets the URL of the current page.
        /// </summary>
        /// <param name="helper">The Html helper</param>
        /// <returns>URL of the currents page without a trailing slash.</returns>
        public static string GetCurrentNodeUrl(this HtmlHelper helper)
        {
            var currentSiteMap = (SiteMapBase)SitefinitySiteMap.GetCurrentProvider();
            var currentNode = currentSiteMap.CurrentNode;
            var url = string.Empty;

            if (currentNode != null)
            {
                url = UrlPath.ResolveUrl(currentNode.Url, absolute: false, removeTrailingSlash: true);
            }

            return url;
        }

        #endregion

        #region Private methods

        /// <summary>
        ///  Returns the item content item defautl Url usign given content location
        /// </summary>
        /// <param name="helper">The Html helper</param>
        /// <param name="location">The location of the item</param>
        /// <param name="alternativeUrl">Alternative Url of the dataItem. It will be used if the location is null</param>
        /// <returns>The default Url of the item</returns>
        private static string GetItemDefaultUrl(this HtmlHelper helper, IContentItemLocation location, string alternativeUrl = null)
        {
            var result = string.Empty;

            if (location != null)
            {
                result = location.ItemAbsoluteUrl;
            }
            else if (!string.IsNullOrEmpty(alternativeUrl))
            {
                result = UrlHtmlHelperExtensions.GetRelativeUrl(helper, alternativeUrl);
            }

            return result;
        }

        #endregion
    }
}
