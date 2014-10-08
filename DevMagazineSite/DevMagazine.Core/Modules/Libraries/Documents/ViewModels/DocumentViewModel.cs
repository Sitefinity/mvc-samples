using System;
using System.Linq;
using DevMagazine.Core.Modules.Shared.ViewModels;

namespace DevMagazine.Core.Modules.Libraries.Documents.ViewModels
{
    /// <summary>
    /// A view model for representing the Doocument content type
    /// </summary>
    public class DocumentViewModel : ViewModelBase
    {
        /// <summary>
        /// The title of the document
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The description of the document
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The download link of the document
        /// </summary>
        public string DownloadUrl { get; set; }

        /// <summary>
        /// The extension of the document, i.e. pdf, txt and etc.
        /// </summary>
        public string Extension { get; set; }

    }
}
