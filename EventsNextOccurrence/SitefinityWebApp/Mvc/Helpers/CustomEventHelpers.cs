using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using Telerik.Sitefinity;
using Telerik.Sitefinity.Events.Model;
using Telerik.Sitefinity.Frontend.Events.Mvc.StringResources;
using Telerik.Sitefinity.Frontend.Mvc.Models;
using Telerik.Sitefinity.Localization;
using Telerik.Sitefinity.Modules.Events;
using Telerik.Sitefinity.RecurrentRules;

namespace SitefinityWebApp.Mvc.Helpers
{
    /// <summary>
    /// This is class contains helper methods for Events widget.
    /// </summary>
    public static class CustomEventHelpers
    {
        /// <summary>
        /// The event basic date description.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The event dates text.</returns>
        public static string NextOccurrenceEventDates(this ItemViewModel item)
        {
            var ev = item.DataItem as Event;
            if (ev == null)
                return string.Empty;

            if (ev.IsRecurrent && !string.IsNullOrEmpty(ev.RecurrenceExpression))
                return BuildRecurringEvent(ev);
            else
                return string.Empty;
        }

        private static string BuildRecurringEvent(Event ev)
        {
            var result = new StringBuilder();

            var start = ev.EventStart;
            var recurrenceDescriptor = GetRecurrenceDescriptor(ev.RecurrenceExpression);
            result.Append(BuildRecurringEvent(recurrenceDescriptor));

            return result.ToString();
        }

        private static string BuildHourMinute(DateTime time)
        {
            var format = string.Empty;
            if (time.Minute == 0)
                format = "hh tt";
            else
                format = "hh mm tt";

            return time.ToString(format, CultureInfo.InvariantCulture).TrimStart('0');
        }

        private static string BuildDayMonthYear(DateTime time)
        {
            return time.ToString("dd MMMM, yyyy", CultureInfo.InvariantCulture).TrimStart('0');
        }

        private static string BuildRecurringEvent(IRecurrenceDescriptor descriptor)
        {
            if (descriptor == null)
                return string.Empty;

            var result = string.Empty;

            if (descriptor.Occurrences != null)
            {
                var nextOccurrence = descriptor.Occurrences.OrderBy(o => o.Date).FirstOrDefault(o => o >= DateTime.Now);

                if (nextOccurrence != null)
                {
                    var sb = new StringBuilder();
                    sb.Append("Next occurrence is: ");
                    sb.Append(BuildDayMonthYear(nextOccurrence.ToSitefinityUITime()));
                    sb.Append(WhiteSpace);
                    sb.Append(Res.Get<EventResources>().At);
                    sb.Append(WhiteSpace);
                    sb.Append(BuildHourMinute(nextOccurrence.ToSitefinityUITime()));

                    result = sb.ToString();
                }
            }

            return result;
        }

        private static IRecurrenceDescriptor GetRecurrenceDescriptor(string recurrenceExpression)
        {
            if (string.IsNullOrEmpty(recurrenceExpression))
                return null;

            var descriptor = new ICalRecurrenceSerializer().Deserialize(recurrenceExpression);
            return descriptor;
        }

        private const string WhiteSpace = " ";
    }
}