using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.LicenseTypes.Queries.GetLicenseTypes;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.LicenseTypes.Commands.UpdateLicenseTypes
{
    public class UpdateLicenseTypesCommand : AuthToken, IRequest<Result>
    {
        public List<UpdateLicenseTypeRequest> LicenseTypes { get; set; }
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateLicenseTypesCommandHandler : IRequestHandler<UpdateLicenseTypesCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;


        public UpdateLicenseTypesCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdateLicenseTypesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var list = new List<LicenseType>();

                await _context.BeginTransactionAsync();

                foreach (var item in request.LicenseTypes)
                {
                    //check if the name of the License type already exists and conflicts with this new name 
                    var UpdatedEntityExists = await _context.LicenseTypes
                        .AnyAsync(x => x.OrganisationId == request.OrganisationId && x.Id != item.Id && x.Name.ToLower().Trim() == item.Name.ToLower().Trim());

                    if (UpdatedEntityExists)
                    {
                        return Result.Failure($"Another License type named {item.Name} already exists. Please change the name and try again.");
                    }
                    var entity = await _context.LicenseTypes.Where(x => x.OrganisationId == request.OrganisationId && x.Id == item.Id)
                                           .FirstOrDefaultAsync();

                    if (entity == null)
                    {
                        entity = new LicenseType
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

                _context.LicenseTypes.UpdateRange(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<LicenseTypeDto>>(list);
                return Result.Success("License types update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"License Types update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }


    }


}
