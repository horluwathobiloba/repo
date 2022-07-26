using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Domain.Enums;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.FormBuilderService.Domain.Entities;
using OnyxDoc.FormBuilderService.Application.ControlProperties.Queries;
using FluentValidation.Results;
using OnyxDoc.FormBuilderService.Application.PageControlItemProperties;
using OnyxDoc.FormBuilderService.Application.ControlPropertyItems.Commands;

namespace OnyxDoc.FormBuilderService.Application.ControlProperties.Commands
{
    public class UpdateControlPropertiesCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int ControlId { get; set; }
        public List<UpdateControlPropertyRequest> ControlProperties { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateControlPropertiesCommandHandler : IRequestHandler<UpdateControlPropertiesCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;


        public UpdateControlPropertiesCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(UpdateControlPropertiesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                var list = new List<ControlProperty>();
                await _context.BeginTransactionAsync();

                var control = await _context.Controls.AnyAsync(x => x.SubscriberId == request.SubscriberId && x.Id == request.ControlId);

                foreach (var item in request.ControlProperties)
                {
                    this.ValidateItem(item);

                    //check if the name of the subscription type already exists and conflicts with this new name 
                    var UpdatedEntityExists = await _context.ControlProperties
                          .AnyAsync(x => x.SubscriberId == request.SubscriberId && x.ControlId == request.ControlId && x.Id != item.Id
                       && (x.Name == item.Name || x.DisplayName == item.DisplayName || (x.Index > 0 && x.Index == item.Index)));

                    if (UpdatedEntityExists)
                    {
                        continue;
                        // return Result.Failure($"The control property named '{request.Name}' or display name '{request.DisplayName}' is already configured for this control.");
                    }

                    var entity = await _context.ControlProperties
                        .Where(x => x.SubscriberId == request.SubscriberId && x.ControlId == request.ControlId
                        && (x.Id == item.Id || x.Name == item.Name || x.DisplayName == item.DisplayName || x.Index == item.Index))
                        .FirstOrDefaultAsync();

                    var index = 0;
                    if (entity == null || item.Id <= 0)
                    {
                        var indexExists = await _context.ControlProperties
                          .FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId && x.ControlId == request.ControlId && x.Index == item.Index);

                        //Auto Incremement Index: Get the last index if the requested index already exists
                        if (indexExists != null)
                        {
                            var maxIndex = await _context.ControlProperties
                           .Where(x => x.SubscriberId == request.SubscriberId && x.ControlId == request.ControlId)
                           .MaxAsync(r => r.Index);
                            index = maxIndex + 1;
                        }
                        else
                        {
                            index++;
                        }

                        entity = new ControlProperty
                        {
                            Name = item.Name,
                            SubscriberId = item.SubscriberId,
                            SubscriberName = _authService.Subscriber?.Name,
                            ControlId = request.ControlId,
                            Description = item.Description,
                            DisplayName = item.DisplayName,
                            PropertyTips = item.PropertyTips,
                            Index = item.Index,
                            ControlPropertyType = item.ControlPropertyType,
                            ControlPropertyTypeDesc = item.ControlPropertyType.ToString(),
                            ControlPropertyValueType = item.ControlPropertyValueType,
                            ControlPropertyValueTypeDesc = item.ControlPropertyValueType.ToString(),
                            ParentPropertyId = item.ParentPropertyId,
                            ShowInContextMenu = item.ShowInContextMenu,

                            UserId = request.UserId,
                            CreatedBy = request.UserId,
                            CreatedDate = DateTime.Now,
                            LastModifiedBy = request.UserId,
                            LastModifiedDate = DateTime.Now,
                            Status = Status.Active,
                            StatusDesc = Status.Active.ToString()
                        };
                        await _context.ControlProperties.AddAsync(entity);
                    }
                    else
                    {
                        entity.Name = item.Name;
                        entity.SubscriberName = _authService.Subscriber?.Name;
                        entity.ControlId = request.ControlId;
                        entity.Description = item.Description;
                        entity.DisplayName = item.DisplayName;
                        entity.PropertyTips = item.PropertyTips;
                        entity.ControlPropertyType = item.ControlPropertyType;
                        entity.ControlPropertyTypeDesc = item.ControlPropertyType.ToString();
                        entity.ControlPropertyValueType = item.ControlPropertyValueType;
                        entity.ControlPropertyValueTypeDesc = item.ControlPropertyValueType.ToString();
                        entity.ParentPropertyId = item.ParentPropertyId;
                        entity.ShowInContextMenu = item.ShowInContextMenu;

                        entity.Status = item.Status;
                        entity.StatusDesc = item.Status.ToString();
                        entity.LastModifiedBy = request.UserId;
                        entity.LastModifiedDate = DateTime.Now;
                        _context.ControlProperties.Update(entity);
                    }
                    await _context.SaveChangesAsync(cancellationToken);

                    if (item.ControlPropertyItems != null && item.ControlPropertyItems.Count > 0)
                    {
                        var command = new UpdateControlPropertyItemsCommand
                        {
                            SubscriberId = request.SubscriberId,
                            ControlPropertyId = entity.Id,
                            ControlPropertyItems = item.ControlPropertyItems,
                            UserId = request.UserId,
                            AccessToken = request.AccessToken
                        };
                        var helper = new ControlPropertyItemHelper(_context, _mapper, _authService);
                        var controlPropertyItems = await helper.GetControlPropertyItems(command, cancellationToken);
                        entity.ControlPropertyItems = controlPropertyItems;

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

                    list.Add(entity);
                }

                _context.ControlProperties.UpdateRange(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<ControlPropertyDto>>(list);
                return Result.Success("Control properties update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Control properties update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

        private void ValidateItem(UpdateControlPropertyRequest item)
        {
            UpdateControlPropertyRequestValidator validator = new UpdateControlPropertyRequestValidator();

            ValidationResult validateResult = validator.Validate(item);
            string validateError = null;

            if (!validateResult.IsValid)
            {
                foreach (var failure in validateResult.Errors)
                {
                    validateError += "Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage + "\n";
                }
                throw new Exception(validateError);
            }
        }


    }


}
