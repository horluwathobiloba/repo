using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.AddressBooks.Queries.GetAddressBooks;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Domain.Entities;
using OnyxDoc.DocumentService.Domain.Enums;
using OnyxDoc.DocumentService.Infrastructure.Services;
using ReventInject;
using ReventInject.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.AddressBooks.Commands.CreateAddressBook
{
    public class CreateAddressBookCommand : IRequest<Result>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfilePicture { get; set; }
        public string PictureExtension { get; set; }
        public string MimeType { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyPhoneNumber { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string RegistrationNumber { get; set; }
        public string PostalCode { get; set; }
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
    }


    public class CreateAddressBookCommandHandler : IRequestHandler<CreateAddressBookCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBase64ToFileConverter _base64ToFileConverter;
        private readonly IBlobStorageService _blobStorageService;

        public CreateAddressBookCommandHandler(IApplicationDbContext context, IMapper mapper, IBlobStorageService blobStorageService,IBase64ToFileConverter base64ToFileConverter)
        {
            _context = context;
            _mapper = mapper;
            _base64ToFileConverter = base64ToFileConverter;
            _blobStorageService = blobStorageService;
        }

        public async Task<Result> Handle(CreateAddressBookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var exists = await _context.AddressBooks
                    .AnyAsync(x => x.SubscriberId == request.SubscriberId && x.EmailAddress.ToLower() == request.EmailAddress.ToLower());

                if (exists)
                {
                    return Result.Failure($" Address Book already exists for({request.EmailAddress})!");
                }
                var fileName = request.EmailAddress.ToString() + "_" + DateTime.Now.Ticks + "." + request.PictureExtension;
                var fileBytes = Convert.FromBase64String(request.ProfilePicture);
                var filePath = "";
                if (fileBytes.Length > 0)
                {
                    filePath = await _blobStorageService.UploadFileToBlobAsync(fileName, fileBytes, request.MimeType);
                }

                var entity = new Domain.Entities.AddressBook
                {
                    SubscriberId = request.SubscriberId,
                    CompanyAddress = request.CompanyAddress,
                    State = request.State,
                    PhoneNumber = request.PhoneNumber,
                    CompanyEmail = request.CompanyEmail,
                    CompanyName = request.CompanyName,
                    CompanyPhoneNumber = request.CompanyPhoneNumber,
                    Country = request.Country,
                    EmailAddress = request.EmailAddress,
                    ProfilePicture = filePath,
                    RegistrationNumber = request.RegistrationNumber,
                    PostalCode = request.PostalCode,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString(),
                     
                };

                await _context.AddressBooks.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<AddressBookDto>(entity);
                return Result.Success("AddressBook was created successfully", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"AddressBook creation failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message} ");
            }
        }
    }

}

