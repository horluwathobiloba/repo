using Onyx.WorkFlowService.Application.Common.Interfaces;
using Onyx.WorkFlowService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Onyx.WorkFlowService.Application.Common.Models;
using System;

namespace Onyx.WorkFlowService.Application.Authentication.Commands.LogOut
{
    public partial class LogoutCommand :  IRequest<Result>
    {
        public string Email { get; set; }
    }

    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        private readonly IAuthenticateService _authenticationService;

        public LogoutCommandHandler(IApplicationDbContext context, IAuthenticateService authenticationService)
        {
            _context = context;
            _authenticationService = authenticationService;
        }

        public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _authenticationService.LogOut(request.Email);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { " Error logging out : " + ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}