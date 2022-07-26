using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OnyxDoc.FormBuilderService.Application.Common.Models
{
    public class CreateSubscriberRequest
    {
        public string Name { get; set; }
        public SubscriberType SubscriberType { get; set; }  
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string ContactEmail { get; set; }
        public string SubscriberCode { get; set; }
        public string StaffSize { get; set; }
        public string Industry { get; set; }
        public string Location { get; set; }
        public string Referrer { get; set; }
        public string SubscriptionPurpose { get; set; }
        public string Address { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string JobTitle { get; set; }

        [NotMapped]
        public Dictionary<string, int> InvitedRecipients { get; set; }
        public string ThemeColor { get; set; }
        public string ProfilePicture { get; set; }

        public string UserId { get; set; }      

    }
}