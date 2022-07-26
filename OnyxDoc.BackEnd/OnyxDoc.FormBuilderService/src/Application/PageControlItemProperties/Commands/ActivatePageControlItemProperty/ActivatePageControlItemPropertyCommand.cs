using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.PageControlItemProperties.Queries;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.PageControlItemProperties.Commands
{
    public class ActivatePageControlItemPropertyCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int PageControlItemId { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
    }

    public class ActivatePageControlItemPropertyCommandHandler : IRequestHandler<ActivatePageControlItemPropertyCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public ActivatePageControlItemPropertyCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(ActivatePageControlItemPropertyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var command = new UpdatePageControlItemPropertyStatusCommand()
                {
                    AccessToken = request.AccessToken, 
                    Id = request.Id,
                    Status = Status.Active,
                    SubscriberId = request.SubscriberId,
                    UserId = request.UserId
                };
                var result = await new UpdatePageControlItemPropertyStatusCommandHandler( _context, _mapper, _authService).Handle(command, cancellationToken);

                return result;
            }
            catch (Exception ex)
            {
                return Result.Failure($"Page control item property activation failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
