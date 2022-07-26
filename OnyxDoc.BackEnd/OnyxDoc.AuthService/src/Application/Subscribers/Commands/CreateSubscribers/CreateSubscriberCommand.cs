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

namespace OnyxDoc.AuthService.Application.Subscribers.Commands.CreateSubscriber
{
    public class CreateSubscriberCommand : IRequest<Result>
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string ContactEmail { get; set; }
        public string StaffSize { get; set; }
        public string Industry { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Logo { get; set; }
        public string Referrer { get; set; }
        public string UserId { get; set; }
        public string ThemeColor { get; set; }
        public string ThemeColorCode { get; set; }
        public string SubscriptionPurpose { get; set; }
        public SubscriberType SubscriberType { get; set; }
        public SubscriberAccessLevel SubscriberAccessLevel { get; set; }
    }

    public class CreateSubscriberCommandHandler : IRequestHandler<CreateSubscriberCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IBase64ToFileConverter _base64ToFileConverter;
        private readonly IStringHashingService _stringHashingService;

        public CreateSubscriberCommandHandler(IApplicationDbContext context, IIdentityService identityService,
            IMapper mapper, IEmailService emailService, IConfiguration configuration,
            IBase64ToFileConverter base64ToFileConverter, IStringHashingService stringHashingService)
        {
            _context = context;
            _identityService = identityService;
            _mapper = mapper;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<Result> Handle(CreateSubscriberCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _context.BeginTransactionAsync();
                string domain = null;
                if (request.SubscriberType == SubscriberType.Corporate)
                {
                     domain = request.ContactEmail.Split("@", StringSplitOptions.None)[1];
                }
                var existingSubscriberName = await _context.Subscribers.Where(a => a.Name == request.Name && a.ContactEmail == request.ContactEmail).FirstOrDefaultAsync();
                if (existingSubscriberName != null)
                {
                    return Result.Failure("Subscriber details already exist");
                }

                var existingSubscriber = await _context.Subscribers.Where(a => a.ContactEmail == request.ContactEmail && (a.SubscriberType==SubscriberType.Corporate && a.ContactEmail.Contains(domain))).FirstOrDefaultAsync();
                if (existingSubscriber != null)
                {
                    return Result.Failure("Subscriber already exists with these email or a corporate subscriber exist with this domain name "+ request.ContactEmail);
                }
               await _context.BeginTransactionAsync();
               bool referrer = Enum.TryParse(typeof(ReferralSource), request.Referrer, true, out object referralSource);
                var subscriber = new Subscriber
                {
                    ContactEmail = request.ContactEmail,
                    Name = request.Name,
                    Industry = request.Industry,
                    Address = request.Address,
                    City = request.City,
                    Country = request.Country,
                    Referrer = request.Referrer,
                    ReferralSource = referrer ? (ReferralSource)referralSource : ReferralSource.Others,
                    SubscriberAccessLevel = request.SubscriberAccessLevel,
                    SubscriberAccessLevelDesc = request.SubscriberAccessLevel.ToString(),
                    Status = Status.Inactive,
                    CreatedDate = DateTime.Now,
                    StaffSize = request.StaffSize,
                    State = request.State,
                    ThemeColor =request.ThemeColor,
                    SubscriptionPurpose = request.SubscriptionPurpose,
                    SubscriberType = request.SubscriberType,
                    SubscriberTypeDesc=request.SubscriberType.ToString(),
                    CreatedById = request.UserId,
                    CreatedByEmail = request.ContactEmail,
                    StatusDesc = Status.Inactive.ToString(),
                    PhoneNumber = request.PhoneNumber,
                    Logo = string.IsNullOrWhiteSpace(request.Logo) ? null :await _base64ToFileConverter.ConvertBase64StringToFile(request.Logo, request.Name + ".png")
                };
                Console.Write(subscriber);

                
              
                var subscriberCount = await _context.SubscribersCount.FirstOrDefaultAsync();
                if (subscriberCount == null)
                {
                    subscriberCount = new SubscriberCount();
                    subscriberCount.Count += 1;
                    await _context.SubscribersCount.AddAsync(subscriberCount);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                subscriber.SubscriberCode = "000000" + subscriberCount.Count;
                subscriberCount.Count += 1;
                await _context.Subscribers.AddAsync(subscriber);
                 _context.SubscribersCount.Update(subscriberCount);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                //add subscription

                

                string webDomain = _configuration["WebDomain"];
                var email = new EmailVm
                {
                    Application = "OnyxDoc",
                    Subject = "Subscriber Account",
                    BCC = "",
                    CC = "",
                    RecipientEmail = request.ContactEmail,
                    FirstName = request.Name,
                    Body = "Your subscriber account has been created successfully!",
                    ButtonText = webDomain,
                    SubscriberName = subscriber.Name,
                    SubscriberId = subscriber.SubscriberId
                };
                await _emailService.SubscriberSignUp(email);
                await _context.CommitTransactionAsync();
                return Result.Success($"Subscriber creation was successful with id:{subscriber.Id}", subscriber);
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure(new string[] { "Subscriber creation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }

        }
    }
}
