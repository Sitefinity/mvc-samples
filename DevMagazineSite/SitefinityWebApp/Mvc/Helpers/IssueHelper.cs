using SitefinityWebApp.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;

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
        #endregion
    }
}
