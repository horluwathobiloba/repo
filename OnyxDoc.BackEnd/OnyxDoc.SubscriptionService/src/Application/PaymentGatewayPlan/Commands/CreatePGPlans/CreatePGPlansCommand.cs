using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.PGPlans.Queries;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.PGPlans.Commands
{
    public class CreatePGPlansCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public List<CreatePGPlanRequest> PGPlans { get; set; }
        public string UserId { get; set; }
    }

    public class CreatePGPlansCommandHandler : IRequestHandler<CreatePGPlansCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreatePGPlansCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(CreatePGPlansCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var list = new List<PGPlan>();

                await _context.BeginTransactionAsync();

                foreach (var item in request.PGPlans)
                {
                    this.ValidateItem(item);
                    var exists = await _context.PGPlans.AnyAsync(a => a.SubscriberId == request.SubscriberId
                && a.SubscriptionId == item.SubscriptionId && a.PaymentGatewayPlanCode == item.PaymentGatewayPlanCode && a.PaymentGateway == item.PaymentGateway);

                    if (exists)
                    {
                        return Result.Failure($"Plan already exists for '{item.PaymentGateway}'.");
                    }
                    var entity = new PGPlan
                    {
                        Name = item.SubscriptionId + "_" + item.PaymentGateway,
                        SubscriberId = request.SubscriberId,
                        SubscriberName = _authService.Subscriber?.Name,
                        SubscriptionId = item.SubscriptionId,
                        PaymentGatewayPlanId = item.PaymentGatewayPlanId,
                        PaymentGatewayPlanCode = item.PaymentGatewayPlanCode,
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
                await _context.PGPlans.AddRangeAsync(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<PGPlanDto>>(list);
                return Result.Success("Payment gateway plan created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Payment gateway plans creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }

        private void ValidateItem(CreatePGPlanRequest item)
        {
            CreatePGPlanRequestValidator validator = new CreatePGPlanRequestValidator();

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
