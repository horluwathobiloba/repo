using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.PermitTypes.Queries.GetPermitTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.PermitTypes.Commands.UpdatePermitType
{
    public class UpdatePermitTypeCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OrganisationId { get; set; }
        public string UserId { get; set; }
    }

    public class UpdatePermitTypeCommandHandler : IRequestHandler<UpdatePermitTypeCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UpdatePermitTypeCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdatePermitTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);

                var entity = await _context.PermitTypes.FirstOrDefaultAsync(x => x.OrganisationId == request.OrganisationId && x.Id == request.Id);
                if (entity == null)
                {
                    return Result.Failure($"Invalid Permit type specified.");
                }

                //check if the name of the service types are
                var UpdatedEntityExists = await _context.PermitTypes.AnyAsync(x => x.OrganisationId == request.OrganisationId && x.Id != request.Id
                && x.Name.ToLower().Trim() == request.Name.ToLower().Trim());

                if (UpdatedEntityExists)
                {
                    return Result.Failure($"Another Permit type named '{request.Name}' already exists. Please change the name.");
                }

                entity.Name = request.Name;
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                _context.PermitTypes.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<PermitTypeDto>(entity);
                return Result.Success("Permit type update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Permit Type update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }


    }


}
