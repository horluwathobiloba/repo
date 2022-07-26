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
using OnyxDoc.FormBuilderService.Application.PageControlItemProperties.Queries;
using FluentValidation.Results;
using OnyxDoc.FormBuilderService.Application.PageControlItemPropertyValues.Commands;
using OnyxDoc.FormBuilderService.Application.PageControlItemPropertyValues;

namespace OnyxDoc.FormBuilderService.Application.PageControlItemProperties.Commands
{
    public class UpdatePageControlItemPropertiesCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int PageControlItemId { get; set; }
        public List<UpdatePageControlItemPropertyRequest> PageControlItemProperties { get; set; }
        public string UserId { get; set; }
    }

    public class UpdatePageControlItemPropertiesCommandHandler : IRequestHandler<UpdatePageControlItemPropertiesCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;


        public UpdatePageControlItemPropertiesCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(UpdatePageControlItemPropertiesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                var list = new List<PageControlItemProperty>();
                await _context.BeginTransactionAsync();

                var pageControlItem = await _context.PageControlItems
                           .AnyAsync(x => x.SubscriberId == request.SubscriberId && x.Id == request.PageControlItemId);

                foreach (var item in request.PageControlItemProperties)
                {
                    this.ValidateItem(item);

                    var entity = item.Id > 0 ? null : await _context.PageControlItemProperties
                        .Where(x => x.SubscriberId == request.SubscriberId && x.PageControlItemId == request.PageControlItemId && x.Id == item.Id)
                        .FirstOrDefaultAsync();

                    if (entity == null || item.Id <= 0)
                    {
                        entity = new PageControlItemProperty
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

                    }
                    else
                    {
                        entity.PageControlItemId = request.PageControlItemId;
                        entity.ControlPropertyId = item.ControlPropertyId;

                        entity.Status = item.Status;
                        entity.StatusDesc = item.Status.ToString();
                        entity.LastModifiedBy = request.UserId;
                        entity.LastModifiedDate = DateTime.Now;
                    }
                    await _context.SaveChangesAsync(cancellationToken);

                    //create the logical name for the property and update the entity
                    entity.Name = $"{entity.PageControlItem?.Name}_{entity.ControlProperty?.Name}_{entity.Id}";
                    entity.Description = $"{entity.PageControlItem?.Name} {entity.ControlProperty?.Name} {entity.Id}";

                    if (item.PageControlItemPropertyValues != null && item.PageControlItemPropertyValues.Count > 0)
                    {
                        var command = new UpdatePageControlItemPropertyValuesCommand
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
                        entity.PageControlItemPropertyValues = pciPropertyValues;
                        #region Do not delete!
                        //    var handler = new UpdatePageControlItemPropertyValuesCommandHandler(_context, _mapper, _authService);
                        //    var pciPropertyValues = await handler.Handle(command, cancellationToken);

                        //    if (pciPropertyValues.Succeeded == false)
                        //    {
                        //        throw new Exception(pciPropertyValues.Error + pciPropertyValues.Message);
                        //    }
                        //    
                        #endregion
                    }
                    list.Add(entity);
                }

                _context.PageControlItemProperties.UpdateRange(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
 
                var result = _mapper.Map<List<PageControlItemPropertyDto>>(list);
                return Result.Success("Page control item properties update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Page control item property update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

        private void ValidateItem(UpdatePageControlItemPropertyRequest item)
        {
            UpdatePageControlItemPropertyRequestValidator validator = new UpdatePageControlItemPropertyRequestValidator();

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
