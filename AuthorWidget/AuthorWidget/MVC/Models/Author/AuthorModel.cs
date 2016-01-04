using System;
using System.Web;
using Telerik.Sitefinity.Modules.GenericContent;
using Telerik.Sitefinity.Modules.Libraries;
using Telerik.Sitefinity.Services;
using Telerik.Sitefinity.Web;

namespace AuthorWidget.MVC.Models.Author
{
    public class AuthorModel
    {
        public Guid PageId { get; set; }

        public string AmazonUrl { get; set; }

        public string ImageProviderName { get; set; }

        public Guid ImageId { get; set; }

        [DynamicLinksContainer]
        public string Description { get; set; }

        public string Name { get; set; }

        public string CssClass { get; set; }

        public AuthorViewModel GetViewModel()
        {
            return new AuthorViewModel()
            {
                PageUrl = this.PageUrl(),
                Description = this.Description,
                ImageUrl = this.GetImageUrl(),
                AmazonUrl = this.AmazonUrl,
                Name = this.Name,
                CssClass = this.CssClass
            };
        }

        private string PageUrl()
        {
            if (this.PageId != Guid.Empty)
	        {
                var siteMap = SitefinitySiteMap.GetCurrentProvider();

                SiteMapNode node;
                var sitefinitySiteMap = siteMap as SiteMapBase;
                if (sitefinitySiteMap != null)
                    node = sitefinitySiteMap.FindSiteMapNodeFromKey(this.PageId.ToString(), false);
                else
                    node = siteMap.FindSiteMapNodeFromKey(this.PageId.ToString());

                if (node != null)
                    return UrlPath.ResolveUrl(node.Url, true);
	        }

            return string.Empty;
        }

        private string GetImageUrl()
        {
            if (this.ImageId != Guid.Empty)
            {
                var image = LibrariesManager.GetManager(this.ImageProviderName).GetImage(this.ImageId);
                if (image != null)
                {
                    return image.Url;
                }
            }

            return string.Empty;
        }
    }
}