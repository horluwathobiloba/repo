using RubyReloaded.AuthService.Domain.Common;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Domain.Entities
{
    public class ExplorePostFile:AuditableEntity
    {
        public string MimeType { get; set; }
        public string Extension { get; set; }
        public List<string> ExplorePostFileURL { get; set; }
        public ExploreImageType ExploreImageType { get; set; }
    }
}
