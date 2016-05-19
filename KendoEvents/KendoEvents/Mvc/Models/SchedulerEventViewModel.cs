using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Sitefinity.Frontend.Mvc.Models;
using Telerik.Sitefinity.Modules.Events;

namespace KendoEvents.Mvc.Models
{
    /// <summary>
    /// This is the view model required by Kendo scheduler.
    /// </summary>
    public class SchedulerEventViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulerEventViewModel"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        public SchedulerEventViewModel(EventOccurrence item)
        {
            this.Id = item.Event.Id;
            this.Title = item.Title;
            this.Description = item.Description;
            this.Start = item.StartDate;
            this.End = item.EndDate ?? DateTime.MaxValue;
            this.RecurrenceID = item.Event.Id;
            this.IsAllDay = item.Event.AllDayEvent;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the start timezone.
        /// </summary>
        /// <value>
        /// The start timezone.
        /// </value>
        public string StartTimezone { get; set; }

        /// <summary>
        /// Gets or sets the start.
        /// </summary>
        /// <value>
        /// The start.
        /// </value>
        public DateTime Start
        {
            get
            {
                return start;
            }
            set
            {
                start = value.ToUniversalTime();
            }
        }

        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        /// <value>
        /// The end.
        /// </value>
        public DateTime End
        {
            get
            {
                return end;
            }
            set
            {
                end = value.ToUniversalTime();
            }
        }


        /// <summary>
        /// Gets or sets the end timezone.
        /// </summary>
        /// <value>
        /// The end timezone.
        /// </value>
        public string EndTimezone { get; set; }

        /// <summary>
        /// Gets or sets the recurrence rule.
        /// </summary>
        /// <value>
        /// The recurrence rule.
        /// </value>
        public string RecurrenceRule { get; set; }

        /// <summary>
        /// Gets or sets the recurrence identifier.
        /// </summary>
        /// <value>
        /// The recurrence identifier.
        /// </value>
        public Guid? RecurrenceID { get; set; }

        /// <summary>
        /// Gets or sets the recurrence exception.
        /// </summary>
        /// <value>
        /// The recurrence exception.
        /// </value>
        public string RecurrenceException { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is all day].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is all day]; otherwise, <c>false</c>.
        /// </value>
        public bool IsAllDay { get; set; }

        private DateTime start;
        private DateTime end;
    }
}
