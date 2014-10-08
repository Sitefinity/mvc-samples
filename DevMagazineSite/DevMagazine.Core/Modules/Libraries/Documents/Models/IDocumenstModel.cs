using DevMagazine.Core.Modules.Libraries.Documents.ViewModels;
using System;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;

namespace DevMagazine.Core.Modules.Libraries.Documents.Models
{
    /// <summary>
    /// Classes that implement this interface will assit in Sitefinit document processing logic
    /// </summary>
    public interface IDocumenstModel
    {
        /// <summary>
        /// Gets related document of a given instance of the DynamicContent type
        /// </summary>
        /// <param name="item">the instance of the DynamicContent type</param>
        /// <param name="fieldName">The name of the RelatedMedia field, which has the document</param>
        /// <returns>The DocumentViewModel that represents the Document or an empty one if there's none</returns>
        DocumentViewModel GetRelatedDocument(DynamicContent item, string fieldName);
    }
}
