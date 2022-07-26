using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Enums;
using Onyx.ContractService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.DashBoards
{
    public class DocumentDistributionQuery:IRequest<Result>
    {
        public int OrganizationId { get; set; }
        public DocumentType DocumentType { get; set; }
    }


    public class DocumentDistributionQueryHandler : IRequestHandler<DocumentDistributionQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        public DocumentDistributionQueryHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(DocumentDistributionQuery request, CancellationToken cancellationToken)
        {
            if (request.DocumentType==DocumentType.Contract)
            {
                var result=await GetQueryByContractType(request.OrganizationId);
                return Result.Success(result);
            } 
            if (request.DocumentType==DocumentType.Permit)
            {
                var result=await GetQueryByPermit(request.OrganizationId);
                return Result.Success(result);
            }
            return Result.Failure("Error retreiving data ");
            
        }
        private async Task<List<DocumentTypeDistributionVm>> GetQueryByPermit(int organizationId)
        {
            var permitList = await _context.PermitTypes.Where(x => x.OrganisationId == organizationId).ToListAsync();
            var contracts = await _context.Contracts.Where(x => x.OrganisationId == organizationId && x.DocumentType == DocumentType.Permit).ToListAsync();
            //var contracts = await _context.Contracts.ToListAsync();
            var list = new List<DocumentTypeDistributionVm>();
            foreach (var permit in permitList)
            {
                var result = new DocumentTypeDistributionVm();
                result.DocumentType = permit.Name;
                result.DocumentCount = contracts.Where(x => x.PermitTypeId ==permit.Id).Count();
                list.Add(result);
            }
            var resultList = list.OrderByDescending(x => x.DocumentCount).Take(5).ToList();
            return resultList;
        }
        private async Task<List<DocumentTypeDistributionVm>> GetQueryByContractType(int organizationId)
        {
            var contractTypeList = await _context.ContractTypes.Where(x => x.OrganisationId == organizationId).ToListAsync();
            var contracts = await _context.Contracts.Where(x => x.OrganisationId == organizationId && x.DocumentType == DocumentType.Contract).ToListAsync();
            var list = new List<DocumentTypeDistributionVm>();
            foreach (var contractType in contractTypeList)
            {
                var result = new DocumentTypeDistributionVm();
                result.DocumentType = contractType.Name;
                result.DocumentCount = contracts.Where(x => x.ContractTypeId == contractType.Id).Count();
                list.Add(result);
            }
            var resultList = list.OrderByDescending(x => x.DocumentCount).Take(5).ToList();
            return resultList;
        }
    }
}
