using RubyReloaded.AuthService.Domain.Common;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Domain.Entities
{
    public class ExplorePost : AuditableEntity
    {
        public string Header { get; set; }
        public string SubHeader { get; set; }
        public string Body { get; set; }
        public int ExploreCategoryId { get; set; }
        public ExploreCategory ExploreCategory { get; set; }
        public int ExplorePostFileId { get; set; }
        public ExplorePostFile ExplorePostFile { get; set; }
       // public List<string> PostImages { get; set; }
        public ICollection<ExploreTagPost> ExploreTagPosts { get; set; }

    }
}
