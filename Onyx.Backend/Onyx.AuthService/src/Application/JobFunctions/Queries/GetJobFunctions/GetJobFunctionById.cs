using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.AuthService.Application.Common.Interfaces;
using Onyx.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.AuthService.Application.JobFunctions.Queries.GetJobFunction
{
    public class GetJobFunctionById : IRequest<Result>
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int OrganizationId { get; set; }
    }

    public class GetJobFunctionByIdQueryHandler : IRequestHandler<GetJobFunctionById, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IBase64ToFileConverter _fileConverter;

        public GetJobFunctionByIdQueryHandler(IApplicationDbContext context, IMapper mapper, IIdentityService identityService, IBase64ToFileConverter fileConverter)
        {
            _context = context;
            _mapper = mapper;
            _fileConverter = fileConverter;
            _identityService = identityService;
        }

        public async Task<Result> Handle(GetJobFunctionById request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _identityService.GetUserByIdAndOrganization(request.UserId, request.OrganizationId);
                if (result.staff == null)
                {
                    return Result.Failure(new string[] { "Invalid User" });
                }
                var job = await _context.JobFunctions.FirstOrDefaultAsync(x => x.Id == request.Id);
                return Result.Success(job);
            }
            catch (Exception ex)
            {
              throw ex;
            }
        }
    }


}
