Configure Events widget to display dates in UTC
======

The following tutorial demonstrates how to modify Feather Event widget in order to display event dates in UTC. In fact you can use the same approach to modify the date formats or completely change the way dates are displayed.

# Integrate *EventsUTCDates* sample in your project

1.	Clone the feather-samples repository.
2.	Copy the file EventsUTCDates\SitefinityWebApp\Mvc\Helpers\CustomEventHelpers.cs in your web application, include it in the project and set its build action to *Compile*
3.      Copy the files from EventsUTCDates\SitefinityWebApp\Mvc\Views in the same folder structure inside your web application.
4.      Build your Sitefinity web application.

# Create the *EventsUTCDates* widget

By default the *Events* widget uses MVC helper method in order to display the dates in user friendly manner. You can modify the format of the event dates by replacing default MVC helper with your custom one. To do this follow the steps bellow:

1.     Create file named *CustomEventHelper* in the file structure SitefinityWebApp\Mvc\Helpers\ and create a static class with the same name
2.     Crete extension method in it named *UTCEventDates* with the following content:
````CSharp
        public static string UTCEventDates(this ItemViewModel item)
        {
            var ev = item.DataItem as Event;
            if (ev == null)
                return string.Empty;

            if (ev.IsRecurrent && !string.IsNullOrEmpty(ev.RecurrenceExpression))
                return BuildRecurringEvent(ev);
            else
                return BuildNonRecurringEvent(ev);
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

* Modify 

````
@item.EventDates()
````

to 

````
@item.UTCEventDates()
````

   4. Repeat the same for the Details view.

   5. Build the SitefinityWebApp project.

Now when you use Feather events widget it will display the dates in UTC format. You can further modify the format of the dates by changing the custom logic in CustomEventHelpers class.
