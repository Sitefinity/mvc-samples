using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Data;
using DevMagazine.Issues.Mvc.ViewModels;

namespace DevMagazine.Issues.Mvc.Models
{
    /// <summary>
    /// Classes that implement this interface could be used as view model for the Issues widget.
    /// </summary>
    public interface IIssueModel : ICacheDependable
    {
        /// <summary>
        /// Gets the list of issues to be displayed inside the widget.
        /// </summary>
        /// <value>
        /// The issues collection.
        /// </value>
        [Browsable(false)]
        IEnumerable<IssueViewModel> Issues { get; }

        /// <summary>
        /// Gets the the detailed issue that will be displayed by the widget
        /// </summary>
        /// <value>
        /// The issues collection.
        /// </value>
        [Browsable(false)]
        IssueViewModel DetailIssue { get; set; }

        /// <summary>
        /// Gets or sets the detail page URL.
        /// </summary>
        /// <value>
        /// The detail page URL.
        /// </value>
        string DetailPageUrl { get; set; }

        /// <summary>
        /// Gets or sets the name of the provider.
        /// </summary>
        /// <value>The name of the provider.</value>
        string ProviderName { get; set; }

        /// <summary>
        /// Gets or sets the items per page
        /// </summary>
        /// <value>The items per page</value>
        int ItemsPerPage { get; set; }

        /// <summary>
        /// Gets or sets the initial items per page
        /// </summary>
        /// <value>The number of initial items</value>
        int InitialItems { get; set; }

        /// <summary>
        /// Gets or sets the total number of pages
        /// </summary>
        /// <value>Total number of pages</value>
        int? TotalPagesCount { get; set; }

        /// <summary>
        /// Gets or sets the current page number
        /// </summary>
        /// <value>The current page number</value>
        int CurrentPage { get; set; }

        /// <summary>
        /// Populates the issues model.
        /// </summary>
        /// <param name="selectionMode">The selection mode for the isses.</param>
        void PopulateModel(IssueSelectionMode selectionMode);

        /// <summary>
        /// Gets the issue.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        IssueViewModel GetIssue(DynamicContent item);

        /// <summary>
        /// Gets issue using its Id
        /// </summary>
        /// <param name="id">Guid, the Id of the issue</param>
        /// <returns>IssueViewModel</returns>
        IssueViewModel GetIssue(Guid id);

        /// <summary>
        /// Gets issue using its urlName
        /// </summary>
        /// <param name="urlName">The url name of the issue</param>
        /// <param name="found">Indicated if the item has been found or not</param>
        /// <returns>IssueViewModel</returns>
        IssueViewModel GetIssue(string urlName, ref bool found);
    }
}
