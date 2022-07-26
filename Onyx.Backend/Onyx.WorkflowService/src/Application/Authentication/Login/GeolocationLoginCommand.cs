using Onyx.WorkFlowService.Application.Common.Exceptions;
using Onyx.WorkFlowService.Application.Common.Interfaces;
using Onyx.WorkFlowService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Onyx.WorkFlowService.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Onyx.WorkFlowService.Application.Authentication.Commands.GeolocationLogin
{
    public partial class GeolocationLoginCommand :  IRequest<AuthResult>
    {
        public GeoLocationLogin GeoLocationLogin { get; set; }
    }

    public class GeolocationLoginCommandHandler : IRequestHandler<GeolocationLoginCommand, AuthResult>
    {
        private readonly IApplicationDbContext _context;

        private readonly IAuthenticateService _authenticationService;

        public GeolocationLoginCommandHandler(IApplicationDbContext context, IAuthenticateService authenticationService)
        {
            _context = context;
            _authenticationService = authenticationService;
        }

        public async Task<AuthResult> Handle(GeolocationLoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.GeoLocationLogin.OrganizationCode))
                {
                    return new AuthResult { IsSuccess = false, Message = "Invalid Organization Code" };
                }
                var organization = await _context.Organizations.FirstOrDefaultAsync(a => a.Code == request.GeoLocationLogin.OrganizationCode);
                if (organization == null)
                {
                    return new AuthResult { IsSuccess = false, Message = "Invalid Organization Code" };
                }
                return await _authenticationService.GeoLocationLogin(request.GeoLocationLogin, organization.Id);
            }
            catch (Exception ex)
            {
                return new AuthResult { IsSuccess = false, Message = " Error logging in : "+ ex?.Message ?? ex?.InnerException.Message }; 
            }
        }
    }
}
