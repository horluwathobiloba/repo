using MediatR;
using Onyx.AuthService.Application.Common.Interfaces;
using Onyx.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Onyx.AuthService.Domain.Entities;
using Onyx.AuthService.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Onyx.AuthService.Application.JobFunctions.Commands
{
    public class CreateJobFunctionCommand : IRequest<Result>
    {
        public string Name { get; set; }
        public int? OrganisationId { get; set; }
        public string UserId { get; set; }
    }

    public class CreateJobFunctionCommandHandler : IRequestHandler<CreateJobFunctionCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly ISqlService _sqlService;

        public CreateJobFunctionCommandHandler(IApplicationDbContext context, IIdentityService identityService, ISqlService sqlService)
        {
            _context = context;
            _identityService = identityService;
            _sqlService = sqlService;
        }

        public async Task<Result> Handle(CreateJobFunctionCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _identityService.GetUserByIdAndOrganization(request.UserId, Convert.ToInt32(request.OrganisationId));
                if (result.staff==null)
                {
                    return Result.Failure(new string[] { "Invalid User"});

                }
                var exists = await _context.JobFunctions.AnyAsync(x => x.OrganisationId == request.OrganisationId && x.Name == request.Name);
                if (exists)
                {
                    return Result.Failure($"A record with the jobfunction '{request.Name}'already exists!");
                }
                Domain.Entities.JobFunction jobFunction = new Domain.Entities.JobFunction
                {
                    Name = request.Name,
                    OrganisationId = request.OrganisationId,
                    CreatedById = request.UserId,
                    LastModifiedById = request.UserId,
                    Status = Status.Active
                };
                await _context.JobFunctions.AddAsync(jobFunction);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Job function creation successful",jobFunction);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Job function creation was not successful", ex?.Message + ex?.InnerException.Message });
            }

        }

    }
}
