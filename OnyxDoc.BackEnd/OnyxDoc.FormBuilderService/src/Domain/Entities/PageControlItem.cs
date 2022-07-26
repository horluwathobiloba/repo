using OnyxDoc.FormBuilderService.Domain.Common;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnyxDoc.FormBuilderService.Domain.Entities
{
    public class PageControlItem : AuditableEntity
    {
        public int DocumentPageId { get; set; }
        public int ControlId { get; set; }

        #region Value captured based on the control's input value type
        public string TextValue { get; set; }
        public long NumberValue { get; set; }
        public decimal FloatValue { get; set; }
        public bool BooleanValue { get; set; }
        public DateTime DateTimeValue { get; set; }
        public string BlobValue { get; set; }


        /// <summary>
        /// Get the value based on the control value type
        /// </summary>
        [NotMapped]
        public object GetValue
        {
            get
            {
                switch (this.Control?.InputValueType)
                {
                    case InputValueType.Boolean:
                        return BooleanValue;

                    case InputValueType.Date:
                        return DateTimeValue;

                    case InputValueType.Number:
                        return NumberValue;

                    case InputValueType.String:
                        return TextValue;

                    case InputValueType.Image:
                    case InputValueType.Signature:
                    case InputValueType.File:
                        return BlobValue;

                    default:
                        return null;
                }
            }
        }
        #endregion


        #region Page ControlItem Dimension
        public string Height { get; set; }
        public string Width { get; set; }
        public string Position { get; set; }
        public string Transform { get; set; }
        #endregion

        [ForeignKey(nameof(DocumentPageId))]
        public DocumentPage DocumentPage { get; set; }

        [ForeignKey(nameof(ControlId))]
        public Control Control { get; set; }

        public List<PageControlItemProperty> PageControlItemProperties { get; set; }

    }
}
