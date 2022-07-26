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
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.PaymentPlans.Commands.CreatePaymentPlans
{
    public class CreatePaymentPlansCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public List<string> PaymentPlanNames { get; set; }
        public string UserId { get; set; }
    }

    public class CreatePaymentPlansCommandHandler : IRequestHandler<CreatePaymentPlansCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreatePaymentPlansCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(CreatePaymentPlansCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var exists = (await _context.PaymentPlans.Where(x => x.OrganisationId == request.OrganisationId).ToListAsync())
                    .Any(x => request.PaymentPlanNames.IN(x.Name));

                if (exists)
                {
                    return Result.Failure($"One of the payment plan names already exists!");
                }

                var list = new List<PaymentPlan>();

                await _context.BeginTransactionAsync();

                foreach (var name in request.PaymentPlanNames)
                {
                    var entity = new PaymentPlan
                    {
                        Name = name,
                        OrganisationId = request.OrganisationId,
                        OrganisationName = _authService.Organisation?.Name,
                        CreatedBy = request.UserId,
                        CreatedDate = DateTime.Now,
                        LastModifiedBy = request.UserId,
                        LastModifiedDate = DateTime.Now,
                        Status = Status.Active,
                        StatusDesc = Status.Active.ToString()
                    };
                    list.Add(entity);
                }

                await _context.PaymentPlans.AddRangeAsync(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<PaymentPlanDto>>(list);
                return Result.Success("Payment plans created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Payment plans creation failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
