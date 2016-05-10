using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Sitefinity.Events.Model;
using Telerik.Sitefinity.Frontend.Events.Mvc.Models;
using Telerik.Sitefinity.Frontend.Mvc.Models;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Modules.Events;

namespace KendoEvents.Mvc.Models
{
    /// <summary>
    /// The model of the scheduler event widget.
    /// </summary>
    public class SchedulerEventsModel : EventModel
    {
        /// <summary>
        /// Gets the scheduler events.
        /// </summary>
        /// <returns></returns>
        public IList<SchedulerEventViewModel> GetSchedulerEvents()
        {
            var viewModel = this.CreateListViewModel(null, 1);
            var manager = (EventsManager)this.GetManager();
            var events = viewModel.Items.Select(i => i.DataItem as Event);
            var allOccurunces = manager.GetEventsOccurrences(events, DateTime.MinValue, DateTime.MaxValue);
            var schedulerEvents = allOccurunces.Select(e => new SchedulerEventViewModel(e));

            return schedulerEvents.ToList();
        }
    }
}
