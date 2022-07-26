using AutoMapper;
using MediatR;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.User.Queries.GetUser
{
    public class GetAllUsersQuery:IRequest<Result>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }


    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly IBase64ToFileConverter _fileConverter;
        public GetAllUsersQueryHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper, IBase64ToFileConverter fileConverter)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
            _fileConverter = fileConverter;
        }
        public async Task<Result> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _identityService.GetAll(request.Skip,request.Take);
                return Result.Success(result.users);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Operation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
           
        }
    }
}
