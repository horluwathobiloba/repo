using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Onyx.WorkFlowService.Application.Common.Interfaces;
using Onyx.WorkFlowService.Application.Common.Models;

namespace Onyx.WorkFlowService.Application.Organizations.Queries.GetOrganizations
{
    public class GetOrganizationByNameQuery : IRequest<Result>
    {
        public string  Name { get; set; }
    }

    public class GetOrganizationByNameQueryHandler : IRequestHandler<GetOrganizationByNameQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBase64ToFileConverter _fileConverter;

        public GetOrganizationByNameQueryHandler(IApplicationDbContext context, IMapper mapper, IBase64ToFileConverter fileConverter)
        {
            _context = context;
            _mapper = mapper;
            _fileConverter = fileConverter;
        }

        public async Task<Result> Handle(GetOrganizationByNameQuery request, CancellationToken cancellationToken)
        {
            var organization = await  _context.Organizations.FirstOrDefaultAsync(a=>a.Name == request.Name);
            if (organization != null)
                organization.LogoFileLocation = _fileConverter.RetrieveFileUrl(organization.LogoFileLocation);
            var org = _mapper.Map<OrganizationDto>(organization);
            return Result.Success(org);
        }
    }
}
