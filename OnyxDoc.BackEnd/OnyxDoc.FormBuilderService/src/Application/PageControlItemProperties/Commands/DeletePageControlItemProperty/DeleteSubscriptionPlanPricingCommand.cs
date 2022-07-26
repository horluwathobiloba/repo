using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.PageControlItemProperties.Commands
{
    public class DeletePageControlItemPropertyCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int PageControlItemId { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
    }


    public class DeletePageControlItemPropertyCommandHandler : IRequestHandler<DeletePageControlItemPropertyCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public DeletePageControlItemPropertyCommandHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(DeletePageControlItemPropertyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);

                var entity = await _context.PageControlItemProperties.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId 
                && x.PageControlItemId == request.PageControlItemId && x.Id == request.Id);

                _context.PageControlItemProperties.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Page control item property deleted successfully", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Delete page control item property failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }

        }
    }
}
