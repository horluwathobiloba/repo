using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Infrastructure.Services;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractTypes.Commands.UpdateContractType
{
    public class UpdateContractTypeCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //public bool EnableInternalSignatories { get; set; }
        //public int NumberOfInternalSignatories { get; set; }
        //public bool EnableExternalSignatories { get; set; }
        //public int NumberOfExternalSignatories { get; set; }
        //public bool EnableThirdPartySignatories { get; set; }
        //public int NumberOfThirdPartySignatories { get; set; }
        //public bool EnableWitnessSignatories { get; set; }
        //public int NumberOfWitnessSignatories { get; set; }
        //public bool EnableContractTypeInitiatorRestriction { get; set; }
        public FileUploadRequest TemplateFilePath { get; set; } = new FileUploadRequest();
        public List<UpdateContractTypeRoleInitiatorRequest> ContractTypeRoleInitiators { get; set; }
        public List<UpdateContractTypeJobFunctionInitiatorRequest> ContractTypeJobFunctionInitiators { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateContractTypeCommandHandler : IRequestHandler<UpdateContractTypeCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IBlobStorageService _blobService;

        public UpdateContractTypeCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService, IBlobStorageService blobService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
            _blobService = blobService;
        }
        public async Task<Result> Handle(UpdateContractTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                //check if the name of the other record has the new name
                var updatedNameExists = (await _context.ContractTypes.ToListAsync()).Where(x => x.OrganisationId == request.OrganisationId && x.Id != request.Id
                && x.Name.Equals(request.Name, StringComparison.InvariantCultureIgnoreCase));

                if (updatedNameExists.Count() > 0)
                {
                    return Result.Failure($"A record with this product service type name {request.Name} already exists. Please change the name.");
                }

                var entity = await _context.ContractTypes
                    .Where(x => x.OrganisationId == request.OrganisationId && x.Id == request.Id)
                    .FirstOrDefaultAsync();

                if (entity == null)
                {
                    return Result.Failure($"Invalid contract type specified.");
                }

                //_context.Entry(entity).CurrentValues.SetValues(request);

                entity.Name = request.Name;
                entity.Description = request.Description;
                //entity.EnableInternalSignatories = request.EnableInternalSignatories;
                //entity.EnableExternalSignatories = request.EnableExternalSignatories;
                //entity.EnableThirdPartySignatories = request.EnableThirdPartySignatories;
                //entity.EnableWitnessSignatories = request.EnableWitnessSignatories;
                //entity.NumberOfInternalSignatories = request.NumberOfInternalSignatories;
                //entity.NumberOfExternalSignatories = request.NumberOfExternalSignatories;
                //entity.NumberOfThirdPartySignatories = request.NumberOfThirdPartySignatories;
                //entity.NumberOfWitnessSignatories = request.NumberOfWitnessSignatories;
                entity.TemplateFilePath = await request.TemplateFilePath.SaveBlobFile(request.Name, _blobService);
               // entity.EnableContractTypeInitiatorRestriction = request.EnableContractTypeInitiatorRestriction;
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                if (string.IsNullOrWhiteSpace(entity.TemplateFilePath))
                {
                    return Result.Failure("Kindly provide file details");
                }

                _context.ContractTypes.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Contract type was updated successfully",entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Contract type update failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

    }

}
