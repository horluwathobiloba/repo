using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using Onyx.ContractService.Infrastructure.Services;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractTypes.Commands.UpdateContractTypeAndInitiators
{
    public class UpdateContractTypeAndInitiatorsCommand : AuthToken, IRequest<Result>
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
        public List<CreateContractTypeRoleInitiatorRequest> ContractTypeRoleInitiators { get; set; }

        public List<CreateContractTypeJobFunctionInitiatorRequest> ContractTypeJobFunctionInitiators { get; set; }

        public FileUploadRequest TemplateFilePath { get; set; } = new FileUploadRequest();

        public string UserId { get; set; }
    }

    public class UpdateContractTypeAndInitiatorsCommandHandler : IRequestHandler<UpdateContractTypeAndInitiatorsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IBlobStorageService _blobService;
        public UpdateContractTypeAndInitiatorsCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService, IBlobStorageService blobService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
            _blobService = blobService;
        }
        public async Task<Result> Handle(UpdateContractTypeAndInitiatorsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                //check if the name of the other record has the new name
                var updatedNameExists = (await _context.ContractTypes.ToListAsync()).Where(x => x.OrganisationId == request.OrganisationId && x.Id != request.Id
                && x.Name.ToLower() == request.Name.ToLower());

                if (updatedNameExists.Count() > 0)
                {
                    return Result.Failure($"A record with this product service type name {request.Name} already exists. Please change the name.");
                }

                var entity = await _context.ContractTypes
                    .Where(x => x.OrganisationId == request.OrganisationId && x.Id == request.Id)
                    .Include(p => p.ContractTypeInitiators)
                    .FirstOrDefaultAsync();

                if (entity == null)
                {
                    return Result.Failure($"Invalid contract type specified.");
                }

               // _context.Entry(entity).CurrentValues.SetValues(request);

                entity.TemplateFilePath = await request.TemplateFilePath.SaveBlobFile(request.Name, _blobService);
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;
                _context.ContractTypes.Update(entity);

                var listofUpdates = new List<ContractTypeInitiator>();

                //ContractTypeRoleInitiators
                // if (request.EnableContractTypeInitiatorRestriction)
                {
                    // Delete children
                    foreach (var contractInitiator in entity.ContractTypeInitiators.ToList())
                    {
                        if (!request.ContractTypeRoleInitiators.Any(c => c.Id == contractInitiator.Id))
                        {
                            _context.ContractTypeInitiators.Remove(contractInitiator);
                        }
                    }
                    foreach (var contractInitiator in request.ContractTypeRoleInitiators)
                    {
                        var existingInitiator = entity.ContractTypeInitiators
                            .Where(c => c.Id == contractInitiator.Id)
                            .SingleOrDefault();

                        if (existingInitiator != null)
                        {
                            _context.Entry(existingInitiator).CurrentValues.SetValues(contractInitiator);  // Update the contract type initiator
                            existingInitiator.LastModifiedBy = request.UserId;
                            existingInitiator.LastModifiedDate = DateTime.Now;
                        }
                        else
                        {
                            var newInitiator = this.Convert(contractInitiator, entity);     // Insert the new contract type initiator
                            _context.ContractTypeInitiators.Add(newInitiator);
                        }
                    }

                    /* Alternative */

                    //check if initiator restriction is enabled and make sure there is no invalid role id
                    if (request.ContractTypeRoleInitiators.Count > 0 && request.ContractTypeRoleInitiators.Any(a => a.RoleId == 0))
                    {
                        return Result.Failure($"A valid Role must be specified for all initiators when contract type initiator restriction is turned on!");
                    }

                    foreach (var contractInitiator in request.ContractTypeRoleInitiators)
                    {
                        var existingInitiator = await _context.ContractTypeInitiators.Where(x => x.OrganisationId == entity.OrganisationId
                        && x.ContractTypeId == entity.Id && x.Id == contractInitiator.Id).SingleOrDefaultAsync();

                        if (existingInitiator != null)
                        {
                            //if the record exists, check if the status is deactivated or inactive then activate it.
                            if (existingInitiator.Status == Status.Deactivated || existingInitiator.Status == Status.Inactive)
                            {
                                existingInitiator.Status = Status.Active;
                                existingInitiator.StatusDesc = Status.Active.ToString();
                                existingInitiator.LastModifiedBy = request.UserId;
                                existingInitiator.LastModifiedDate = DateTime.Now;
                                listofUpdates.Add(existingInitiator);
                            }
                            //else do nothing
                        }
                        else
                        {
                            entity.ContractTypeInitiators.Add(this.Convert(contractInitiator, entity));
                        }
                    }

                //JobFunction
                

                    //check if initiator restriction is enabled and make sure there is no invalid role id
                    if (request.ContractTypeJobFunctionInitiators.Count > 0 && request.ContractTypeJobFunctionInitiators.Any(a => a.Id == 0))
                    {
                        return Result.Failure($"A valid JobFunction must be specified for all initiators when contract type initiator restriction is turned on!");
                    }

                    foreach (var contractInitiator in request.ContractTypeJobFunctionInitiators)
                    {
                        var existingInitiator = await _context.ContractTypeInitiators.Where(x => x.OrganisationId == entity.OrganisationId
                        && x.ContractTypeId == entity.Id && x.Id == contractInitiator.Id).SingleOrDefaultAsync();

                        if (existingInitiator != null)
                        {
                            //if the record exists, check if the status is deactivated or inactive then activate it.
                            if (existingInitiator.Status == Status.Deactivated || existingInitiator.Status == Status.Inactive)
                            {
                                existingInitiator.Status = Status.Active;
                                existingInitiator.StatusDesc = Status.Active.ToString();
                                existingInitiator.LastModifiedBy = request.UserId;
                                existingInitiator.LastModifiedDate = DateTime.Now;
                                listofUpdates.Add(existingInitiator);
                            }
                            //else do nothing
                        }
                        else
                        {
                            entity.ContractTypeInitiators.Add(this.Convert(contractInitiator, entity));
                        }
                    }
                }

                //else
                // {
                // //deactivate all the contract initiators
                // foreach (var item in request.ContractTypeRoleInitiators)
                // {
                //     var existingInitiator = await _context.ContractTypeInitiators.Where(x => x.OrganisationId == entity.OrganisationId
                //     && x.ContractTypeId == entity.Id && x.RoleId == item.RoleId).SingleOrDefaultAsync();

                //     //If the record exists, deactivate it
                //     if (existingInitiator != null)
                //     {
                //         existingInitiator.Status = Status.Deactivated;
                //         existingInitiator.StatusDesc = Status.Deactivated.ToString();
                //         existingInitiator.LastModifiedBy = request.UserId;
                //         existingInitiator.LastModifiedDate = DateTime.Now;
                //         listofUpdates.Add(existingInitiator);
                //     }
                //     //else do nothing!
                //// }
                //}

                ////deactivate all the contract initiators
                //foreach (var item in request.ContractTypeJobFunctionInitiators)
                //{
                //    var existingInitiator = await _context.ContractTypeInitiators.Where(x => x.OrganisationId == entity.OrganisationId
                //    && x.ContractTypeId == entity.Id && x.JobFunctionId == item.JobFunctionId).SingleOrDefaultAsync();

                //    //If the record exists, deactivate it
                //    if (existingInitiator != null)
                //    {
                //        existingInitiator.Status = Status.Deactivated;
                //        existingInitiator.StatusDesc = Status.Deactivated.ToString();
                //        existingInitiator.LastModifiedBy = request.UserId;
                //        existingInitiator.LastModifiedDate = DateTime.Now;
                //        listofUpdates.Add(existingInitiator);
                //    }
                //    //else do nothing!
                //    // }
                //}

                //save all changes;
                _context.ContractTypeInitiators.UpdateRange(listofUpdates);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Contract type was updated successfully",entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Contract type update failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        private ContractTypeInitiator Convert(CreateContractTypeRoleInitiatorRequest item, ContractType contractType)
        {
            var entity = new ContractTypeInitiator
            {
                OrganisationId = contractType.OrganisationId,
                OrganisationName = _authService.Organisation.Name,
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

        private ContractTypeInitiator Convert(CreateContractTypeJobFunctionInitiatorRequest item, ContractType contractType)
        {
            var entity = new ContractTypeInitiator
            {
                OrganisationId = contractType.OrganisationId,
                OrganisationName = _authService.Organisation.Name,
                ContractTypeId = contractType.Id,
                //RoleId = item.RoleId,
                //RoleName = item.RoleName,
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
