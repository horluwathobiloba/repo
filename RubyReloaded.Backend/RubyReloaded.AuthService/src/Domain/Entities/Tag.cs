using RubyReloaded.AuthService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Domain.Entities
{
    public class Tag:AuditableEntity
    {
        public IList<TagFAQ> TagFAQs { get; set; }
    }
}
