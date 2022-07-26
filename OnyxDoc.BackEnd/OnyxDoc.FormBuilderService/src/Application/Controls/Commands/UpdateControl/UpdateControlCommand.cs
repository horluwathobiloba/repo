using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.ControlProperties.Commands;
using OnyxDoc.FormBuilderService.Application.Controls.Queries;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OnyxDoc.FormBuilderService.Application.PageControlItemProperties;

namespace OnyxDoc.FormBuilderService.Application.Controls.Commands
{
    public class UpdateControlCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public InputValueType InputValueType { get; set; }
        public ControlType ControlType { get; set; }
        public BlockControlType? BlockControlType { get; set; }
        public FieldControlType? FieldControlType { get; set; }
        public string DisplayName { get; set; }
        public string ControlTips { get; set; }
        public decimal VersionNumber { get; set; }
        public List<UpdateControlPropertyRequest> ControlProperties { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateSubscriptionCommandHandler : IRequestHandler<UpdateControlCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;

        public UpdateSubscriptionCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
            _configuration = configuration;
        }
        public async Task<Result> Handle(UpdateControlCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var entity = await _context.Controls.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId && x.Id == request.Id);
                if (entity == null)
                {
                    return Result.Failure($"Invalid control specified.");
                }

                var controlNameExists = await _context.Controls.AnyAsync(x => x.SubscriberId == request.SubscriberId && x.Id != request.Id
                && (x.Name == request.Name || x.DisplayName == request.DisplayName));

                if (controlNameExists)
                {
                    return Result.Failure($"Another control named {request.Name} or with display name {request.DisplayName} already exists."); 
                }

                entity.Name = request.Name;
                entity.Description = request.Description;
                entity.SubscriberId = request.SubscriberId;
                entity.SubscriberName = _authService.Subscriber?.SubscriberName;
                entity.InputValueType = request.InputValueType;
                entity.InputValueTypeDesc = request.InputValueType.ToString();
                entity.ControlType = request.ControlType;
                entity.ControlTypeDesc = request.ControlType.ToString();
                entity.DisplayName = request.DisplayName;
                entity.ControlTips = request.ControlTips;
                entity.VersionNumber = request.VersionNumber;

                switch (entity.ControlType)
                {
                    case ControlType.BlockControl:
                        entity.BlockControlType = request.BlockControlType;
                        entity.BlockControlTypeDesc = request.BlockControlType.ToString();
                        break;
                    case ControlType.FieldControl:
                        entity.FieldControlType = request.FieldControlType;
                        entity.FieldControlTypeDesc = request.FieldControlType.ToString();
                        break;
                    default:
                        //do nothing
                        break;
                }

                entity.UserId = request.UserId;
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                await _context.BeginTransactionAsync();
                _context.Controls.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

                if (request.ControlProperties != null && request.ControlProperties.Count > 0)
                {
                    var command = new UpdateControlPropertiesCommand
                    {
                        SubscriberId = request.SubscriberId,
                        SubscriberName = _authService.Subscriber?.Name,
                        ControlId = request.Id,
                        ControlProperties = request.ControlProperties,
                        UserId = request.UserId,
                        AccessToken = request.AccessToken,

                    };
                    var helper = new ControlPropertyHelper(_context, _mapper, _authService);
                    var controlProperties = await helper.GetControlProperties(command, cancellationToken);
                    entity.ControlProperties = controlProperties;
                    #region Do not delete!
                    //var handler = new UpdateControlPropertiesCommandHandler(_context, _mapper, _authService);
                    //var controlPropertiesResult = await handler.Handle(command, cancellationToken);

                    //if (controlPropertiesResult.Succeeded == false)
                    //{
                    //    throw new Exception(controlPropertiesResult.Error + controlPropertiesResult.Message);
                    //}
                    #endregion 
                }

                await _context.CommitTransactionAsync();
                var result = _mapper.Map<ControlDto>(entity);
                return Result.Success("Control update was successful!", result);
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure($"Control update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}

