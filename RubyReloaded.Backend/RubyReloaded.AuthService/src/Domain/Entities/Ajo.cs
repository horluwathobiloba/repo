using RubyReloaded.AuthService.Domain.Common;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Domain.Entities
{
    public class Ajo:AuditableEntity
    {
        public string Email { get; set; }
        public CollectionCycle CollectionCycle { get; set; }
        public decimal CollectionAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfUsers  { get; set; }
        public decimal AmountPerUser  { get; set; }
        public decimal AmountToDisbursePerUser  { get; set; }
        public string Code { get; set; }

    }
}
