using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Domain.Enums;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Application.PGPlans.Queries;
using FluentValidation.Results;

namespace OnyxDoc.SubscriptionService.Application.PGPlans.Commands
{
    public class UpdatePGPlansCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public List<UpdatePGPlanRequest> PGPlans { get; set; }
        public string UserId { get; set; }
    }

    public class UpdatePGPlansCommandHandler : IRequestHandler<UpdatePGPlansCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;


        public UpdatePGPlansCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(UpdatePGPlansCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                var list = new List<PGPlan>();
                await _context.BeginTransactionAsync();



                foreach (var item in request.PGPlans)
                {
                    this.ValidateItem(item);
                    var subscriptionPlan = await _context.PGPlans
                          .AnyAsync(x => x.SubscriberId == request.SubscriberId && x.Id == item.SubscriptionId);

                    //check if the name of the subscription type already exists and conflicts with this new name 
                    var entity = await _context.PGPlans
                        .Where(x => x.SubscriberId == request.SubscriberId && x.SubscriptionId == item.SubscriptionId
                        && (x.Id == item.Id || (x.PaymentGatewayPlanCode == item.PaymentGatewayPlanCode && x.PaymentGateway == item.PaymentGateway)))
                        .FirstOrDefaultAsync();

                    if (entity == null || item.Id <= 0)
                    {
                        entity = new PGPlan
                        {
                            Name = item.PaymentGateway + "-" + item.PaymentGateway.ToString(),
                            SubscriberId = request.SubscriberId,
                            SubscriberName = _authService.Subscriber?.Name,
                            SubscriptionId = item.SubscriptionId,
                            PaymentGateway = item.PaymentGateway,
                            PaymentGatewayDesc = item.PaymentGateway.ToString(),
                            PaymentGatewayPlanId = item.PaymentGatewayPlanId,
                            PaymentGatewayPlanCode = item.PaymentGatewayPlanCode,

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
                        entity.SubscriptionId = item.SubscriptionId;
                        entity.PaymentGatewayPlanId = item.PaymentGatewayPlanId;
                        entity.PaymentGatewayPlanCode = item.PaymentGatewayPlanCode;
                        entity.PaymentGateway = item.PaymentGateway;
                        entity.PaymentGatewayDesc = item.PaymentGateway.ToString();

                        entity.Status = item.Status;
                        entity.StatusDesc = item.Status.ToString();

                        entity.LastModifiedBy = request.UserId;
                        entity.LastModifiedDate = DateTime.Now;
                    }
                    list.Add(entity);
                }

                _context.PGPlans.UpdateRange(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<PGPlanDto>>(list);
                return Result.Success("Payment gateway plan update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Payment gateway plan update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

        private void ValidateItem(UpdatePGPlanRequest item)
        {
            UpdatePGPlanRequestValidator validator = new UpdatePGPlanRequestValidator();

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
