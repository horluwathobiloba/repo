using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.PGSubscriptions.Queries;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.PGSubscriptions.Commands
{
    public class UpdatePGSubscriptionStatusCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int SubscriptionId { get; set; }        
        public int Id { get; set; }
        public Status Status { get; set; }
        public string UserId { get; set; }
    }

    public class UpdatePGSubscriptionStatusCommandHandler : IRequestHandler<UpdatePGSubscriptionStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UpdatePGSubscriptionStatusCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(UpdatePGSubscriptionStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);

                var entity = await _context.PGSubscriptions.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId 
                && x.SubscriptionId ==  request.SubscriptionId && x.Id == request.Id);
                if (entity == null)
                {
                    return Result.Failure("Invalid payment gateway subscription!");
                }

                string message = "";
                switch (request.Status)
                {
                    case Status.Inactive:
                        entity.Status = Status.Inactive;
                        message = "Payment gateway subscription is now inactive!";
                        break;
                    case Status.Active:
                        entity.Status = Status.Active;
                        message = "Payment gateway subscription was successfully activated!";
                        break;
                    case Status.Deactivated:
                        message = "Payment gateway subscription was deactivated!";
                        break;
                    default:
                        break;
                }

                entity.Status = request.Status;
                entity.StatusDesc = request.Status.ToString();
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<PGSubscriptionDto>(entity);
                return Result.Success(message, result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Payment gateway subscription status update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
