using SitefinityWebApp.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Libraries.Model;
using Telerik.Sitefinity.RelatedData;

namespace SitefinityWebApp.Mvc.Helpers
{
    /// <summary>
    /// Contains Helper methods for Issues
    /// </summary>
    public class IssueHelper
    {
        #region Static methods
        /// <summary>
        /// Gets the latest issue.
        /// </summary>
        /// <returns></returns>
        public static DynamicContent GetLatestIssue()
        {
            var dynamicManager = DynamicModuleManager.GetManager();

            var latestIssue = dynamicManager.GetDataItems(IssueViewModel.IssueType)
              .Where(di => di.Status == ContentLifecycleStatus.Live)
              .Where(di => di.Visible == true)
              .OrderByDescending(di => di.PublicationDate)
              .FirstOrDefault();

            return latestIssue;
        }

        /// <summary>
        /// Gets the document URL per issue.
        /// </summary>
        /// <returns></returns>
        public static string GetDocumentUrlFromContext()
        {
            var query = HttpContext.Current.Request.Url.Query;
            var queryIdValue = HttpUtility.ParseQueryString(query).Get("issue");
            if (!string.IsNullOrEmpty(queryIdValue))
            {
                var issueId = Guid.Parse(queryIdValue);


                var dynamicManager = DynamicModuleManager.GetManager();

                var issue = dynamicManager.GetDataItem(IssueViewModel.IssueType, issueId);

                if (issue != null)
                {
                    var document = issue.GetRelatedItems<Document>("IssueDocument").FirstOrDefault();

                    if (document != null)
                        return document.MediaUrl;
                }
            }

            return string.Empty;
        }

        #endregion
    }
}
