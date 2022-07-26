using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DriveApp.Model
{
    public abstract class BaseEntity
    {
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }

    }
}
