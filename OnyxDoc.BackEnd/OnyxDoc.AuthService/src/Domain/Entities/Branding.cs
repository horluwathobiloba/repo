using OnyxDoc.AuthService.Domain.Common;
using OnyxDoc.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.AuthService.Domain.Entities
{
    public class Branding : AuditableEntity
    {
        public string Logo { get; set; }
        public string ThemeColor { get; set; }
        public string ThemeColorCode { get; set; }
        public string FooterInformation { get; set; }
        public bool HasFooterInformation { get; set; }
    }
}