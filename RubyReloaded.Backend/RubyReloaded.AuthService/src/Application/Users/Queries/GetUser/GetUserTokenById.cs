using MediatR;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Users.Queries.GetUser
{
    public class GetUserTokenById:IRequest<Result>
    {
        public string Id { get; set; }
    }

    public class GetUserTokenByIdHandler : IRequestHandler<GetUserTokenById, Result>
    {
        private readonly ITokenService _tokenService;
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IBase64ToFileConverter _fileConverter;

        public GetUserTokenByIdHandler(IApplicationDbContext context, IIdentityService identityService, IBase64ToFileConverter fileConverter, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
            _identityService = identityService;
            _fileConverter = fileConverter;
        }

        public async Task<Result> Handle(GetUserTokenById request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _identityService.GetUserById(request.Id);
                var result = await _tokenService.GenerateUserToken(user.user);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Operation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
