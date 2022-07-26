using OnyxDoc.FormBuilderService.Domain.Common;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System.Collections.Generic;

namespace OnyxDoc.FormBuilderService.Domain.Entities
{
    public class Document : AuditableEntity
    {
        public string DisplayName { get; set; }
        public string Watermark { get; set; }
        public decimal VersionNumber { get; set; }
        public int InitialDocumentVersionId { get; set; }
        public string DocumentTips { get; set; }
        public DocumentType DocumentType { get; set; }
        public string DocumentTypeDesc { get; set; }
        public DocumentShareType DocumentShareType { get; set; }
        public string DocumentShareTypeDesc { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public string DocumentStatusDesc { get; set; }

        #region Document Details and Settings
        public string DocumentOwnerId { get; set; }
        public string DocumentOwner { get; set; }

        //
        //Summary:
        //      List of tags to identify the document (comma seprated)
        public string DocumentTags { get; set; }

        public bool AttachCompletedDocumentAsEmail { get; set; }
        public bool ApplyDocumentSequence { get; set; }

        public bool EnableDocumentForwarding { get; set; }
        public bool EnableSignatureForwarding { get; set; }
        public bool AllowRecipientsDownload { get; set; }
        public bool AllowSuggestedEdits { get; set; }

        #endregion

        #region Expiration Settings
        public PeriodFrequency PeriodFrequency { get; set; } = PeriodFrequency.Days;
        public int ExpiryPeriod { get; set; }

        /// <summary>
        /// Warn signers a day before expiration
        /// </summary>
        public bool WarnSigners { get; set; }
        #endregion

        #region Auto Reminders         
        public int FirstReminderInDays { get; set; }
        public List<DocumentReminder> DocumentReminders { get; set; }
        #endregion

        public List<DocumentPage> DocumentPages { get; set; }
        public List<DocumentSequence> DocumentSequences { get; set; }
    }
}
