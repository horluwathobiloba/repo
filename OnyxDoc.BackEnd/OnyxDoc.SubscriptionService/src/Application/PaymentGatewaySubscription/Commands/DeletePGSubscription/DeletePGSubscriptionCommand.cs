using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.PGSubscriptions.Commands
{
    public class DeletePGSubscriptionCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int SubscriptionId { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
    }


    public class DeletePGSubscriptionCommandHandler : IRequestHandler<DeletePGSubscriptionCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public DeletePGSubscriptionCommandHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(DeletePGSubscriptionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);

                var entity = await _context.PGSubscriptions.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId 
                && x.SubscriptionId == request.SubscriptionId && x.Id == request.Id);

                _context.PGSubscriptions.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Payment gateway subscription deleted successfully", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Delete payment gateway subscription failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }

        }
    }
}
