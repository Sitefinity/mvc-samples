namespace DevMagazine.Issues
{
    /// <summary>
    /// The rendering options for the Issues widget. 
    /// </summary>
    /// <remarks>
    /// Each option describes different selection of items that will be included while rendering the Issues widget.
    /// </remarks>
    public enum IssueSelectionMode
    {
        /// <summary>
        /// Refers to all Issue items.
        /// </summary>
        AllIssues,

        /// <summary>
        /// Refers the latest issue.
        /// </summary>
        LatestIssue
    }
}
