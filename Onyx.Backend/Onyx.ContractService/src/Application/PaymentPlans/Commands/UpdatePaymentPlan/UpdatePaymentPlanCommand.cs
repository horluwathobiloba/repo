using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.PaymentPlans.Queries.GetPaymentPlans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.PaymentPlans.Commands.UpdatePaymentPlan
{
    public class UpdatePaymentPlanCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OrganisationId { get; set; }
        public string UserId { get; set; }
    }

    public class UpdatePaymentPlanCommandHandler : IRequestHandler<UpdatePaymentPlanCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UpdatePaymentPlanCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(UpdatePaymentPlanCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);

                var entity = await _context.PaymentPlans.FirstOrDefaultAsync(x => x.OrganisationId == request.OrganisationId && x.Id == request.Id);
                if (entity == null)
                {
                    return Result.Failure($"Invalid vendor type specified.");
                }

                //check if the name of the service types are
                var UpdatedEntityExists = await _context.PaymentPlans.AnyAsync(x => x.OrganisationId == request.OrganisationId && x.Id != request.Id
                && x.Name.ToLower() == request.Name.ToLower());

                if (UpdatedEntityExists)
                {
                    return Result.Failure($"A record with this product service type name {request.Name} already exists. Please change the name.");
                }

                entity.Name = request.Name;
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                _context.PaymentPlans.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<PaymentPlanDto>(entity);
                return Result.Success("Vendor type updated was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"PaymentPlan update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }


    }


}
