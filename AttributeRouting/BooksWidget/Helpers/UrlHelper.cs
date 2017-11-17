using System.Text;
using System.Web;
using Telerik.Sitefinity.Services;

namespace BooksWidget.Helpers
{
    internal static class UrlHelper
    {
        public static string ResolvePageRelativeUrl(string path)
        {
            StringBuilder sb = new StringBuilder();

            var currentNode = SiteMap.CurrentNode;
            if (currentNode != null)
                sb.Append(currentNode.Url);
            else
                sb.Append(VirtualPathUtility.RemoveTrailingSlash(SystemManager.CurrentHttpContext.Request.Path));

            sb.Append(path);

            return sb.ToString();
        }
    }
}
