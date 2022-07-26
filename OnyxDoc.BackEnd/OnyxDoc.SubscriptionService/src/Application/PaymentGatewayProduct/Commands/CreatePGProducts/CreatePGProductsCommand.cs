using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.PGProducts.Queries;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.PGProducts.Commands
{
    public class CreatePGProductsCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int SubscriptionPlanId { get; set; }
        public List<CreatePGProductRequest> PGProducts { get; set; }
        public string UserId { get; set; }
    }

    public class CreatePGProductsCommandHandler : IRequestHandler<CreatePGProductsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreatePGProductsCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(CreatePGProductsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var list = new List<PGProduct>();

                await _context.BeginTransactionAsync();

                foreach (var item in request.PGProducts)
                {
                    this.ValidateItem(item);
                    var exists = await _context.PGProducts.AnyAsync(a => a.SubscriberId == request.SubscriberId
                && a.SubscriptionPlanId == request.SubscriptionPlanId && a.PaymentGatewayProductCode == item.PaymentGatewayProductCode && a.PaymentGateway == item.PaymentGateway);

                    if (exists)
                    {
                        return Result.Failure($"Product already exists for '{item.PaymentGateway}'.");                         
                    }
                    var entity = new PGProduct
                    {
                        Name = request.SubscriptionPlanId + "_" + item.PaymentGateway,
                        SubscriberId = request.SubscriberId,
                        SubscriberName = _authService.Subscriber?.Name,
                        SubscriptionPlanId = request.SubscriptionPlanId,
                        PaymentGatewayProductId = item.PaymentGatewayProductId,
                        PaymentGatewayProductCode = item.PaymentGatewayProductCode,
                        PaymentGateway = item.PaymentGateway,
                        PaymentGatewayDesc = item.PaymentGateway.ToString(),

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
                await _context.PGProducts.AddRangeAsync(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<PGProductDto>>(list);
                return Result.Success("Payment gateway product created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Payment gateway products creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }

        private void ValidateItem(CreatePGProductRequest item)
        {
            CreatePGProductRequestValidator validator = new CreatePGProductRequestValidator();

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
