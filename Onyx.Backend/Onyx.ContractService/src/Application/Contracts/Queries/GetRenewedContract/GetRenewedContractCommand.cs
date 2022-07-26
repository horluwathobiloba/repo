using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.Contracts.Commands.UpdateContractStatus;
using Onyx.ContractService.Application.Contracts.Queries.GetContracts;
using Onyx.ContractService.Application.SupportingDocuments.Queries.GetSupportingDocuments;
using Onyx.ContractService.Application.Vendors.Queries.GetVendors;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Contracts.Queries.GetRenewedContract
{
    public class GetRenewedContractCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
    }

    public class GetRenewedContractCommandHandler : IRequestHandler<GetRenewedContractCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;

        public GetRenewedContractCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService, IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
            _emailService = emailService;
        }

        public async Task<Result> Handle(GetRenewedContractCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var entity = await _context.Contracts.Include(a=>a.Vendor).FirstOrDefaultAsync(x => x.OrganisationId == request.OrganisationId && x.Id == request.Id
                && (x.Status == Status.Active || x.ContractStatus == ContractStatus.Active || x.ContractStatus == ContractStatus.Expired));

                if (entity == null)
                {
                    return Result.Failure("Invalid contract!");
                }

                if (entity.RenewedContractId > 0)
                {
                    return Result.Failure("This contract has already been renewed by another user!");
                }
                var contract = _mapper.Map<ContractDto>(entity);
                var supportingDocuments=await _context.SupportingDocuments.Where(a => a.OrganisationId == request.OrganisationId && a.ContractId == contract.Id).ToListAsync();
                //load the intital contract Id
                if (entity.InitialContractId <= 0)
                {
                    contract.InitialContractId = entity.Id;
                }
                else
                {
                    contract.InitialContractId = entity.InitialContractId;
                }               
                contract.RenewedContractId = entity.Id;
                contract.Name = $"{entity.Name} - {string.Format("{0:ddMMyyyy}", DateTime.Now)} "; // should we be using version as a naming convention?
                contract.Status = Status.Inactive;
                contract.ContractStatus = ContractStatus.Processing;
                contract.ContractStatusDesc = ContractStatus.Processing.ToString();
                contract.ContractStartDate = null;
                contract.UserId = request.UserId;
                contract.NextActorAction = null;
                contract.NextActorEmail = null;
                contract.NextActorRank = 0;
                contract.IsAnExecutedDocument = false;
                contract.InitiatorSignature = null;
                contract.InitiatorSignature = null;
                contract.InitiatorSignatureFileExtension = null;
                contract.InitiatorSignatureMimeType = null;
                contract.ExecutedContract = null;
                contract.ExecutedContractFileExtension = null;
                contract.ExecutedContractMimeType = null;
                contract.VendorId = entity.VendorId;
                if (entity.Vendor != null)
                {
                    contract.Vendor = _mapper.Map<VendorDto>(entity.Vendor);
                }
                if (supportingDocuments != null && supportingDocuments.Count > 0)
                {
                    contract.SupportingDocuments = _mapper.Map<List<SupportingDocumentDto>>(supportingDocuments);
                }
                contract.Id = 0;

                return Result.Success($"The {entity.DocumentType.ToString()} ({entity.Name}) document has been cloned for renewal successfully", contract);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Contract renewal failed { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }


    }
}
