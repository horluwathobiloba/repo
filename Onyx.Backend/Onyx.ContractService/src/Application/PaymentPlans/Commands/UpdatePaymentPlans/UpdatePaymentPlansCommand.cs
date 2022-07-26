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

namespace Onyx.ContractService.Application.PaymentPlans.Commands.UpdatePaymentPlans
{
    public class UpdatePaymentPlansCommand : AuthToken, IRequest<Result>
    {
        public List<UpdatePaymentPlanRequest> PaymentPlans { get; set; }
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public string UserId { get; set; }
    }

    public class UpdatePaymentPlansCommandHandler : IRequestHandler<UpdatePaymentPlansCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UpdatePaymentPlansCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdatePaymentPlansCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);

                var list = new List<PaymentPlan>();

                await _context.BeginTransactionAsync();

                foreach (var item in request.PaymentPlans)
                {
                    //check if the name of the payment plan already exists and conflicts with this new name 
                    var UpdatedEntityExists = await _context.PaymentPlans
                        .AnyAsync(x => x.OrganisationId == request.OrganisationId && x.Id != item.Id && x.Name.ToLower() == item.Name.ToLower());

                    if (UpdatedEntityExists)
                    {
                        return Result.Failure($"Another payment plan named {item.Name} already exists. Please change the name.");
                    }
                    var entity = await _context.PaymentPlans.Where(x => x.OrganisationId == request.OrganisationId && x.Id == item.Id)
                                           .FirstOrDefaultAsync();

                    if (entity == null)
                    {
                        entity = new PaymentPlan
                        {
                            Name = item.Name,
                            OrganisationId = request.OrganisationId,
                            OrganisationName = request.OrganisationName,
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
                        entity.Name = item.Name;
                        entity.LastModifiedBy = request.UserId;
                        entity.LastModifiedDate = DateTime.Now;
                    }
                    list.Add(entity);
                }

                _context.PaymentPlans.UpdateRange(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<PaymentPlanDto>>(list);
                return Result.Success("Payment plans update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Payment plans update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }


    }


}
