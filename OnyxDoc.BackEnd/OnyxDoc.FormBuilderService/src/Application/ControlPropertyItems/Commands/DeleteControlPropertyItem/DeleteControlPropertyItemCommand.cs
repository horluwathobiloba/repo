using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.ControlPropertyItems.Commands
{
    public class DeleteControlPropertyItemCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int ControlPropertyId { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
    }


    public class DeleteControlPropertyItemCommandHandler : IRequestHandler<DeleteControlPropertyItemCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public DeleteControlPropertyItemCommandHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(DeleteControlPropertyItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);

                var entity = await _context.ControlPropertyItems.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId 
                && x.ControlPropertyId == request.ControlPropertyId && x.Id == request.Id);

                _context.ControlPropertyItems.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Control property item deleted successfully", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Delete Control property item failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }

        }
    }
}
