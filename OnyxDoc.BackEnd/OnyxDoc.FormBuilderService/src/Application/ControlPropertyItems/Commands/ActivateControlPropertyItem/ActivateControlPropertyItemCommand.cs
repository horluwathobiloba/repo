using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.ControlPropertyItems.Queries;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.ControlPropertyItems.Commands
{
    public class ActivateControlPropertyItemCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        /*added controlpropertyid*/
        public int ControlPropertyId { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
    }

    public class ActivateControlPropertyItemCommandHandler : IRequestHandler<ActivateControlPropertyItemCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public ActivateControlPropertyItemCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(ActivateControlPropertyItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var command = new UpdateControlPropertyItemStatusCommand()
                {
                    AccessToken = request.AccessToken,
                    Id = request.Id,
                    Status = Status.Active,
                    ControlPropertyId = request.ControlPropertyId,
                    SubscriberId = request.SubscriberId,
                    UserId = request.UserId
                };
                var result = await new UpdateControlPropertyItemStatusCommandHandler( _context, _mapper, _authService).Handle(command, cancellationToken);

                return result;
            }
            catch (Exception ex)
            {
                return Result.Failure($"Control property item activation failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
