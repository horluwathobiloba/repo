using MediatR;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Application.Common.Models;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.AuthService.Application.Authentication
{
    public partial class ApplicationLoginCommand : IRequest<AuthResult>
    {
        public string Name { get; set; }
    }

    public class DeveloperLoginCommandHandler : IRequestHandler<ApplicationLoginCommand, AuthResult>
    {
        private readonly IApplicationDbContext _context;

        private readonly IAuthenticateService _authenticationService;

        public DeveloperLoginCommandHandler(IApplicationDbContext context, IAuthenticateService authenticationService)
        {
            _context = context;
            _authenticationService = authenticationService;
        }

        public async Task<AuthResult> Handle(ApplicationLoginCommand request, CancellationToken cancellationToken)
        {
            return await _authenticationService.ApplicationLogin(request.Name,cancellationToken);
        }
    }
}
