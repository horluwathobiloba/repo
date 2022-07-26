using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.ControlProperties.Queries;
using OnyxDoc.FormBuilderService.Application.ControlPropertyItems.Commands;
using OnyxDoc.FormBuilderService.Application.PageControlItemProperties;
using OnyxDoc.FormBuilderService.Domain.Entities;
using OnyxDoc.FormBuilderService.Domain.Enums;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.ControlProperties.Commands
{
    public class CreateControlPropertyCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int ControlId { get; set; }
        public int ParentPropertyId { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public string PropertyTips { get; set; }
        public ControlPropertyType ControlPropertyType { get; set; }
        public string ControlPropertyTypeDesc { get; set; }
        public ControlPropertyValueType ControlPropertyValueType { get; set; }
        public string ControlPropertyValueTypeDesc { get; set; }
        public bool ShowInContextMenu { get; set; }


        public string UserId { get; set; }

        public List<UpdateControlPropertyItemRequest> ControlPropertyItems { get; set; }
    }

    public class CreateSubscriptionControlCommandHandler : IRequestHandler<CreateControlPropertyCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreateSubscriptionControlCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _authService = authService;
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreateControlPropertyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var exists = await _context.ControlProperties.AnyAsync(a => a.SubscriberId == request.SubscriberId
                && a.ControlId == request.ControlId && a.ParentPropertyId == request.ParentPropertyId && (a.Name == request.Name || a.Index == request.Index));

                if (exists)
                {
                    return Result.Failure($"Control property with name '{request.Name}' or index {request.Index} already exists for the specified parent property.");
                }

                Control control = await _context.Controls.FirstOrDefaultAsync(a => a.Id == request.ControlId);

                if (control == null)
                {
                    throw new Exception("Control specified was not found!");
                }

                var entity = new ControlProperty
                {
                    Name = request.Name,
                    SubscriberId = request.SubscriberId,
                    SubscriberName = _authService.Subscriber?.Name,
                    ControlId = request.ControlId,
                    Description = request.Description,
                    DisplayName = request.DisplayName,
                    PropertyTips = request.PropertyTips,
                    Index = request.Index,
                    ControlPropertyType = request.ControlPropertyType,
                    ControlPropertyTypeDesc = request.ControlPropertyType.ToString(),
                    ControlPropertyValueType = request.ControlPropertyValueType,
                    ControlPropertyValueTypeDesc = request.ControlPropertyValueType.ToString(),
                    ParentPropertyId = request.ParentPropertyId,
                    ShowInContextMenu = request.ShowInContextMenu,

                    UserId = request.UserId,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.BeginTransactionAsync();
                await _context.ControlProperties.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                if (request.ControlPropertyItems != null && request.ControlPropertyItems.Count > 0)
                {
                    var command = new UpdateControlPropertyItemsCommand
                    {
                        SubscriberId = request.SubscriberId,
                        ControlPropertyId = entity.Id,
                        ControlPropertyItems = request.ControlPropertyItems,
                        UserId = request.UserId,
                        AccessToken = request.AccessToken
                    };
                    var helper = new ControlPropertyItemHelper(_context, _mapper, _authService);
                    var controlPropertyItems = await helper.GetControlPropertyItems(command, cancellationToken);
                    entity.ControlPropertyItems=controlPropertyItems;

                    #region Do not delete!
                    //    var handler = new UpdateControlPropertyItemsCommandHandler(_context, _mapper, _authService);
                    //    var controlPropertItemResult = await handler.Handle(command, cancellationToken);

                    //    if (controlPropertItemResult.Succeeded == false)
                    //    {
                    //        throw new Exception(controlPropertItemResult.Error + controlPropertItemResult.Message);
                    //    }
                    //    
                    #endregion
                } 

                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                var result = _mapper.Map<ControlPropertyDto>(entity);
                return Result.Success("Control property created successfully!", result);
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure($"Control property creation failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }

        }
    }
}
