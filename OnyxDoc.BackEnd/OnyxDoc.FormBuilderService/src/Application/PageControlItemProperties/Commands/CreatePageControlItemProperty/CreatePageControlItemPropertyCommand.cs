using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.PageControlItemProperties.Queries;
using OnyxDoc.FormBuilderService.Application.PageControlItemPropertyValues;
using OnyxDoc.FormBuilderService.Application.PageControlItemPropertyValues.Commands;
using OnyxDoc.FormBuilderService.Domain.Entities;
using OnyxDoc.FormBuilderService.Domain.Enums;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.PageControlItemProperties.Commands
{
    public class CreatePageControlItemPropertyCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int PageControlItemId { get; set; }
        public int ControlPropertyId { get; set; }
        public List<CreatePageControlItemPropertyValueRequest> PageControlItemPropertyValues { get; set; }
        public string UserId { get; set; }
    }

    public class CreateSubscriptionCurrencyCommandHandler : IRequestHandler<CreatePageControlItemPropertyCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreateSubscriptionCurrencyCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _authService = authService;
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreatePageControlItemPropertyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var exists = await _context.PageControlItemProperties.AnyAsync(a => a.SubscriberId == request.SubscriberId
                && a.PageControlItemId == request.PageControlItemId && a.ControlPropertyId == request.ControlPropertyId);

                if (exists)
                {
                    return Result.Failure($"Page control item property already exists. You can only update this record.");
                }

                PageControlItem pageControlItem = await _context.PageControlItems.FirstOrDefaultAsync(a => a.SubscriberId == request.SubscriberId && a.Id == request.PageControlItemId);
                ControlProperty controlProperty = await _context.ControlProperties.FirstOrDefaultAsync(a => a.SubscriberId == request.SubscriberId && a.Id == request.ControlPropertyId);

                if (pageControlItem == null)
                {
                    return Result.Failure($"Page control item specified is invalid.");
                }
                if (controlProperty == null)
                {
                    return Result.Failure($"Control property specified is invalid.");
                }

                var entity = new PageControlItemProperty
                {
                    SubscriberId = request.SubscriberId,
                    SubscriberName = _authService.Subscriber?.Name,
                    PageControlItemId = request.PageControlItemId,
                    ControlPropertyId = request.ControlPropertyId,

                    UserId = request.UserId,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.BeginTransactionAsync();
                await _context.PageControlItemProperties.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                //create the logical name for the property and update the entity
                entity.Name = $"{pageControlItem.Name}_{controlProperty.Name}_{entity.Id}";
                entity.Description = $"{pageControlItem.Name} {controlProperty.Name} {entity.Id}";

                if (request.PageControlItemPropertyValues != null && request.PageControlItemPropertyValues.Count > 0)
                {
                    var command = new CreatePageControlItemPropertyValuesCommand
                    {
                        SubscriberId = request.SubscriberId,
                        PageControlItemPropertyId = entity.Id,
                        PageControlItemPropertyValues = request.PageControlItemPropertyValues,
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
                             
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<PageControlItemPropertyDto>(entity);
                return Result.Success("Page control item property created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Page control item property creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
