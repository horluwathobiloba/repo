using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.PermitTypes.Queries.GetPermitTypes;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.PermitTypes.Commands.UpdatePermitTypes
{
    public class UpdatePermitTypesCommand : AuthToken, IRequest<Result>
    {
        public List<UpdatePermitTypeRequest> PermitTypes { get; set; }
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public string UserId { get; set; }
    }

    public class UpdatePermitTypesCommandHandler : IRequestHandler<UpdatePermitTypesCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;


        public UpdatePermitTypesCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdatePermitTypesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var list = new List<PermitType>();

                await _context.BeginTransactionAsync();

                foreach (var item in request.PermitTypes)
                {
                    //check if the name of the Permit type already exists and conflicts with this new name 
                    var UpdatedEntityExists = await _context.PermitTypes
                        .AnyAsync(x => x.OrganisationId == request.OrganisationId && x.Id != item.Id && x.Name.ToLower().Trim() == item.Name.ToLower().Trim());

                    if (UpdatedEntityExists)
                    {
                        return Result.Failure($"Another Permit type named {item.Name} already exists. Please change the name and try again.");
                    }
                    var entity = await _context.PermitTypes.Where(x => x.OrganisationId == request.OrganisationId && x.Id == item.Id)
                                           .FirstOrDefaultAsync();

                    if (entity == null)
                    {
                        entity = new PermitType
                        {
                            Name = item.Name,
                            OrganisationId = request.OrganisationId,
                            OrganisationName =  _authService.Organisation?.OrganisationName,
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

                _context.PermitTypes.UpdateRange(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<PermitTypeDto>>(list);
                return Result.Success("Permit types update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Permit Types update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }


    }


}
