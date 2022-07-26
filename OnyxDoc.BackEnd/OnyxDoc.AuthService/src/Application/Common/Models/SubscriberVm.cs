using OnyxDoc.AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.AuthService.Application.Common.Models
{
    public class SubscriberVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<User> Users { get; set; }
        public string SubscriberCode { get; set; }
        public string Logo { get; set; }
    }
}
