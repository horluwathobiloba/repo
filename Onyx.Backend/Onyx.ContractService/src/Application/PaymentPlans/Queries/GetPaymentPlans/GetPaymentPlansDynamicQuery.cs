using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ReventInject;

namespace Onyx.ContractService.Application.PaymentPlans.Queries.GetPaymentPlans
{
    public class GetPaymentPlansDynamicQuery : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string SearchText { get; set; }
    }


    public class GetPaymentPlansDynamicQueryHandler : IRequestHandler<GetPaymentPlansDynamicQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetPaymentPlansDynamicQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetPaymentPlansDynamicQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                if (request.OrganisationId <= 0)
                {
                    return Result.Failure($"Organisation must be specified.");
                }
                if (string.IsNullOrEmpty(request.SearchText))
                {
                    return Result.Failure($"Search text must be specified.");
                }

                var list = await _context.PaymentPlans.Where(a => a.OrganisationId == request.OrganisationId).ToListAsync();
                list = list.Where(a => request.SearchText.IsIn(a.Name)).ToList();

                if (list == null)
                {
                    throw new NotFoundException(nameof(PaymentPlan));
                }

                var result = _mapper.Map<List<PaymentPlanDto>>(list);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving payment plans. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }

}
