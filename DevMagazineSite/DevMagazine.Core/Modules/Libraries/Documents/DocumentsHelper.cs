using DevMagazine.Core.Modules.Libraries.Documents.ViewModels;
using System;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Libraries.Model;
using Telerik.Sitefinity.RelatedData;

namespace DevMagazine.Core.Modules.Libraries.Documents
{
    /// <summary>
    /// This class implements Sitefintiy document processing logic
    /// </summary>
    public class DocumentsHelper
    {
        /// <inheritdoc />
        public static DocumentViewModel GetRelatedDocument(DynamicContent item, string fieldName)
        {

            if (item == null)
                throw new ArgumentNullException("item", "Item cannot be null");

            if (String.IsNullOrEmpty(fieldName))
                throw new ArgumentException(message: "Value cannot be null or empty", paramName: "fieldName");

            DocumentViewModel documentModel = new DocumentViewModel();

            var document = item.GetRelatedItems<Document>(fieldName).FirstOrDefault();
            if (document != null)
            {
                documentModel.Title = document.Title;
                documentModel.DownloadUrl = document.MediaUrl;
                documentModel.Description = document.Description;
                documentModel.Id = document.Id;
                documentModel.Extension = document.Extension;
            }

            return documentModel;
        }
    }
}
