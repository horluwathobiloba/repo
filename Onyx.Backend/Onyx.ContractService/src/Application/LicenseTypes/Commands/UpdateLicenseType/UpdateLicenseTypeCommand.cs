using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.LicenseTypes.Queries.GetLicenseTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.LicenseTypes.Commands.UpdateLicenseType
{
    public class UpdateLicenseTypeCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OrganisationId { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateLicenseTypeCommandHandler : IRequestHandler<UpdateLicenseTypeCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UpdateLicenseTypeCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdateLicenseTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);

                var entity = await _context.LicenseTypes.FirstOrDefaultAsync(x => x.OrganisationId == request.OrganisationId && x.Id == request.Id);
                if (entity == null)
                {
                    return Result.Failure($"Invalid License type specified.");
                }

                //check if the name of the service types are
                var UpdatedEntityExists = await _context.LicenseTypes.AnyAsync(x => x.OrganisationId == request.OrganisationId && x.Id != request.Id
                && x.Name.ToLower().Trim() == request.Name.ToLower().Trim());

                if (UpdatedEntityExists)
                {
                    return Result.Failure($"Another License type named '{request.Name}' already exists. Please change the name.");
                }

                entity.Name = request.Name;
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                _context.LicenseTypes.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<LicenseTypeDto>(entity);
                return Result.Success("License type update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"License Type update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }


    }


}
