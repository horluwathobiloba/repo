using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.ContractDuration.Queries.GetContractDurations;
using Onyx.ContractService.Application.VendorTypes.Queries.GetVendorTypes;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractDuration.Commands.UpdateContractDurations
{
    public class UpdateContractDurations : IRequest<Result>
    {
        public List<UpdateContractDurationRequest> ContractDurations { get; set; }
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateContractDurationsHandler : IRequestHandler<UpdateContractDurations, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateContractDurationsHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result> Handle(UpdateContractDurations request, CancellationToken cancellationToken)
        {
            try
            {
                var list = new List<Domain.Entities.ContractDuration>();
                await _context.BeginTransactionAsync();

                foreach (var item in request.ContractDurations)
                {
                    //check if the name of the vendor type already exists and conflicts with this new name 
                  
                    var entity = await _context.ContractDurations.Where(x => x.OrganisationId == request.OrganisationId && x.Id == item.Id)
                                           .FirstOrDefaultAsync();

                    if (entity == null)
                    {
                        entity = new Domain.Entities.ContractDuration
                        {
                           
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
                        entity.LastModifiedBy = request.UserId;
                        entity.LastModifiedDate = DateTime.Now;
                    }
                    list.Add(entity);
                }

                _context.ContractDurations.UpdateRange(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<ContractDurationDto>>(list);
                return Result.Success("Contract Duration update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Contract Duration update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }


    }


}
