using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.DynamicModules.Builder;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Frontend.Mvc.Models;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Utilities.TypeConverters;

namespace SingleDynamicContent.Mvc.Models
{
    public class SingleDynamicContentModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Reference object.
        /// </summary>
        public DynamicContent Item
        {
            get
            {
                return this.item;
            }
            set
            {
                this.item = value;
            }
        }

        /// <summary>
        /// Gets or sets the Reference object.
        /// </summary>
        public string SelectedItemId
        {
            get
            {
                return this.selectedItemId;
            }
            set
            {
                this.selectedItemId = value;
            }
        }

        #endregion

        #region Public Methods

        public void Populate(string itemType)
        {
            var dynamicModuleManager = DynamicModuleManager.GetManager();
            Type referenceType = TypeResolutionService.ResolveType(itemType);

            var query = dynamicModuleManager.GetDataItems(referenceType).Where(s => s.Status == Telerik.Sitefinity.GenericContent.Model.ContentLifecycleStatus.Live);

            Guid itemId;
            if (!string.IsNullOrWhiteSpace(this.SelectedItemId) && Guid.TryParse(this.SelectedItemId, out itemId))
                this.Item = query.SingleOrDefault(n => n.Id == itemId);
            else
                this.Item = query.FirstOrDefault();
        }

        #endregion

        #region Private fields

        private DynamicContent item;
        private string selectedItemId;

        #endregion
    }
}
