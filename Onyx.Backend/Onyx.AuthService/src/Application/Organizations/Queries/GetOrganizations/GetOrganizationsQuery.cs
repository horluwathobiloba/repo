using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Onyx.AuthService.Application.Common.Interfaces;
using Onyx.AuthService.Application.Common.Models;

namespace Onyx.AuthService.Application.Organizations.Queries.GetOrganizations
{
    public class GetOrganizationsQuery : IRequest<Result>
    {
    }

    public class GetOrganizationsQueryHandler : IRequestHandler<GetOrganizationsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBase64ToFileConverter _fileConverter;

        public GetOrganizationsQueryHandler(IApplicationDbContext context, IMapper mapper,IBase64ToFileConverter fileConverter)
        {
            _context = context;
            _mapper = mapper;
            _fileConverter = fileConverter;
        }

        public async Task<Result> Handle(GetOrganizationsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var organizations = await _context.Organizations.ToListAsync();
              
                var orgListResult = _mapper.Map<List<OrganizationListDto>>(organizations);
                return Result.Success(orgListResult);
 
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Error retrieving organizations", ex?.Message + ex?.InnerException.Message });
            }
        }
    }
}
