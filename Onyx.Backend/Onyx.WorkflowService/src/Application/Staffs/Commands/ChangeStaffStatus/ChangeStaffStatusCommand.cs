using Onyx.WorkFlowService.Application.Common.Exceptions;
using Onyx.WorkFlowService.Application.Common.Interfaces;
using Onyx.WorkFlowService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Onyx.WorkFlowService.Application.Common.Models;
using System;
using AutoMapper;

namespace Onyx.WorkFlowService.Application.Staffs.Commands.ChangeStaffStatus
{
    public class ChangeStaffStatusCommand :  IRequest<Result>
    {
        public int OrganizationId { get; set; }
        public string UserId { get; set; }
        public string StaffId { get; set; }
       
    }

    public class ChangeStaffStatusCommandHandler : IRequestHandler<ChangeStaffStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public ChangeStaffStatusCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<Result> Handle(ChangeStaffStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _identityService.GetUserByIdAndOrganization(request.UserId, request.OrganizationId);
                if (user.staff == null)
                {
                    return Result.Failure(new string[] { "Unable to change status.Invalid User ID and Organization credentials!" });
                }
                var userStatusForChange = await _identityService.GetUserById(request.StaffId);
                if (userStatusForChange.staff == null)
                {
                    return Result.Failure(new string[] { "Invalid Staff for status change" });
                }
                userStatusForChange.staff.StaffId = request.StaffId;
               var result =  await _identityService.ChangeStaffStatusAsync(userStatusForChange.staff);
                await _context.SaveChangesAsync(cancellationToken);
                if (result.Succeeded)
                    return Result.Success(result.Messages[0]);
                else
                    return Result.Failure(result.Messages);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Staff status change was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
