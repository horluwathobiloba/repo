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
    public class Sequence : AuditableEntity
    {
        [MaxLength(50)]
        public string SequenceName { get; set; }

        public int StartNumber { get; set; }
    }
}
