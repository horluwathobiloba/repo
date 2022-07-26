using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.AuthService.Domain.Enums;
using System;
using OnyxDoc.AuthService.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Configuration;

namespace OnyxDoc.AuthService.Application.Subscribers.Commands.UpdateSubscriber
{
    public class UpdateSubscriberCommand : IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string ContactEmail { get; set; }
        public string StaffSize { get; set; }
        public string Industry { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Logo { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Referrer { get; set; }
        public string UserId { get; set; }
        public string SubscriptionPurpose { get; set; }
        public SubscriberType SubscriberType { get; set; }
        public SubscriberAccessLevel SubscriberAccessLevel { get; set; }
    }

    public class UpdateSubscriberCommandHandler : IRequestHandler<UpdateSubscriberCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IBase64ToFileConverter _base64ToFileConverter;
        private readonly IStringHashingService _stringHashingService;

        public UpdateSubscriberCommandHandler(IApplicationDbContext context, IIdentityService identityService,
            IMapper mapper, IEmailService emailService, IConfiguration configuration,
            IBase64ToFileConverter base64ToFileConverter, IStringHashingService stringHashingService)
        {
            _context = context;
            _identityService = identityService;
            _mapper = mapper;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<Result> Handle(UpdateSubscriberCommand request, CancellationToken cancellationToken)
        {
            try
            {
                
                //get existing user by user id
                var user = await _identityService.GetUserById(request.UserId);

                if (user.user == null)
                {
                    return Result.Failure("Invalid User Details");
                }
                var logo = string.IsNullOrWhiteSpace(request.Logo) ? null : await _base64ToFileConverter.ConvertBase64StringToFile(request.Logo, user.user.FirstName + "_" + user.user.LastName + "_logo.png");

                var existingSubscriber = await _context.Subscribers.Where(a => a.ContactEmail == request.ContactEmail).FirstOrDefaultAsync();
                if (existingSubscriber == null)
                {
                    return Result.Failure("Invalid Subscriber Details");
                }
               await _context.BeginTransactionAsync();
                existingSubscriber.ContactEmail = request.ContactEmail;
                existingSubscriber.Name = request.Name;
                existingSubscriber.Industry = request.Industry;
                existingSubscriber.Address = request.Address;
                existingSubscriber.City = request.City;
                existingSubscriber.Country = request.Country;
                existingSubscriber.SubscriberAccessLevel = request.SubscriberAccessLevel;
                existingSubscriber.LastModifiedById = request.UserId;
                existingSubscriber.LastModifiedByEmail = user.user.Email;
                existingSubscriber.StaffSize = request.StaffSize;
                existingSubscriber.State = request.State;
                existingSubscriber.SubscriberType = request.SubscriberType;
                existingSubscriber.PhoneNumber = request.PhoneNumber;
                existingSubscriber.Logo = logo;
                existingSubscriber.Longitude = request.Longitude;
                existingSubscriber.Latitude = request.Latitude;
                
                 _context.Subscribers.Update(existingSubscriber);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
               
                return Result.Success("Subscriber update was successful", existingSubscriber);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Subscriber update was not successful", ex?.Message ?? ex?.InnerException.Message });
            }

        }
    }
}
