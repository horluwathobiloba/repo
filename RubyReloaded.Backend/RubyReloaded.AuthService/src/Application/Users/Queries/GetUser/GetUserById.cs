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
    public class GetUserByIdQuery : IRequest<Result>
    {
        public string Id { get; set; }
    }

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly IBase64ToFileConverter _fileConverter;

        public GetUserByIdQueryHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper, IBase64ToFileConverter fileConverter)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
            _fileConverter = fileConverter;
        }

        public async Task<Result> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _identityService.GetUserById(request.Id);
                //result.user.ProfilePicture = _fileConverter.RetrieveFileUrl(result.user.ProfilePicture);
                //var user = _mapper.Map<UserVm>(result.user);
                return Result.Success(result.user);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Operation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
