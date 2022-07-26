using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.PageControlItems.Queries;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.PageControlItems.Commands
{
    public class DeactivatePageControlItemCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int DocumentPageId { get; set; }
        public int ControlId { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
    }

    public class DeactivatePageControlItemCommandHandler : IRequestHandler<DeactivatePageControlItemCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public DeactivatePageControlItemCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(DeactivatePageControlItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var command = new UpdatePageControlItemStatusCommand()
                {
                    AccessToken = request.AccessToken,
                    Id = request.Id,
                    ControlId = request.ControlId,
                    DocumentPageId = request.DocumentPageId,
                    Status = Status.Deactivated,
                    SubscriberId = request.SubscriberId,
                    UserId = request.UserId
                };
                var result = await new UpdatePageControlItemStatusCommandHandler( _context, _mapper, _authService).Handle(command, cancellationToken);

                return result;
            }
            catch (Exception ex)
            {
                return Result.Failure($"Page control item deactivation failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
