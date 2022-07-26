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
using OnyxDoc.FormBuilderService.Application.ControlPropertyItems.Queries;
using FluentValidation.Results;

namespace OnyxDoc.FormBuilderService.Application.ControlPropertyItems.Commands
{
    public class UpdateControlPropertyItemsCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int ControlPropertyId { get; set; }
        public List<UpdateControlPropertyItemRequest> ControlPropertyItems { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateControlPropertyItemsCommandHandler : IRequestHandler<UpdateControlPropertyItemsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;


        public UpdateControlPropertyItemsCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(UpdateControlPropertyItemsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                var list = new List<ControlPropertyItem>();
                await _context.BeginTransactionAsync();

                var ControlProperty = await _context.ControlProperties
                           .AnyAsync(x => x.SubscriberId == request.SubscriberId && x.Id == request.ControlPropertyId);

                foreach (var item in request.ControlPropertyItems)
                {
                    this.ValidateItem(item);

                    //check if the name of the subscription type already exists and conflicts with this new name 
                    var UpdatedEntityExists = item.Id > 0 && await _context.ControlPropertyItems
                           .AnyAsync(x => x.SubscriberId == request.SubscriberId && x.ControlPropertyId == request.ControlPropertyId
                           && (x.Value == item.Value || x.Index == x.Index));

                    if (UpdatedEntityExists)
                    {
                        continue;
                        //return Result.Failure($"The Control code '{item.ControlCode}' is already configured for this subscription.");
                    }

                    var entity = item.Id == 0 ? default : await _context.ControlPropertyItems
                        .Where(x => x.SubscriberId == request.SubscriberId && x.ControlPropertyId == request.ControlPropertyId
                        && (x.Id == item.Id || x.Index == item.Index || x.Value == item.Value))
                        .FirstOrDefaultAsync();

                    if (entity == null || item.Id <= 0)
                    {
                        entity = new ControlPropertyItem
                        {
                            Name = item.Value,
                            Description = item.Description,
                            SubscriberId = item.SubscriberId,
                            SubscriberName = _authService.Subscriber?.Name,
                            ControlPropertyId = item.ControlPropertyId,
                            Index = item.Index,
                            Value = item.Value,
                            IsDefaultValue = item.IsDefaultValue,

                            UserId = request.UserId,
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
                        entity.Description = item.Description;
                        entity.SubscriberName = _authService.Subscriber?.Name;
                        entity.ControlPropertyId = item.ControlPropertyId;
                        entity.Index = item.Index;
                        entity.Value = item.Value;
                        entity.IsDefaultValue = item.IsDefaultValue;

                        entity.Status = item.Status;
                        entity.StatusDesc = item.Status.ToString();

                        entity.LastModifiedBy = request.UserId;
                        entity.LastModifiedDate = DateTime.Now;
                    }
                    list.Add(entity);
                }

                _context.ControlPropertyItems.UpdateRange(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<ControlPropertyItemDto>>(list);
                return Result.Success("Control property items update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Control property items update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

        private void ValidateItem(UpdateControlPropertyItemRequest item)
        {
            UpdateControlPropertyItemRequestValidator validator = new UpdateControlPropertyItemRequestValidator();

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
