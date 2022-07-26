using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.PGCustomers.Commands
{
    public class DeletePGCustomerCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; } 
        public int Id { get; set; }
        public string UserId { get; set; }
    }


    public class DeletePGCustomerCommandHandler : IRequestHandler<DeletePGCustomerCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public DeletePGCustomerCommandHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(DeletePGCustomerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);

                var entity = await _context.PGCustomers.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId   && x.Id == request.Id);

                _context.PGCustomers.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Payment gateway customer deleted successfully", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Delete payment gateway customer failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }

        }
    }
}
