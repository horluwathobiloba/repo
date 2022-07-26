using Onyx.AuthService.Application.Common.Exceptions;
using Onyx.AuthService.Application.Common.Interfaces;
using Onyx.AuthService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Onyx.AuthService.Application.Common.Models;
using System;
using AutoMapper;

namespace Onyx.AuthService.Application.Users.Commands.ChangeUserStatus
{
    public class ChangeUserStatusCommand :  IRequest<Result>
    {
        public int OrganizationId { get; set; }
        public string UserId { get; set; }
       
    }

    public class ChangeUserStatusCommandHandler : IRequestHandler<ChangeUserStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public ChangeUserStatusCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<Result> Handle(ChangeUserStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _identityService.GetUserByIdAndOrganization(request.UserId, request.OrganizationId);
                if (user.staff == null)
                {
                    return Result.Failure(new string[] { "Unable to change status.Invalid User ID and Organization credentials!" });
                }
                var userStatusForChange = await _identityService.GetUserById(request.UserId);
                if (userStatusForChange.staff == null)
                {
                    return Result.Failure(new string[] { "Invalid User for status change" });
                }
                userStatusForChange.staff.UserId = request.UserId;
               var result =  await _identityService.ChangeUserStatusAsync(userStatusForChange.staff);
                await _context.SaveChangesAsync(cancellationToken);
                if (result.Succeeded)
                    return Result.Success(result.Messages[0]);
                else
                    return Result.Failure(result.Messages);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "User status change was not successful", ex?.Message + ex?.InnerException.Message });
            }
        }
    }
}
