# Calendar view for Events widget using Kendo Scheduler

The following sample demonstrates how to add a calendar view using [Kendo Scheduler](http://docs.telerik.com/kendo-ui/controls/scheduling/scheduler/overview) in Sitefinity Feather Event widget.

###  Prerequisites
- .NET Framework 4.5 or higher
- [Feather](https://github.com/Sitefinity/feather/wiki/Getting-Started) 1.6 or later

### Using the Calendar view of the Events widget
1. Clone the [feather-samples](https://github.com/Sitefinity/feather-samples) repository.
1. Open **KendoEvents** project in Visual Studion.
1. Check if version of Feather nugets referenced in KendoEvents project is the same as the version that you have in your project. It they are different make sure to upgrade the KendoEvents project to your version.
1. Build the **KendoEvents** project. 
1. Reference the **KendoEvents.dll** from your Sitefinity’s web application.
1. Copy `SitefinityWebApp/Mvc/Views/Event/List.Scheduler.cshtml` to your web site root path `~/Mvc/Views/Event/List.Scheduler.cshtml`.
1. **Scheduler** is now an available option to choose a view in list settings of any Event widget instance.
