using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.PGCustomers.Queries;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.PGCustomers.Commands
{
    public class CreatePGCustomersCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public List<CreatePGCustomerRequest> PGCustomers { get; set; }
        public string UserId { get; set; }
    }

    public class CreatePGCustomersCommandHandler : IRequestHandler<CreatePGCustomersCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreatePGCustomersCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(CreatePGCustomersCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var list = new List<PGCustomer>();

                await _context.BeginTransactionAsync();

                foreach (var item in request.PGCustomers)
                {
                    this.ValidateItem(item);
                    var exists = await _context.PGCustomers.AnyAsync(a => a.SubscriberId == request.SubscriberId && a.PaymentGatewayCustomerCode == item.PaymentGatewayCustomerCode && a.PaymentGateway == item.PaymentGateway);

                    if (exists)
                    {
                        //return Result.Failure($"Customer already exists for '{item.PaymentGateway}'.");
                        continue;
                    }
                    var entity = new PGCustomer
                    {
                        Name = request.SubscriberId + "_" + item.PaymentGateway,
                        SubscriberId = request.SubscriberId,
                        SubscriberName = _authService.Subscriber?.Name,
                        PaymentGatewayCustomerId = item.PaymentGatewayCustomerId,
                        PaymentGatewayCustomerCode = item.PaymentGatewayCustomerCode,
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
                await _context.PGCustomers.AddRangeAsync(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<PGCustomerDto>>(list);
                return Result.Success("Payment gateway customer created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Payment gateway customers creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }

        private void ValidateItem(CreatePGCustomerRequest item)
        {
            CreatePGCustomerRequestValidator validator = new CreatePGCustomerRequestValidator();

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
