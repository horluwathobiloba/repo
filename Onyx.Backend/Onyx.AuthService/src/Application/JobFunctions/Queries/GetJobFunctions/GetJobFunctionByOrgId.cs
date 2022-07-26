using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.AuthService.Application.Common.Interfaces;
using Onyx.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.AuthService.Application.JobFunctions.Queries.GetJobFunction
{
    public class GetJobFunctionByOrgId:IRequest<Result>
    {
        public int OrgId { get; set; }
    }
    public class GetJobFunctionByOrgIdQueryHandler : IRequestHandler<GetJobFunctionByOrgId, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBase64ToFileConverter _fileConverter;

        public GetJobFunctionByOrgIdQueryHandler(IApplicationDbContext context, IMapper mapper, IBase64ToFileConverter fileConverter)
        {
            _context = context;
            _mapper = mapper;
            _fileConverter = fileConverter;
        }

        public async Task<Result> Handle(GetJobFunctionByOrgId request, CancellationToken cancellationToken)
        {
            try
            {
                var jobs = await _context.JobFunctions.Where(x=>x.OrganisationId == request.OrgId).ToListAsync();
            
                return Result.Success(jobs);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Bank creation was not successful", ex?.Message + ex?.InnerException.Message });
            }
        }
    }
}
