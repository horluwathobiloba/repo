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
using OnyxDoc.SubscriptionService.Application.PGCustomers.Queries;
using FluentValidation.Results;

namespace OnyxDoc.SubscriptionService.Application.PGCustomers.Commands
{
    public class UpdatePGCustomersCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public List<UpdatePGCustomerRequest> PGCustomers { get; set; }
        public string UserId { get; set; }
    }

    public class UpdatePGCustomersCommandHandler : IRequestHandler<UpdatePGCustomersCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;


        public UpdatePGCustomersCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(UpdatePGCustomersCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                var list = new List<PGCustomer>();
                await _context.BeginTransactionAsync();

                var subscriptionPlan = await _context.PGCustomers
                           .AnyAsync(x => x.SubscriberId == request.SubscriberId);

                foreach (var item in request.PGCustomers)
                {
                    this.ValidateItem(item);

                    //check if the name of the record already exists and conflicts with this new name                                       
                    var entity = await _context.PGCustomers
                        .Where(x => x.SubscriberId == request.SubscriberId && (x.Id == item.Id || (x.PaymentGatewayCustomerCode == item.PaymentGatewayCustomerCode && x.PaymentGateway == item.PaymentGateway)))
                        .FirstOrDefaultAsync();

                    if (entity == null || item.Id <= 0)
                    {
                        entity = new PGCustomer
                        {
                            Name = item.PaymentGateway + "-" + item.PaymentGateway.ToString(),
                            SubscriberId = request.SubscriberId,
                            SubscriberName = _authService.Subscriber?.Name,
                            PaymentGateway = item.PaymentGateway,
                            PaymentGatewayDesc = item.PaymentGateway.ToString(),
                            PaymentGatewayCustomerId = item.PaymentGatewayCustomerId,
                            PaymentGatewayCustomerCode = item.PaymentGatewayCustomerCode,

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
                        entity.PaymentGatewayCustomerId = item.PaymentGatewayCustomerId;
                        entity.PaymentGatewayCustomerCode = item.PaymentGatewayCustomerCode;
                        entity.PaymentGateway = item.PaymentGateway;
                        entity.PaymentGatewayDesc = item.PaymentGateway.ToString();

                        entity.Status = item.Status;
                        entity.StatusDesc = item.Status.ToString();

                        entity.LastModifiedBy = request.UserId;
                        entity.LastModifiedDate = DateTime.Now;
                    }
                    list.Add(entity);
                }

                _context.PGCustomers.UpdateRange(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<PGCustomerDto>>(list);
                return Result.Success("Payment gateway customer update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Payment gateway customer update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

        private void ValidateItem(UpdatePGCustomerRequest item)
        {
            UpdatePGCustomerRequestValidator validator = new UpdatePGCustomerRequestValidator();

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
