using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Telerik.Sitefinity.Taxonomies;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Taxonomies.Model;
using Telerik.Sitefinity.RelatedData;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.News.Model;
using Telerik.Sitefinity.Modules.Pages;
using DevMagazine.Core.Mvc.Helpers;
using Telerik.Sitefinity.Web;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Frontend.Mvc.Models;

namespace SitefinityWebApp.Mvc.HtmlHelpers
{
    public static class HtmlHelperExtensions
    {
        #region Public methods

        /// <summary>
        /// Renders the topic title as link.
        /// </summary>
        /// <param name="helper">The HTML helper.</param>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Topic title link
        /// </returns>
        public static IHtmlString RenderTopicLink(this HtmlHelper helper, ContentListViewModel model)
        {
            var viewItem = model.Items.FirstOrDefault();

            if (viewItem == null)
                return null;
            
            var taxon = viewItem.GetFlatTaxon("Tags");

            if (taxon == null)
                return null;
            
            return new HtmlString(string.Format("<a href=\"../{0}\">{1}</a>", taxon.Name, taxon.Title));
        }

        /// <summary>
        /// Renders the related issue item.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="newsItem">The news item.</param>
        /// <returns>Link with related Issue</returns>
        public static IHtmlString RenderRelatedIssueItem(this HtmlHelper helper, NewsItem newsItem)
        {
            Type issueType = Telerik.Sitefinity.Utilities.TypeConverters.TypeResolutionService.ResolveType("Telerik.Sitefinity.DynamicTypes.Model.Issues.Issue");
            IDataItem relatedIssueItem = newsItem.GetRelatedParentItems(issueType.FullName).FirstOrDefault();

            if (relatedIssueItem == null)
                return null;

            DynamicModuleManager manager = DynamicModuleManager.GetManager();
            var issueItem = manager.GetDataItem(issueType, relatedIssueItem.Id);

            var sb = new StringBuilder();
            sb.AppendLine("<span class=\"text-muted\">|</span>");
            sb.AppendFormat("<a href=\"{0}\">{1}</a>", UrlHtmlHelperExtensions.GetItemDefaultUrl(helper, issueItem), issueItem.GetValue("Title"));

            return new HtmlString(sb.ToString());
        }

        /// <summary>
        /// Renders the page tag link.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="newsItem">The news item.</param>
        /// <param name="urlHelper">The URL helper.</param>
        /// <returns>Current page tag's link</returns>
        public static IHtmlString RenderPageTagLink(this HtmlHelper helper, NewsItem newsItem, System.Web.Mvc.UrlHelper urlHelper)
        {
            var tagId = ((Telerik.OpenAccess.TrackedList<System.Guid>)newsItem.GetValue("Tags")).First();

            var taxonomyManager = TaxonomyManager.GetManager();
            var tag = taxonomyManager.GetTaxa<FlatTaxon>().Where(t => t.Id == tagId).Single();

            var pageManager = PageManager.GetManager();
            var pageNode = pageManager.GetPageNodes().Where(node => node.Title.Contains(tag.Name) && node.RootNodeId == SiteInitializer.CurrentFrontendRootNodeId).FirstOrDefault();

            return pageNode == null ? null : new HtmlString(string.Format("<a href=\"{0}\">{1}</a>", urlHelper.Content(pageNode.GetFullUrl()), tag.Title));
        }

        /// <summary>
        /// Constructs collection of news items.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>Collection of <see cref="NewsItem"/></returns>
        public static IEnumerable<NewsItem> AsNewsItems(this IEnumerable<ItemViewModel> items)
        {
            var newsItems = items.Select(i=>(Telerik.Sitefinity.News.Model.NewsItem)i.DataItem).ToList();

            return newsItems;
        }

        /// <summary>
        /// Constructs collection of news items. 
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>Collection of <see cref="NewsItem"/></returns>
        public static T GetDataItem<T>(this ItemViewModel item) where T: class
        {
            return item.DataItem as T;
        }


        #endregion
    }
}
