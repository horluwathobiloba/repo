using MediatR;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Subscriptions.Commands.CreateSubscription
{
    public class CreateSubscriptionCommand:IRequest<Result>
    {

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CompanyName { get; set; }
        public string CompanyEmail { get; set; }
        public string SuscriberCode { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
    }

    public class CreateSubscriptionCommandHandler : IRequestHandler<CreateSubscriptionCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        // private readonly IBase64ToFileConverter _fileConverter;
        private readonly IEmailService _emailService;
        public CreateSubscriptionCommandHandler(IApplicationDbContext context, IIdentityService identityService, IBase64ToFileConverter fileConverter, IEmailService emailService)
        {
            _context = context;
            _identityService = identityService;
            // _fileConverter = fileConverter;
            _emailService = emailService;
        }
        public async Task<Result> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var subscription = new Domain.Entities.Subscription
                {
                   
                    CreatedDate = DateTime.Now,
                    StatusDesc = Status.Active.ToString(),
                    Status = Status.Active,
                    Email = request.Email,
                    Location = request.Location,
                    PhoneNumber = request.PhoneNumber,
                    Name = request.Name
                };
                await _context.Subscriptions.AddAsync(subscription);
                var res = await _context.SaveChangesAsync(cancellationToken);
                return Result.Success(subscription);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Subscription creation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }

        }
    }
}
