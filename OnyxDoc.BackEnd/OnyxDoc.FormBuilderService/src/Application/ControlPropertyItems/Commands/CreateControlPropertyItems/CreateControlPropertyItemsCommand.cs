using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.ControlPropertyItems.Queries;
using OnyxDoc.FormBuilderService.Domain.Entities;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.ControlPropertyItems.Commands
{
    public class CreateControlPropertyItemsCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int ControlPropertyId { get; set; }
        public List<CreateControlPropertyItemRequest> ControlPropertyItems { get; set; }
        public string UserId { get; set; }
    }

    public class CreateControlPropertyItemsCommandHandler : IRequestHandler<CreateControlPropertyItemsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreateControlPropertyItemsCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(CreateControlPropertyItemsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var list = new List<ControlPropertyItem>();

                await _context.BeginTransactionAsync();

                foreach (var item in request.ControlPropertyItems)
                {
                    this.ValidateItem(item);
                    var exists = await _context.ControlPropertyItems.AnyAsync(a => a.SubscriberId == request.SubscriberId
                && a.ControlPropertyId == request.ControlPropertyId && a.Value == item.Value);

                    if (exists)
                    {
                        return Result.Failure($"Control property item for '{item.Value}' already exists.");
                    }
                    var entity = new ControlPropertyItem
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
                    list.Add(entity);
                }
                await _context.ControlPropertyItems.AddRangeAsync(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<ControlPropertyItemDto>>(list);
                return Result.Success("Control property items created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Control property items creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }

        private void ValidateItem(CreateControlPropertyItemRequest item)
        {
            CreateControlPropertyItemRequestValidator validator = new CreateControlPropertyItemRequestValidator();

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
