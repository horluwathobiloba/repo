using Onyx.WorkFlowService.Application.Common.Exceptions;
using Onyx.WorkFlowService.Application.Common.Interfaces;
using Onyx.WorkFlowService.Domain.Entities;
using Onyx.WorkFlowService.Domain.Enums;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Onyx.WorkFlowService.Application.Common.Models;
using System;

namespace Onyx.WorkFlowService.Application.Authentication.Commands.ChangePassword
{
    public partial class ChangePasswordCommand :  IRequest<Result>
    {
        public string Username { get; set; }
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }

    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        private readonly IAuthenticateService _authenticationService;

        public ChangePasswordCommandHandler(IApplicationDbContext context, IAuthenticateService authenticationService)
        {
            _context = context;
            _authenticationService = authenticationService;
        }

        public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.OldPassword == request.NewPassword)
                {
                    return Result.Failure(new string[] { "Error!, Old Password is same as New Password" });
                }
                return await _authenticationService.ChangePassword(request.Username, request.OldPassword, request.NewPassword);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { " Error changing password : " + ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
