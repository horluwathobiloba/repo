﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.PageControlItemPropertyValues.Queries;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.PageControlItemPropertyValues.Commands
{
    public class ActivatePageControlItemPropertyValueCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
    }

    public class ActivatePageControlItemPropertyValueCommandHandler : IRequestHandler<ActivatePageControlItemPropertyValueCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public ActivatePageControlItemPropertyValueCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(ActivatePageControlItemPropertyValueCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var command = new UpdatePageControlItemPropertyValueStatusCommand()
                {
                    AccessToken = request.AccessToken,
                    Id = request.Id,
                    Status = Status.Active,
                    SubscriberId = request.SubscriberId,
                    UserId = request.UserId
                };
                var result = await new UpdatePageControlItemPropertyValueStatusCommandHandler( _context, _mapper, _authService).Handle(command, cancellationToken);

                return result;
            }
            catch (Exception ex)
            {
                return Result.Failure($"Page control item property value activation failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
