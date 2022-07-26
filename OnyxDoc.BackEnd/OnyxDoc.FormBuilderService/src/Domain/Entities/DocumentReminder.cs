using OnyxDoc.FormBuilderService.Domain.Common;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnyxDoc.FormBuilderService.Domain.Entities
{
    /// <summary>
    /// Automatically send email reminders to signers who have not yet completed the document.
    /// </summary>
    public class DocumentReminder : AuditableEntity
    {
        public int DocumentId { get; set; }
        public ReminderFrequency ReminderFrequency { get; set; }
        public DateTime? StartDateAndTime { get; set; }
        public DateTime? EndDateAndTime { get; set; }

        //
        //Summary:
        //      Optional settings for Daily Reminders 
        public int RepeatEvery { get; set; }
        public DayAndWeekFrequency DayAndWeekFrequency { get; set; }


        #region Weekly Settings

        //
        //Summary:
        //      Optional settings for Monthly Reminders: Contains list of weeks in a Month (comma seprated)       
        public string ListOfDaysInWeek { get; set; }
        #endregion

        //
        //Summary:
        //      Optional settings for Monthly Reminders: Contains list of months in the year  (comma seprated)  
        public string ListOfMonths { get; set; }

        //
        //Summary:
        //      Optional settings for Monthly Reminders: Contains list of days in a Month (comma seprated)
        public string ListOfDaysInMonth { get; set; }

        //
        //Summary:
        //     Optional settings for Monthly Reminders: Contains list of weeks in a Month (comma seprated)
        public string ListOfWeeksInMonth { get; set; }

        [ForeignKey(nameof(DocumentId))]
        public Document Document { get; set; }
    }
}
