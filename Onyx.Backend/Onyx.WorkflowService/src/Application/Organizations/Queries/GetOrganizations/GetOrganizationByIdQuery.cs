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
    public class GetOrganizationByIdQuery : IRequest<Result>
    {
        public int  Id { get; set; }
    }

    public class GetOrganizationByIdQueryHandler : IRequestHandler<GetOrganizationByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBase64ToFileConverter _fileConverter;

        public GetOrganizationByIdQueryHandler(IApplicationDbContext context, IMapper mapper, IBase64ToFileConverter fileConverter)
        {
            _context = context;
            _mapper = mapper;
            _fileConverter = fileConverter;
        }

        public async Task<Result> Handle(GetOrganizationByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var organization = await _context.Organizations.FindAsync(request.Id);
                if (organization != null)
                    organization.LogoFileLocation = _fileConverter.RetrieveFileUrl(organization.LogoFileLocation);
                var orgResult = _mapper.Map<OrganizationDto>(organization);
                return Result.Success(orgResult);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
