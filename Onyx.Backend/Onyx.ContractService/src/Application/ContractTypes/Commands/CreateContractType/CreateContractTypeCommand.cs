using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using Onyx.ContractService.Domain.ViewModels;
using Onyx.ContractService.Infrastructure.Services;
using ReventInject;
using ReventInject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractTypes.Commands.CreateContractType
{
    public class CreateContractTypeCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
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
        public FileUploadRequest TemplateFilePath { get; set; } = new FileUploadRequest();

        public List<CreateContractTypeRoleInitiatorRequest> ContractTypeRoleInitiators { get; set; }

        public List<CreateContractTypeJobFunctionInitiatorRequest> ContractTypeJobFunctionInitiators { get; set; }
        public string UserId { get; set; }
    }


    public class CreateContractTypeCommandHandler : IRequestHandler<CreateContractTypeCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBase64ToFileConverter _fileConverter;
        private readonly IAuthService _authService;
        protected Tuple<OrganisationVm, RoleListVm, UserListVm> _OrgData;
        private readonly IBlobStorageService _blobService;

        public CreateContractTypeCommandHandler(IApplicationDbContext context, IMapper mapper, IBase64ToFileConverter fileConverter, IAuthService authService, IBlobStorageService blobService)
        {
            _context = context;
            _mapper = mapper;
            _fileConverter = fileConverter;
            _authService = authService;
            _blobService = blobService;
        }
        public async Task<Result> Handle(CreateContractTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var exists = await _context.ContractTypes.AnyAsync(x => x.OrganisationId == request.OrganisationId && x.Name.ToLower() == request.Name.ToLower());

                if (exists)
                {
                    return Result.Failure($"Contract type name already exists!");
                }
                await _context.BeginTransactionAsync();

                var entity = new ContractType
                {
                    Description = request.Description,
                    OrganisationId = request.OrganisationId,
                    //EnableContractTypeInitiatorRestriction = request.EnableContractTypeInitiatorRestriction,
                    Name = request.Name,
                    //EnableInternalSignatories = request.EnableInternalSignatories,
                    //EnableExternalSignatories = request.EnableExternalSignatories,
                    //EnableThirdPartySignatories = request.EnableThirdPartySignatories,
                    //EnableWitnessSignatories = request.EnableWitnessSignatories,
                    //NumberOfInternalSignatories = request.NumberOfInternalSignatories,
                    //NumberOfExternalSignatories = request.NumberOfExternalSignatories,
                    //NumberOfThirdPartySignatories = request.NumberOfThirdPartySignatories,
                    //NumberOfWitnessSignatories = request.NumberOfWitnessSignatories,
                    TemplateFilePath = await request.TemplateFilePath.SaveBlobFile(request.Name,_blobService),
                    ContractTypeStatus = ContractTypeStatus.InProgress.ToString(),

                    DeviceId = new NetworkService().getClientIPAddress(),
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.ContractTypes.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

               // if (request.EnableContractTypeInitiatorRestriction)
                {
                    //check if initiator restriction is enabled and make sure there is no invalid role id
                    if (request.ContractTypeRoleInitiators.Count > 0 && request.ContractTypeRoleInitiators.Any(a => a.RoleId == 0))
                    {
                        return Result.Failure($"Role must be specified for all initiators when contract type initiator restriction is turned on!");
                    }
                    if (request.ContractTypeJobFunctionInitiators.Count > 0 && request.ContractTypeJobFunctionInitiators.Any(a => a.JobFunctionId == 0))
                    {
                        return Result.Failure($"Role must be specified for all initiators when contract type initiator restriction is turned on!");
                    }

                    foreach (var x in request.ContractTypeRoleInitiators)
                    {
                        entity.ContractTypeInitiators.Add(await this.Convert(x, entity));
                    }
                    foreach (var x in request.ContractTypeJobFunctionInitiators)
                    {
                        entity.ContractTypeInitiators.Add(await this.Convert(x, entity));
                    }
                    await _context.ContractTypeInitiators.AddRangeAsync(entity.ContractTypeInitiators);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                await _context.CommitTransactionAsync();
                return Result.Success("Contract type was created successfully", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Contract type creation failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message} ");
            }
        }

        private async Task<ContractTypeInitiator> Convert(CreateContractTypeRoleInitiatorRequest item, ContractType contractType)
        {
            var exists = await _context.ContractTypeInitiators.AnyAsync(x => x.OrganisationId == contractType.OrganisationId
            && x.ContractTypeId == contractType.Id && x.RoleId == item.RoleId);

            if (exists)
            {
                throw new Exception($"Contract type initiator already exists for {item.RoleName} !");
            }

            var entity = new ContractTypeInitiator
            {
                OrganisationId = contractType.OrganisationId,
                OrganisationName = contractType.OrganisationName,
                ContractTypeId = contractType.Id,
                RoleId = item.RoleId,
                RoleName = item.RoleName,
                //JobFunctionId = item.JobFunctionId,
                //JobFunctionName = item.JobFunctionName,
                CreatedBy = contractType.CreatedBy,
                CreatedDate = DateTime.Now,
                LastModifiedBy = contractType.LastModifiedBy,
                LastModifiedDate = DateTime.Now,
                Status = Status.Active,
                StatusDesc = Status.Active.ToString()
            };

            return entity;
        }

        private async Task<ContractTypeInitiator> Convert(CreateContractTypeJobFunctionInitiatorRequest item, ContractType contractType)
        {
            var exists = await _context.ContractTypeInitiators.AnyAsync(x => x.OrganisationId == contractType.OrganisationId
            && x.ContractTypeId == contractType.Id && x.JobFunctionId == item.JobFunctionId);

            if (exists)
            {
                throw new Exception($"Contract type initiator already exists for {item.JobFunctionName} !");
            }

            var entity = new ContractTypeInitiator
            {
                OrganisationId = contractType.OrganisationId,
                OrganisationName = contractType.OrganisationName,
                ContractTypeId = contractType.Id,
                //RoleId = item.RoleId,
                //RoleName = item.
                JobFunctionId = item.JobFunctionId,
                JobFunctionName = item.JobFunctionName,
                CreatedBy = contractType.CreatedBy,
                CreatedDate = DateTime.Now,
                LastModifiedBy = contractType.LastModifiedBy,
                LastModifiedDate = DateTime.Now,
                Status = Status.Active,
                StatusDesc = Status.Active.ToString()
            };

            return entity;
        }
    }

}
