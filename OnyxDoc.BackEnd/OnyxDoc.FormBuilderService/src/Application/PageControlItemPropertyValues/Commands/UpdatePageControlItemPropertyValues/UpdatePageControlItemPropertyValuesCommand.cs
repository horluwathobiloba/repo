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
using OnyxDoc.FormBuilderService.Application.PageControlItemPropertyValues.Queries;
using FluentValidation.Results;

namespace OnyxDoc.FormBuilderService.Application.PageControlItemPropertyValues.Commands
{
    public class UpdatePageControlItemPropertyValuesCommand : AuthToken, IRequest<Result>
    {
        public int PageControlItemPropertyId { get; set; }
        public List<UpdatePageControlItemPropertyValueRequest> PageControlItemPropertyValues { get; set; }
        public int SubscriberId { get; set; } 
        public string UserId { get; set; }
    }

    public class UpdatePageControlItemPropertyValuesCommandHandler : IRequestHandler<UpdatePageControlItemPropertyValuesCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;


        public UpdatePageControlItemPropertyValuesCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(UpdatePageControlItemPropertyValuesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                var list = new List<PageControlItemPropertyValue>();
                await _context.BeginTransactionAsync();

                var pciProperty = await _context.PageControlItemProperties.FirstOrDefaultAsync(x => x.Id == request.PageControlItemPropertyId);

                if (pciProperty == null || pciProperty.Id <= 0)
                {
                    return Result.Failure("Invalid currency specified");
                }

                foreach (var item in request.PageControlItemPropertyValues)
                {
                    var entity = await _context.PageControlItemPropertyValues.Where(x => x.SubscriberId == request.SubscriberId
                     && x.PageControlItemPropertyId == request.PageControlItemPropertyId && x.Id == item.Id).FirstOrDefaultAsync();

                    if (entity == null)
                    {
                        entity = new PageControlItemPropertyValue
                        {
                            SubscriberId = request.SubscriberId, 
                            SubscriberName = _authService.Subscriber?.Name,
                            Name = item.Value,
                            Description = item.Value,
                            PageControlItemPropertyId = request.PageControlItemPropertyId,
                            Value = item.Value,

                            CreatedByEmail = _authService.User?.Email,
                            CreatedBy = request.UserId,
                            CreatedDate = DateTime.Now,
                            LastModifiedBy = request.UserId,
                            LastModifiedDate = DateTime.Now,
                            Status = Status.Active,
                            StatusDesc = Status.Active.ToString()
                        };
                    }
                    else
                    {
                        entity.Name = item.Value;
                        entity.Description = item.Value;
                        entity.Value = item.Value;

                        entity.LastModifiedBy = request.UserId;
                        entity.LastModifiedDate = DateTime.Now;
                    }
                    list.Add(entity);
                }

                _context.PageControlItemPropertyValues.UpdateRange(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<PageControlItemPropertyValueDto>>(list);
                return Result.Success("Page control item property values update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Page control item property value update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

        private void ValidateItem(UpdatePageControlItemPropertyValueRequest item)
        {
            UpdatePageControlItemPropertyValueRequestValidator validator = new UpdatePageControlItemPropertyValueRequestValidator();

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
