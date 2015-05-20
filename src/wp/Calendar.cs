using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Windows;
using Windows.System;
using WPCordovaClassLib.Cordova;
using WPCordovaClassLib.Cordova.Commands;
using WPCordovaClassLib.Cordova.JSON;

namespace Cordova.Extension.Commands
{
    public class Calendar : BaseCommand
    {
       #region Types associés aux données envoyées depuis wrapper Calendar.js

        public class PluginOptions
        {
            public int? firstReminderMinutes    {get; set;}
            public int? secondReminderMinutes   {get; set; }
            public bool? recurrence             {get; set; }
            public bool? recurrenceEndDate      {get; set; }
            public string calendarName          {get; set; }

        }
        public class PluginData
        {
            public string title         { get; set; }
            public string location      { get; set; }
            public string notes         { get; set; }
            public long startTime       { get; set; }
            public long endTime         { get; set; }
            public PluginOptions options { get; set; }
        }

        #endregion


        public Calendar()
        {
            
        }

        /// <summary>
        /// Appel direct depuis le JS : window.plugins.calendar.createEvent(...)
        /// </summary>
        /// <param name="options">
        ///     ["title", "location", "notes", "ISO startDate", "ISO endTime", "plusing id"] => décodage via le Helper de cordova
        /// </param>

        public void createEvent(string options)
        {
            string[] values = JsonHelper.Deserialize<string[]>(options);

            string subject     = values[0];
            string location    = values[1];
            string notes       = values[2];
            DateTime startDate = DateTime.Parse(values[3].Replace("\"", ""), CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            DateTime endDate   = DateTime.Parse(values[4].Replace("\"", ""), CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);

            addEvent(subject, location, notes, startDate, endDate);            
        }
        /// <summary>
        /// Appel depuis le wrapper Calandar.js
        /// </summary>
        /// <param name="options">
        /// Référence aux données envoyées depuis le wrapper.
        /// Structure des données :
        ///     ["JSON event data", "plusing id"] => décodage via le Helper de cordova
        ///         avec {"JSON event data"} => décodage des données de l'event via un parser JSON autre que celui de cordova
        /// </param>
        public void createEventWithOptions(string options)
        {
            try
            {
                /* Dans le cas ou values contient 2 élements c'est que l'appel de la méthode est effectué depuis de wrapper 'Calendar.js'
                 * dans le cas contraire c'est que l'appel à la méthode est direct, values contient l'ensemble des paramètres :
                 */
                string[] values = JsonHelper.Deserialize<string[]>(options);

                // Les données complexes ne doivent pas être désérialisées via JsonHelper.Deserialize() de cordova
                var data = JsonConvert.DeserializeObject<PluginData>(values[0]);

                string subject      = data.title;
                string location     = data.location;
                string notes        = data.notes;
                DateTime startDate  = UnixTimestampToDateTime(data.startTime);
                DateTime endDate    = UnixTimestampToDateTime(data.endTime);

                addEvent(subject, location, notes, startDate, endDate);
            }
            catch (Exception e)
            {
                DispatchCommandResult(new PluginResult(PluginResult.Status.ERROR, e.Message));
            }
        }

        /// <summary>
        /// Ajotu de l'événement au clendrier
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="location"></param>
        /// <param name="notes"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public void addEvent(string subject, string location, string notes, DateTime startDate, DateTime endDate)
        {
            
            try
            {                
                SaveAppointmentTask task = new SaveAppointmentTask();
                task.StartTime  = startDate;
                task.EndTime    = endDate;
                task.Subject    = subject;
                task.Details    = notes;
                task.Location   = location;
                task.IsAllDayEvent = true;
                task.Reminder   = Reminder.OneDay;
                task.AppointmentStatus = Microsoft.Phone.UserData.AppointmentStatus.Busy;
                task.Show();

                // Mis en commentaire de manière à éviter l'affichage du message de confirmation coté JS même si l'utilisateur
                // annule la création de la tache.
                //DispatchCommandResult(new PluginResult(PluginResult.Status.OK, ""));
            }
            catch (Exception e)
            {
                DispatchCommandResult(new PluginResult(PluginResult.Status.ERROR, e.Message));
            }
        }


        /// <summary>
        /// Créé un objet DateTime à partir d'un timestamp unix en milli-secondes
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        private DateTime UnixTimestampToDateTime(long timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddMilliseconds(timestamp);
        }
    }


}