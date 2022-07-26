using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Application.AddressBooks.Queries.GetAddressBooks;
using OnyxDoc.DocumentService.Domain.Entities;
using OnyxDoc.DocumentService.Domain.Enums;
using ReventInject;
using ReventInject.Utilities.Enums;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.DocumentService.Infrastructure.Services;

namespace OnyxDoc.DocumentService.Application.AddressBooks.Commands.UpdateAddressBook
{
    public class UpdateAddressBookCommand : IRequest<Result>
    {
        public int Id { get; set; }
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
        public string PictureExtension { get; set; }
        public string RegistrationNumber { get; set; }
        public string PostalCode { get; set; }
        public int SubscriberId { get; set; }
        public string MimeType { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateAddressBookCommandHandler : IRequestHandler<UpdateAddressBookCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobStorageService;

        public UpdateAddressBookCommandHandler(IApplicationDbContext context, IMapper mapper, IBlobStorageService blobStorageService)
        {
            _context = context;
            _mapper = mapper;
            _blobStorageService = blobStorageService;
        }
        public async Task<Result> Handle(UpdateAddressBookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.AddressBooks
                   .Where(x => x.SubscriberId == request.SubscriberId && x.Id == request.Id)
                   .FirstOrDefaultAsync();

                if (entity == null)
                {
                    return Result.Failure($"Invalid Address Book specified.");
                }

                var modfiedEntityExists = await _context.AddressBooks.AnyAsync(x => x.SubscriberId == request.SubscriberId && x.Id != request.Id
                && x.EmailAddress.ToLower() == request.EmailAddress.ToLower() && x.CompanyEmail.ToLower() == request.CompanyEmail.ToLower());

                if (modfiedEntityExists)
                {
                    return Result.Failure($"Another Address Book with this email '{request.EmailAddress}' and company email {request.CompanyEmail} already exists. Please change the email.");
                }
                if (!request.ProfilePicture.Contains("https://"))
                {
                    var fileName = request.EmailAddress.ToString() + "_" + DateTime.Now.Ticks + "." + request.PictureExtension;
                    var fileBytes = Convert.FromBase64String(request.ProfilePicture);
                    var filePath = "";
                    if (fileBytes.Length > 0)
                    {
                        filePath = await _blobStorageService.UploadFileToBlobAsync(fileName, fileBytes, request.MimeType);
                        request.ProfilePicture = filePath;
                    }
                }
               

                entity.EmailAddress = request.EmailAddress;
                entity.FirstName = request.FirstName;
                entity.LastName = request.LastName;
                entity.RegistrationNumber = request.RegistrationNumber;
                entity.CompanyEmail = request.CompanyEmail;
                entity.CompanyName = request.CompanyName;
                entity.CompanyAddress = request.CompanyAddress;
                entity.CompanyPhoneNumber = request.CompanyPhoneNumber;
                entity.Country = request.Country;
                entity.State = request.State;
                entity.PostalCode = request.PostalCode;
                entity.ProfilePicture = request.ProfilePicture;
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                _context.AddressBooks.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<AddressBookDto>(entity);
                return Result.Success("Address Book was updated successfully", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($" recipient update failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }
    }

}
