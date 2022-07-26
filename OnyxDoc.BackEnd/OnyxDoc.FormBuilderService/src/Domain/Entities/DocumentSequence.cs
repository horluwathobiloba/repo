using OnyxDoc.FormBuilderService.Domain.Common;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnyxDoc.FormBuilderService.Domain.Entities
{
    /// <summary>
    /// Automatically send email reminders to signers who have not yet completed the document.
    /// </summary>
    public class DocumentSequence : AuditableEntity
    {
        public int DocumentId { get; set; }
        
        public int SequenceId { get; set; } 
        public bool IsDefault { get; set; }

        [ForeignKey(nameof(DocumentId))]
        public Document Document { get; set; }

        [ForeignKey(nameof(SequenceId))]
        public Sequence Sequence { get; set; }
    }
}
