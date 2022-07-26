using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.PageControlItemProperties;
using OnyxDoc.FormBuilderService.Application.PageControlItemProperties.Commands;
using OnyxDoc.FormBuilderService.Application.PageControlItems.Queries;
using OnyxDoc.FormBuilderService.Domain.Entities;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.PageControlItems.Commands
{
    public class CreatePageControlItemsCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int DocumentPageId { get; set; }
        public List<CreatePageControlItemRequest> PageControlItems { get; set; }
        public string UserId { get; set; }
    }

    public class CreatePageControlItemsCommandHandler : IRequestHandler<CreatePageControlItemsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreatePageControlItemsCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(CreatePageControlItemsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var list = new List<PageControlItem>();

                await _context.BeginTransactionAsync();

                foreach (var item in request.PageControlItems)
                {
                    this.ValidateItem(item);

                    var entity = new PageControlItem
                    {
                        SubscriberId = request.SubscriberId,
                        SubscriberName = _authService.Subscriber?.Name,
                        DocumentPageId = request.DocumentPageId,
                        ControlId = item.ControlId,
                        Height = item.Height,
                        Width = item.Width,
                        Transform = item.Transform,
                        Position = item.Position,

                        UserId = request.UserId,
                        CreatedBy = request.UserId,
                        CreatedDate = DateTime.Now,
                        LastModifiedBy = request.UserId,
                        LastModifiedDate = DateTime.Now,
                        Status = Status.Active,
                        StatusDesc = Status.Active.ToString()
                    };

                    await _context.PageControlItems.AddAsync(entity);
                    await _context.SaveChangesAsync(cancellationToken);

                    if (item.PageControlItemProperties != null && item.PageControlItemProperties.Count > 0)
                    {
                        var command = new UpdatePageControlItemPropertiesCommand
                        {
                            SubscriberId = request.SubscriberId,
                            SubscriberName = _authService.Subscriber?.Name,
                            PageControlItemId = entity.Id,
                            PageControlItemProperties = item.PageControlItemProperties,
                            UserId = request.UserId,
                            AccessToken = request.AccessToken
                        };
                        var helper = new PageControlItemPropertyHelper(_context, _mapper, _authService);
                        var pciProperties = await helper.GetPageControlItemProperties(command, cancellationToken);
                        entity.PageControlItemProperties = pciProperties;
                        #region Do not delete!
                        //var handler = new UpdatePageControlItemPropertiesCommandHandler(_context, _mapper, _authService);
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
                _context.PageControlItems.UpdateRange(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<PageControlItemDto>>(list);
                return Result.Success("Page control items created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Page control items creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }

        private void ValidateItem(CreatePageControlItemRequest item)
        {
            CreatePageControlItemRequestValidator validator = new CreatePageControlItemRequestValidator();

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
