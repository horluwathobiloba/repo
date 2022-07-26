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
    public class GetDocumentStatQuery:IRequest<Result>
    {
        public int OrganizationId { get; set; }
        public int Year { get; set; }
    }
    public class GetDocumentStatQueryHandler : IRequestHandler<GetDocumentStatQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        public GetDocumentStatQueryHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(GetDocumentStatQuery request, CancellationToken cancellationToken)
        {
            var contracts = await _context.Contracts.Where(b => b.OrganisationId == request.OrganizationId&&b.CreatedDate.Year==request.Year).ToListAsync();
            var resultList = new List<DocumentStatVm>();
            foreach (Year month in Enum.GetValues(typeof(Year)))
            {
                var stat = new DocumentStatVm();
                stat.Month = month.ToString();
                stat.PermitType = contracts.Where(x => x.DocumentType == DocumentType.Permit&&x.CreatedDate.Month==(int)month).Count();
                stat.ContractType = contracts.Where(x => x.DocumentType == DocumentType.Contract&&x.CreatedDate.Month==(int)month).Count();
                resultList.Add(stat);
            }
            return Result.Success(resultList);
        }
    }
}
