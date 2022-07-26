using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.PageControlItemPropertyValues.Queries;
using OnyxDoc.FormBuilderService.Domain.Entities;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.PageControlItemPropertyValues.Commands
{
    public class CreatePageControlItemPropertyValuesCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int PageControlItemPropertyId { get; set; }
        public List<CreatePageControlItemPropertyValueRequest> PageControlItemPropertyValues { get; set; }
        public string UserId { get; set; }
    }

    public class CreatePageControlItemPropertyValuesCommandHandler : IRequestHandler<CreatePageControlItemPropertyValuesCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreatePageControlItemPropertyValuesCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(CreatePageControlItemPropertyValuesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.PageControlItemPropertyValues == null || request.PageControlItemPropertyValues.Count <= 0)
                {
                    return Result.Failure("No record found!");
                }

                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var list = new List<UpdatePageControlItemPropertyValueRequest>();
                var result = new Result();

                foreach (var item in request.PageControlItemPropertyValues)
                {
                    this.ValidateItem(item);

                    var entity = new UpdatePageControlItemPropertyValueRequest
                    {
                        SubscriberId = request.SubscriberId,
                        Value = item.Value,
                    };
                    list.Add(entity);
                }

                if (list.Count <= 0)
                {
                    return Result.Failure("No record found!");
                }
                var command = new UpdatePageControlItemPropertyValuesCommand
                {
                    SubscriberId = request.SubscriberId,
                    PageControlItemPropertyId = request.PageControlItemPropertyId,
                    PageControlItemPropertyValues = list,
                    UserId = request.UserId,
                    AccessToken = request.AccessToken
                };

                var handler = new UpdatePageControlItemPropertyValuesCommandHandler(_context, _mapper, _authService);
                result = await handler.Handle(command, cancellationToken);

                if (result.Succeeded == false)
                {
                    throw new Exception(result.Error + result.Message);
                }
                return Result.Success("Page control item property values created successfully!", result); 
            }
            catch (Exception ex)
            {
                return Result.Failure($"Page control item property values creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }

        private void ValidateItem(CreatePageControlItemPropertyValueRequest item)
        {
            CreatePageControlItemPropertyValueRequestValidator validator = new CreatePageControlItemPropertyValueRequestValidator();

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
