using RubyReloaded.AuthService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Domain.Entities
{
    public class ExploreTagPost:AuditableEntity
    {
        public int ExplorePostId { get; set; }
        public ExplorePost ExplorePost { get; set; }
        public int ExploreTagId { get; set; }
        public ExploreTag Tag { get; set; }
    }
}
