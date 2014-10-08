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

namespace SitefinityWebApp.Mvc.HtmlHelpers
{
    public static class HtmlHelperExtensions
    {
        #region Public methods

        /// <summary>
        /// Renders the topic title taken by the serialized taxonomy filter
        /// </summary>
        /// <param name="helper">The HTML helper.</param>
        /// <param name="serializedTaxonomyFilter">The serialized taxonomy filter.</param>
        /// <param name="htmlTag">The preferred HTML tag wrapper.</param>
        /// <returns>
        /// Topic title
        /// </returns>
        public static IHtmlString RenderTopicTitle(this HtmlHelper helper, string serializedTaxonomyFilter)
        {
            var taxon = GetTaxonByTaxonomyFilter(serializedTaxonomyFilter);

            if (taxon != null)
            {
                return new HtmlString(taxon.Title);
            }

            return null;
        }

        /// <summary>
        /// Renders the topic title taken by the serialized taxonomy filter
        /// </summary>
        /// <param name="helper">The HTML helper.</param>
        /// <param name="serializedTaxonomyFilter">The serialized taxonomy filter.</param>
        /// <param name="htmlTag">The preferred HTML tag wrapper.</param>
        /// <returns>Topic title link</returns>
        public static IHtmlString RenderTopicLink(this HtmlHelper helper, string serializedTaxonomyFilter)
        {
            var taxon = GetTaxonByTaxonomyFilter(serializedTaxonomyFilter);

            if (taxon != null)
            {
                return new HtmlString(string.Format("<a href=\"../{0}\">{1}</a>", taxon.Name, taxon.Title));
            }

            return null;
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
            DynamicContent issueItem = manager.GetDataItem(issueType, relatedIssueItem.Id);

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
        #endregion

        #region Private memebers

        private static Taxon GetTaxonByTaxonomyFilter(string serializedTaxonomyFilter)
        {
            var taxonomyDictionary = (Dictionary<string, string>)ServiceStack.Text.JsonObject.Parse(serializedTaxonomyFilter);

            if (taxonomyDictionary != null && taxonomyDictionary.ContainsKey("Tags"))
            {
                var tagString = taxonomyDictionary["Tags"];

                // remove deserialized object unnecessarily symbols
                var tagGuid = tagString.Substring(2, tagString.Length - 4);

                Guid taxonId = Guid.Parse(tagGuid);

                TaxonomyManager manager = TaxonomyManager.GetManager();

                var taxon = (Telerik.Sitefinity.Taxonomies.Model.Taxon)manager.GetItem(typeof(Telerik.Sitefinity.Taxonomies.Model.Taxon), taxonId);

                return taxon;
            }

            return null;
        }

        #endregion

    }
}
