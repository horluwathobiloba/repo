using OnyxDoc.DocumentService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Domain.Entities
{
   public class AddressBook : AuditableEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfilePicture { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyPhoneNumber { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string RegistrationNumber { get; set; }
        public string PostalCode { get; set; }
    }
}
