Configure Events widget to display next occurrence of recurring event
======

The following tutorial demonstrates how to modify Feather Event widget in order to display next occurrence of recurring event.

# Integrate *EventsNextOccurrence* sample in your project

1.	Clone the feather-samples repository.
2.	Copy the file EventsUTCDates\SitefinityWebApp\Mvc\Helpers\CustomEventHelpers.cs in your web application, include it in the project and set its build action to *Compile*
3.      Copy the files from EventsUTCDates\SitefinityWebApp\Mvc\Views in the same folder structure inside your web application.
4.      Build your Sitefinity web application.

# Create the *EventsNextOccurrence* widget

By default the *Events* widget uses MVC helper method in order to display the dates in user friendly manner. You can additionally add information about the next occurrence of the recurring event by creating new MVC helper which will return the date. To do this follow the steps bellow:

1.     Create file named *CustomEventHelper* in the file structure SitefinityWebApp\Mvc\Helpers\ and create a static class with the same name
2.     Crete extension method in it named *NextOccurrenceEventDates* with the following content:
    ````CSharp
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

    ````
    Now you can add your custom logic to build the string that will be displayed for event date.

    NOTE: You may want to show different text formats depending if event is recurring or all day.

3. Modify the List view of *Events* widget to use the new format. You can use the [code](https://github.com/Sitefinity/feather-widgets/Telerik.Sitefinity.Frontend.Events/Mvc/Views/Event) of the default view, copy its content to SitefinityWebApp\Mvc\Views\Events,
and make the following changes:

    * Add using clause:

    ````
@using SitefinityWebApp.Mvc.Helpers
    ````

    * Add the following code bellow the <time>..</time>

    ````
<br><time>@item.NextOccurrenceEventDates()</time>
    ````

4. Repeat the same for the Details view.

5. Build the SitefinityWebApp project.

Now when you use Feather events widget, it will display the next occurrence next along with event default information. You can further modify the format of the dates by changing the custom logic in CustomEventHelpers class.
