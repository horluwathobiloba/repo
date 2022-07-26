using AutoMapper;
using MediatR;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.User.Queries.GetUser
{
    public class GetUserByEmailQuery:IRequest<Result>
    {
        public string Email { get; set; }
    }
    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly IBase64ToFileConverter _fileConverter;

        public GetUserByEmailQueryHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper, IBase64ToFileConverter fileConverter)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
            _fileConverter = fileConverter;
        }

        public async Task<Result> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _identityService.GetUserByEmail(request.Email);
                //result.user.ProfilePicture = _fileConverter.RetrieveFileUrl(result.user.ProfilePicture);
                var user = _mapper.Map<UserVm>(result.user);
                return Result.Success(user);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Operation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
