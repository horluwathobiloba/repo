using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OnyxDoc.FormBuilderService.Domain.Enums;
using OnyxDoc.FormBuilderService.Domain.Entities;
using OnyxDoc.FormBuilderService.Application.Controls.Queries;
using OnyxDoc.FormBuilderService.Application.ControlProperties.Commands;
using OnyxDoc.FormBuilderService.Application.PageControlItemProperties;

namespace OnyxDoc.FormBuilderService.Application.Controls.Commands
{
    public class CreateControlCommand : AuthToken, IRequest<Result>
    {
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

    public class CreateSubscriptionCommandHandler : IRequestHandler<CreateControlCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IAuthService _authService;
        private INotificationService _notificationService;
        private IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public CreateSubscriptionCommandHandler(IApplicationDbContext context, IMapper mapper, IMediator mediator,
            IAuthService authService, IConfiguration configuration, INotificationService notificationService, IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
            _authService = authService;
            _configuration = configuration;
            _notificationService = notificationService;
            _emailService = emailService;
        }

        public async Task<Result> Handle(CreateControlCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                //get user object
                var user = await _authService.GetUserAsync(request.AccessToken, request.SubscriberId, request.UserId, request.UserId);

                if (user == null)
                {
                    return Result.Failure("UserId is not valid");
                }
                var exists = await _context.Controls.AnyAsync(x => x.SubscriberId == request.SubscriberId && x.Name.ToLower().Trim() == request.Name.ToLower().Trim());

                if (exists)
                {
                    return Result.Failure($"Control name already exists!");
                }

                var entity = new Control
                {
                    Name = request.Name,
                    Description = request.Description,
                    SubscriberId = request.SubscriberId,
                    SubscriberName = _authService.Subscriber?.SubscriberName,
                    InputValueType = request.InputValueType,
                    InputValueTypeDesc = request.InputValueType.ToString(),
                    ControlType = request.ControlType,
                    ControlTypeDesc = request.ControlType.ToString(),

                    DisplayName = request.DisplayName,
                    ControlTips = request.ControlTips,
                    VersionNumber = request.VersionNumber,

                    UserId = request.UserId,
                    CreatedByEmail = user.Entity.Email,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

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

                await _context.BeginTransactionAsync();
                await _context.Controls.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                if (request.ControlProperties != null && request.ControlProperties.Count > 0)
                {
                    var command = new UpdateControlPropertiesCommand
                    {
                        SubscriberId = request.SubscriberId,
                        SubscriberName = _authService.Subscriber?.Name,
                        ControlId = entity.Id,
                        ControlProperties = request.ControlProperties,
                        UserId = request.UserId,
                        AccessToken = request.AccessToken
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
                return Result.Success("Control request created successfully!", result);
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure($"Control request creation failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

    }
}
