using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.ControlProperties.Queries;
using OnyxDoc.FormBuilderService.Application.ControlPropertyItems.Commands;
using OnyxDoc.FormBuilderService.Application.PageControlItemProperties;
using OnyxDoc.FormBuilderService.Domain.Entities;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.ControlProperties.Commands
{
    public class CreateControlPropertiesCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int ControlId { get; set; }
        public List<CreateControlPropertyRequest> ControlProperties { get; set; }
        public string UserId { get; set; }
    }

    public class CreateControlPropertiesCommandHandler : IRequestHandler<CreateControlPropertiesCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreateControlPropertiesCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(CreateControlPropertiesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var list = new List<ControlProperty>();


                Control control = await _context.Controls.FirstOrDefaultAsync(a => a.SubscriberId == request.SubscriberId && a.Id == request.ControlId);

                if (control == null)
                {
                    throw new Exception("Control specified was not found!");
                }

                await _context.BeginTransactionAsync();

                foreach (var item in request.ControlProperties)
                {
                    this.ValidateItem(item);
                    var exists = await _context.ControlProperties.AnyAsync(a => a.SubscriberId == request.SubscriberId
                    && a.ControlId == request.ControlId && a.ParentPropertyId == item.ParentPropertyId && (a.Name == item.Name || a.Index == item.Index));

                    if (exists)
                    {
                        return Result.Failure($"Control property with name '{item.Name}' or index {item.Index} already exists for the specified parent property.");
                    }

                    var entity = new ControlProperty
                    {
                        Name = item.Name,
                        SubscriberId = item.SubscriberId,
                        SubscriberName = _authService.Subscriber?.Name,
                        ControlId = request.ControlId,
                        Description = item.Description,
                        DisplayName = item.DisplayName,
                        Index = item.Index,
                        ControlPropertyType = item.ControlPropertyType,
                        ControlPropertyTypeDesc = item.ControlPropertyType.ToString(),
                        ControlPropertyValueType = item.ControlPropertyValueType,
                        ControlPropertyValueTypeDesc = item.ControlPropertyValueType.ToString(),
                        ParentPropertyId = item.ParentPropertyId,
                        PropertyTips = item.PropertyTips,
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
                return Result.Success("Control properties created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Control properties creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }

        private void ValidateItem(CreateControlPropertyRequest item)
        {
            CreateControlPropertyRequestValidator validator = new CreateControlPropertyRequestValidator();

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
