using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.PageControlItemProperties.Queries;
using OnyxDoc.FormBuilderService.Application.PageControlItemPropertyValues;
using OnyxDoc.FormBuilderService.Application.PageControlItemPropertyValues.Commands;
using OnyxDoc.FormBuilderService.Domain.Entities;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.PageControlItemProperties.Commands
{
    public class CreatePageControlItemPropertiesCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int PageControlItemId { get; set; }
        public List<CreatePageControlItemPropertyRequest> PageControlItemProperties { get; set; }
        public string UserId { get; set; }
    }

    public class CreatePageControlItemPropertiesCommandHandler : IRequestHandler<CreatePageControlItemPropertiesCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreatePageControlItemPropertiesCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(CreatePageControlItemPropertiesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var list = new List<PageControlItemProperty>();

                await _context.BeginTransactionAsync();

                PageControlItem pageControlItem = await _context.PageControlItems.FirstOrDefaultAsync(a => a.SubscriberId == request.SubscriberId && a.Id == request.PageControlItemId);

                if (pageControlItem == null)
                {
                    return Result.Failure($"Page control item specified is invalid.");
                }

                foreach (var item in request.PageControlItemProperties)
                {
                    this.ValidateItem(item);
                    var exists = await _context.PageControlItemProperties.AnyAsync(a => a.SubscriberId == request.SubscriberId
                    && a.PageControlItemId == request.PageControlItemId && a.ControlPropertyId == item.ControlPropertyId);

                    if (exists)
                    {
                        continue;
                    }

                    ControlProperty controlProperty = await _context.ControlProperties.FirstOrDefaultAsync(a => a.SubscriberId == request.SubscriberId
                    && a.Id == item.ControlPropertyId);

                    if (controlProperty == null)
                    {
                        return Result.Failure($"Control property specified is invalid.");
                    }

                    var entity = new PageControlItemProperty
                    {
                        SubscriberId = request.SubscriberId,
                        SubscriberName = _authService.Subscriber?.Name,
                        PageControlItemId = request.PageControlItemId,
                        ControlPropertyId = item.ControlPropertyId,

                        UserId = request.UserId,
                        CreatedBy = request.UserId,
                        CreatedDate = DateTime.Now,
                        LastModifiedBy = request.UserId,
                        LastModifiedDate = DateTime.Now,
                        Status = Status.Active,
                        StatusDesc = Status.Active.ToString()
                    };

                    await _context.PageControlItemProperties.AddAsync(entity);
                    await _context.SaveChangesAsync(cancellationToken);

                    //update the logical names
                    entity.Name = $"{pageControlItem.Name}_{entity.ControlProperty.Name}_{entity.Id}";
                    entity.Description = $"{pageControlItem.Name} {entity.ControlProperty.Name} {entity.Id}";

                    //create the logical name for the property and update the entity
                    entity.Name = $"{pageControlItem.Name}_{entity.ControlProperty.Name}_{entity.Id}";
                    entity.Description = $"{pageControlItem.Name} {entity.ControlProperty.Name} {entity.Id}";

                    if (item.PageControlItemPropertyValues != null && item.PageControlItemPropertyValues.Count > 0)
                    {
                        var command = new CreatePageControlItemPropertyValuesCommand
                        {
                            SubscriberId = request.SubscriberId,
                            PageControlItemPropertyId = entity.Id,
                            PageControlItemPropertyValues = item.PageControlItemPropertyValues,
                            UserId = request.UserId,
                            AccessToken = request.AccessToken
                        };
                        var helper = new PageControlItemPropertyValueHelper(_context, _mapper, _authService);
                        var pciPropertyValues = await helper.GetPageControlItemPropertyValues(command, cancellationToken);
                        //entity.PageControlItemPropertyValues.AddRange(pciPropertyValues);
                        entity.PageControlItemPropertyValues = (pciPropertyValues);

                        #region Do not delete!
                        //var handler = new CreatePageControlItemPropertyValuesCommandHandler(_context, _mapper, _authService);
                        //var subscriptionFeatureResult = await handler.Handle(command, cancellationToken);
                        ////  var recipientsResult = _mediator.Send(command).Result;
                        //if (subscriptionFeatureResult.Succeeded == false)
                        //{
                        //    throw new Exception(subscriptionFeatureResult.Error + subscriptionFeatureResult.Message);
                        //}
                        #endregion 
                    }
                    list.Add(entity);
                }
                _context.PageControlItemProperties.UpdateRange(list);
                await _context.SaveChangesAsync(cancellationToken);                 
                await _context.CommitTransactionAsync();


                var result = _mapper.Map<List<PageControlItemPropertyDto>>(list);
                return Result.Success("Page control item properties created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Page control item properties creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }

        private void ValidateItem(CreatePageControlItemPropertyRequest item)
        {
            CreatePageControlItemPropertyRequestValidator validator = new CreatePageControlItemPropertyRequestValidator();

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
