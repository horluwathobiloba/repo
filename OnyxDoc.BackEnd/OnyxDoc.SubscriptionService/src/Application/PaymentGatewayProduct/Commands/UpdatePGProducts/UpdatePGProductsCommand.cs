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
using OnyxDoc.SubscriptionService.Application.PGProducts.Queries;
using FluentValidation.Results;

namespace OnyxDoc.SubscriptionService.Application.PGProducts.Commands
{
    public class UpdatePGProductsCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int SubscriptionPlanId { get; set; }
        public List<UpdatePGProductRequest> PGProducts { get; set; }
        public string UserId { get; set; }
    }

    public class UpdatePGProductsCommandHandler : IRequestHandler<UpdatePGProductsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;


        public UpdatePGProductsCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(UpdatePGProductsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                var list = new List<PGProduct>();
                await _context.BeginTransactionAsync();

                var subscriptionPlan = await _context.PGProducts
                           .AnyAsync(x => x.SubscriberId == request.SubscriberId && x.Id == request.SubscriptionPlanId);

                foreach (var item in request.PGProducts)
                {
                    this.ValidateItem(item);

                    //check if the name of the subscription type already exists and conflicts with this new name 
                    var entity = await _context.PGProducts
                        .Where(x => x.SubscriberId == request.SubscriberId && x.SubscriptionPlanId == request.SubscriptionPlanId
                        && (x.Id == item.Id || (x.PaymentGatewayProductCode == item.PaymentGatewayProductCode && x.PaymentGateway == item.PaymentGateway)))
                        .FirstOrDefaultAsync();

                    if (entity == null || item.Id <= 0)
                    {
                        entity = new PGProduct
                        {
                            Name = item.PaymentGateway + "-" + item.PaymentGateway.ToString(),
                            SubscriberId = request.SubscriberId,
                            SubscriberName = _authService.Subscriber?.Name,
                            SubscriptionPlanId = request.SubscriptionPlanId,
                            PaymentGateway = item.PaymentGateway,
                            PaymentGatewayDesc = item.PaymentGateway.ToString(),
                            PaymentGatewayProductId = item.PaymentGatewayProductId,
                            PaymentGatewayProductCode = item.PaymentGatewayProductCode,

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
                        entity.SubscriberName = _authService.Subscriber?.Name;
                        entity.SubscriptionPlanId = item.SubscriptionPlanId;
                        entity.PaymentGatewayProductId = item.PaymentGatewayProductId;
                        entity.PaymentGatewayProductCode = item.PaymentGatewayProductCode;
                        entity.PaymentGateway = item.PaymentGateway;
                        entity.PaymentGatewayDesc = item.PaymentGateway.ToString();

                        entity.Status = item.Status;
                        entity.StatusDesc = item.Status.ToString();

                        entity.LastModifiedBy = request.UserId;
                        entity.LastModifiedDate = DateTime.Now;
                    }
                    list.Add(entity);
                }

                _context.PGProducts.UpdateRange(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<PGProductDto>>(list);
                return Result.Success("Payment gateway product update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Payment gateway product update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

        private void ValidateItem(UpdatePGProductRequest item)
        {
            UpdatePGProductRequestValidator validator = new UpdatePGProductRequestValidator();

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
