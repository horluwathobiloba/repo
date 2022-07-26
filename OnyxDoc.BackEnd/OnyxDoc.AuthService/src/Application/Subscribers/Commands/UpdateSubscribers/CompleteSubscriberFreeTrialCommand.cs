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
    public class CompleteSubscriberFreeTrialCommand : IRequest<Result>
    {
        public int SubscriberId { get; set; }

        public string UserId { get; set; }
    }

    public class CompleteSubscriberFreeTrialCommandHandler : IRequestHandler<CompleteSubscriberFreeTrialCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public CompleteSubscriberFreeTrialCommandHandler(IApplicationDbContext context, IIdentityService identityService,
            IMapper mapper, IEmailService emailService, IConfiguration configuration)
        {
            _context = context;
            _identityService = identityService;
            _mapper = mapper;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<Result> Handle(CompleteSubscriberFreeTrialCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _identityService.GetUserById(request.UserId);
                if (user.user == null)
                {
                    return Result.Failure("Invalid User Details");
                }
                var existingSubscriber = await _context.Subscribers.Where(a => a.SubscriberId == request.SubscriberId).FirstOrDefaultAsync();
                if (existingSubscriber == null)
                {
                    return Result.Failure("Invalid Subscriber Details");
                }
               await _context.BeginTransactionAsync();
                existingSubscriber.LastModifiedById = request.UserId;
                existingSubscriber.LastModifiedByEmail = user.user.Email;
                existingSubscriber.FreeTrialCompleted = true;
                
                 _context.Subscribers.Update(existingSubscriber);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
               
                return Result.Success("Completing Free Trial Subscription was successful", existingSubscriber);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Completing Free Trial Subscription was not successful", ex?.Message ?? ex?.InnerException.Message });
            }

        }
    }
}
