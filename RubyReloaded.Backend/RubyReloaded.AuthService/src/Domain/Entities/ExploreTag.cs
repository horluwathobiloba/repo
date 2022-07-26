using RubyReloaded.AuthService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Domain.Entities
{
    public class ExploreTag:AuditableEntity
    {
        public IList<ExploreTagPost> ExploreTagPosts { get; set; }

    }
}
