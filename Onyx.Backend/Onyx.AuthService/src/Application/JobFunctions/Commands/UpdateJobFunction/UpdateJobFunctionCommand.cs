using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.AuthService.Application.Common.Interfaces;
using Onyx.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.AuthService.Application.JobFunctions.Commands
{
    public class UpdateJobFunctionCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public int OrganizationId { get; set; }
    }
    public class UpdateJobFunctionCommandHandler : IRequestHandler<UpdateJobFunctionCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly ISqlService _sqlService;

        public UpdateJobFunctionCommandHandler(IApplicationDbContext context, IIdentityService identityService, ISqlService sqlService)
        {
            _context = context;
            _identityService = identityService;
            _sqlService = sqlService;
        }

        public async Task<Result> Handle(UpdateJobFunctionCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _identityService.GetUserByIdAndOrganization(request.UserId,request.OrganizationId);
                if (result.staff == null)
                {
                    return Result.Failure(new string[] { "Invalid User" });

                }
                var job = await _context.JobFunctions.FirstOrDefaultAsync(x => x.Id == request.Id);
                job.Name = request.Name;
                job.LastModifiedById = request.UserId;

                _context.JobFunctions.Update(job);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Job function update successful",job);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Job function update was not successful", ex?.Message + ex?.InnerException.Message });
            }

        }

    }

}
