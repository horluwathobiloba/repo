using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.ControlProperties.Commands
{
    public class DeleteControlPropertyCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int ControlId { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
    }


    public class DeleteControlPricingCommandHandler : IRequestHandler<DeleteControlPropertyCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public DeleteControlPricingCommandHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(DeleteControlPropertyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);

                var entity = await _context.ControlProperties.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId 
                && x.ControlId == request.ControlId && x.Id == request.Id);

                _context.ControlProperties.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Control property deleted successfully", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Delete Control property failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }

        }
    }
}
