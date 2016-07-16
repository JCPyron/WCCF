using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;
using System.Collections;

namespace WCCFNew
{
    /// <summary>
    /// Interaction logic for GoogleCalenderPG.xaml
    /// </summary>
    public partial class GoogleCalenderPG : Page
    {
        private static string[] Scopes = { CalendarService.Scope.Calendar }; //Scope of the OAuth Client. (.Calender is Read/Write and .CalendarReadonly is Read Only)
        private static string ApplicationName = "Google Calendar Manager";
        private IClientService myService;
        private EventsResource.ListRequest request;
        private Event selEvent;
        private IList<EventAttendee> atList = new List<EventAttendee>();
        
        public GoogleCalenderPG()
        {
            InitializeComponent();

            grabDBInfo();

            UserCredential credential;

            using (var stream =
                new FileStream("client_secrets_gc.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                credPath = System.IO.Path.Combine(credPath, ".credentials/calendar-dotnet-quickstart");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            // Create Google Calendar API service.
            var service = new CalendarService(new BaseClientService.Initializer()  //Creates the Google Calender service. This is used to manage the calendar.
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            myService = service;
            request = service.Events.List("primary");

            setReadRequest();
            EventListBox.DataContext = request.Execute();

            txtStartYear.Text = DateTime.Today.AddDays(1).Year.ToString();
            txtStartMonth.Text = DateTime.Today.AddDays(1).Month.ToString();
            txtStartDay.Text = DateTime.Today.AddDays(1).Day.ToString();
            txtStartHour.Text = DateTime.Now.Hour.ToString();
            txtStartMinute.Text = DateTime.Now.Minute.ToString();

            txtEndYear.Text = DateTime.Today.AddDays(2).Year.ToString();
            txtEndMonth.Text = DateTime.Today.AddDays(2).Month.ToString();
            txtEndDay.Text = DateTime.Today.AddDays(2).Day.ToString();
            txtEndHour.Text = DateTime.Now.Hour.ToString();
            txtEndMinute.Text = DateTime.Now.Minute.ToString();
        }

        private void grabDBInfo()
        {
            var db = new SEMDBDataContext(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\SMBDB.mdf;Integrated Security=True;Connect Timeout=30");
            // Change this for final DB.
            Table<Email> clients = db.GetTable<Email>();
            
            foreach (Email address in clients)
            {
                EventAttendee attendee = new EventAttendee();
                attendee.Email = address.EmailAddress;

                atList.Add(attendee);
            }
        }

        private void setReadRequest()
        {
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 25;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
        }

        private Event createEventBody()
        {
            int startYear, startMonth, startDay, startHour, startMinute,
                endYear, endMonth, endDay, endHour, endMinute;

            try
            {
                startYear = Convert.ToInt32(txtStartYear.Text);
                startMonth = Convert.ToInt32(txtStartMonth.Text);
                startDay = Convert.ToInt32(txtStartDay.Text);
                startHour = Convert.ToInt32(txtStartHour.Text);
                startMinute = Convert.ToInt32(txtStartMinute.Text);

                endYear = Convert.ToInt32(txtEndYear.Text);
                endMonth = Convert.ToInt32(txtEndMonth.Text);
                endDay = Convert.ToInt32(txtEndDay.Text);
                endHour = Convert.ToInt32(txtEndHour.Text);
                endMinute = Convert.ToInt32(txtEndMinute.Text);

                EventDateTime evtStart = new EventDateTime();
                EventDateTime evtEnd = new EventDateTime();

                evtStart.DateTime = new DateTime(startYear, startMonth, startDay, startHour, startMinute, 0);
                evtEnd.DateTime = new DateTime(endYear, endMonth, endDay, endHour, endMinute, 0);

                Event newEvent = new Event();
                newEvent.Start = evtStart;
                newEvent.End = evtEnd;
                newEvent.ColorId = "5";
                addAttendees(newEvent); //Adds attendees to the event.

                if (txtSummary.Text.Trim() != "") { newEvent.Summary = txtSummary.Text; }

                if (txtDescription.Text.Trim() != "") { newEvent.Description = txtDescription.Text; }

                if (txtLocation.Text.Trim() != "") { newEvent.Location = txtLocation.Text; }
                return newEvent;
            }
            catch (FormatException)
            {
                MessageBox.Show("A non number or blank space has been entered into the date. Please use numbers when entering the date.");
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("An invalid date has been entered. Please make sure you enter a valid date.");
            }
            return null;
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            Event newEvent = createEventBody();
            addAttendees(newEvent);

            if (newEvent != null)
            {
                try
                {
                    EventsResource.InsertRequest insReq = new EventsResource.InsertRequest(myService, newEvent, "primary");
                    insReq.SendNotifications = true;
                    insReq.Execute();

                    if (newEvent.Summary == null)
                    {
                        MessageBox.Show("A new event will be added to your calendar that will start at " + newEvent.Start.DateTime.ToString());
                    }
                    else
                    {
                        MessageBox.Show("A new event, titled \"" + newEvent.Summary + "\", will be added to your calendar that will take place from " + newEvent.Start.DateTime.ToString() + " to " + newEvent.End.DateTime.ToString());
                    }
                }
                catch (Google.GoogleApiException)
                {
                    MessageBox.Show("An invalid time range has been entered. Please make sure your start date occurs before your end date.");
                }

                setReadRequest();
                EventListBox.DataContext = request.Execute();
            }
        }

        private void EventListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EventListBox.SelectedItem != null)
            {
                selEvent = (Event)EventListBox.SelectedItem;

                if (selEvent.Start.DateTime != null)
                {
                    txtStartYear.Text = selEvent.Start.DateTime.Value.Year.ToString();
                    txtStartMonth.Text = selEvent.Start.DateTime.Value.Month.ToString();
                    txtStartDay.Text = selEvent.Start.DateTime.Value.Day.ToString();
                    txtStartHour.Text = selEvent.Start.DateTime.Value.Hour.ToString();
                    txtStartMinute.Text = selEvent.Start.DateTime.Value.Minute.ToString();

                    txtEndYear.Text = selEvent.End.DateTime.Value.Year.ToString();
                    txtEndMonth.Text = selEvent.End.DateTime.Value.Month.ToString();
                    txtEndDay.Text = selEvent.End.DateTime.Value.Day.ToString();
                    txtEndHour.Text = selEvent.End.DateTime.Value.Hour.ToString();
                    txtEndMinute.Text = selEvent.End.DateTime.Value.Minute.ToString();
                }
                else if (selEvent.Start.Date != null)
                {
                    string[] selStartDates = selEvent.Start.Date.ToString().Split('-');
                    string[] selEndDates = selEvent.End.Date.ToString().Split('-');

                    txtStartYear.Text = selStartDates[0];
                    txtStartMonth.Text = selStartDates[1];
                    txtStartDay.Text = selStartDates[2];
                    txtStartHour.Text = "00";
                    txtStartMinute.Text = "00";

                    txtEndYear.Text = selEndDates[0];
                    txtEndMonth.Text = selEndDates[1];
                    txtEndDay.Text = selEndDates[2];
                    txtEndHour.Text = "00";
                    txtEndMinute.Text = "00";
                }
                if (selEvent.Summary != null) { txtSummary.Text = selEvent.Summary.ToString(); } else { txtSummary.Text = ""; }
                if (selEvent.Description != null) { txtDescription.Text = selEvent.Description.ToString(); } else { txtDescription.Text = ""; }
                if (selEvent.Location != null) { txtLocation.Text = selEvent.Location.ToString(); } else { txtLocation.Text = ""; }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selEvent != null)
            {
                try
                {
                    EventsResource.DeleteRequest delReq = new EventsResource.DeleteRequest(myService, "primary", selEvent.Id);
                    string deletedTitle = selEvent.Summary;
                    addAttendees(selEvent);
                    delReq.Execute();

                    if (deletedTitle == null)
                    {
                        MessageBox.Show("The selected event has been deleted.");
                    }
                    else
                    {
                        MessageBox.Show("The selected event, titled " + deletedTitle + ", has been deleted.");
                    }
                }
                catch (Google.GoogleApiException)
                {
                    MessageBox.Show("The selected event has already been deleted.");
                }

                selEvent = null;
                EventListBox.SelectedItem = null;

                setReadRequest();
                EventListBox.DataContext = request.Execute();
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Event newEvent = createEventBody();
            addAttendees(newEvent);

            if (newEvent != null && selEvent != null)
            {
                try
                {
                    EventsResource.UpdateRequest upReq = new EventsResource.UpdateRequest(myService, newEvent, "primary", selEvent.Id);
                    upReq.SendNotifications = false;
                    upReq.Execute();

                    selEvent = null;
                    EventListBox.SelectedItem = null;

                    MessageBox.Show("The selected event has been updated.");
                }
                catch (Google.GoogleApiException)
                {
                    MessageBox.Show("An invalid time range has been entered. Please make sure your start date occurs before your end date.");
                }

                setReadRequest();
                EventListBox.DataContext = request.Execute();
            }
        }

        private void addAttendees(Event currentEvent)
        {
            currentEvent.Attendees = atList;
        }
    }
}
