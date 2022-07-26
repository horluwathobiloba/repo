using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.ControlProperties.Queries;
using OnyxDoc.FormBuilderService.Application.ControlPropertyItems.Commands;
using OnyxDoc.FormBuilderService.Application.PageControlItemProperties;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.ControlProperties.Commands
{
    public class UpdateControlPropertyCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int ControlId { get; set; }
        public int ParentPropertyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public string PropertyTips { get; set; }
        public int Index { get; set; }
        public ControlPropertyType ControlPropertyType { get; set; }
        public string ControlPropertyTypeDesc { get; set; }
        public ControlPropertyValueType ControlPropertyValueType { get; set; }
        public string ControlPropertyValueTypeDesc { get; set; }
        public bool ShowInContextMenu { get; set; }
        public List<UpdateControlPropertyItemRequest> ControlPropertyItems { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateSubscriptionControlCommandHandler : IRequestHandler<UpdateControlPropertyCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UpdateSubscriptionControlCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdateControlPropertyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                //N.B: When checking for the index,  the value must be greater than zero
                var UpdatedEntityExists = await _context.ControlProperties
                       .AnyAsync(x => x.SubscriberId == request.SubscriberId && x.ControlId == request.ControlId && x.Id != request.Id
                       && (x.Name == request.Name || x.DisplayName == request.DisplayName || (x.Index > 0 && x.Index == request.Index)));


                if (UpdatedEntityExists)
                {
                    return Result.Failure($"The control property named '{request.Name}' or display name '{request.DisplayName}' is already configured for this control.");
                }

                var entity = await _context.ControlProperties.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId
                && x.ControlId == request.ControlId && x.Id == request.Id);

                if (entity == null)
                {
                    return Result.Failure($"Invalid control property specified.");
                }

                entity.Name = request.Name;
                entity.SubscriberName = _authService.Subscriber?.Name;
                entity.ControlId = request.ControlId;
                entity.Description = request.Description;
                entity.DisplayName = request.DisplayName;
                entity.PropertyTips = request.PropertyTips;
                entity.ControlPropertyType = request.ControlPropertyType;
                entity.ControlPropertyTypeDesc = request.ControlPropertyType.ToString();
                entity.ControlPropertyValueType = request.ControlPropertyValueType;
                entity.ControlPropertyValueTypeDesc = request.ControlPropertyValueType.ToString();
                entity.ParentPropertyId = request.ParentPropertyId;
                entity.ShowInContextMenu = request.ShowInContextMenu;

                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                await _context.BeginTransactionAsync();

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
                    entity.ControlPropertyItems = (controlPropertyItems);

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
                return Result.Success("Control property update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Control property update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

    }


}
