using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.PaymentPlans.Queries.GetPaymentPlans;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.PaymentPlans.Commands.CreatePaymentPlan
{
    public class CreatePaymentPlanCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
    }

    public class CreatePaymentPlanCommandHandler : IRequestHandler<CreatePaymentPlanCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreatePaymentPlanCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(CreatePaymentPlanCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var exists = await _context.PaymentPlans.AnyAsync(x => x.OrganisationId == request.OrganisationId && x.Name.ToLower() == request.Name.ToLower());

                if (exists)
                {
                    return Result.Failure($"Payment plan name already exists!");
                }

                var entity = new Domain.Entities.PaymentPlan
                {
                    Name = request.Name,
                    OrganisationId = request.OrganisationId,
                    OrganisationName = _authService.Organisation?.Name,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.PaymentPlans.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<PaymentPlanDto>(entity);
                return Result.Success("PaymentPlan created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"PaymentPlan creation failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
