using AutoMapper;
using OnyxDoc.DocumentService.Application.Common.Mappings;
using OnyxDoc.DocumentService.Domain.Entities;
using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Application.AddressBooks.Queries.GetAddressBooks
{
    public class AddressBookDto : IMapFrom<Domain.Entities.AddressBook>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
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
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.AddressBook, AddressBookDto>();
            profile.CreateMap<AddressBookDto, Domain.Entities.AddressBook>();
        }
    }
}
