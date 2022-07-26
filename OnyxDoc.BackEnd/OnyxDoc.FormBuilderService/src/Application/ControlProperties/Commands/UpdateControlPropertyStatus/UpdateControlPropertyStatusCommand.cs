﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.ControlProperties.Queries;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.ControlProperties.Commands
{
    public class UpdateControlPropertyStatusCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int ControlId { get; set; }        
        public int Id { get; set; }
        public Status Status { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateControlPropertyStatusCommandHandler : IRequestHandler<UpdateControlPropertyStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UpdateControlPropertyStatusCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(UpdateControlPropertyStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId, request.ControlId);

                var entity = await _context.ControlProperties.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId 
                && x.ControlId ==  request.ControlId && x.Id == request.Id);
                if (entity == null)
                {
                    return Result.Failure("Invalid control properties");
                }

                string message = "";
                switch (request.Status)
                {
                    case Status.Inactive:
                        entity.Status = Status.Inactive;
                        message = "Control property is now inactive!";
                        break;
                    case Status.Active:
                        entity.Status = Status.Active;
                        message = "Control property was successfully activated!";
                        break;
                    case Status.Deactivated:
                        message = "Control property was deactivated!";
                        break;
                    default:
                        break;
                }

                entity.Status = request.Status;
                entity.StatusDesc = request.Status.ToString();
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<ControlPropertyDto>(entity);
                return Result.Success(message, result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Control property status update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
