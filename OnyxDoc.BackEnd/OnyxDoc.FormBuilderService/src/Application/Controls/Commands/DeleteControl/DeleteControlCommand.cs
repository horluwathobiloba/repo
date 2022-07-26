using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.Controls.Commands
{
    public class DeleteControlCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
    }


    public class DeleteControlCommandHandler : IRequestHandler<DeleteControlCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public DeleteControlCommandHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(DeleteControlCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var entity = await _context.Controls.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId && x.Id == request.Id);
                _context.Controls.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Control deleted successfully", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Delete Control failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }

        }
    }
}
