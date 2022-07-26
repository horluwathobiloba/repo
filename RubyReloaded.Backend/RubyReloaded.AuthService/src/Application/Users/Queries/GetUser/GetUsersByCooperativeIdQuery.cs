using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.User.Queries.GetUser
{
    public class GetUsersByCooperativeIdQuery : IRequest<Result>
    {
        public int CooperativeId { get; set; }
    }

    public class GetUsersByCooperativeIdQueryHandler : IRequestHandler<GetUsersByCooperativeIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly IBase64ToFileConverter _fileConverter;

        public GetUsersByCooperativeIdQueryHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper, IBase64ToFileConverter fileConverter)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
            _fileConverter = fileConverter;
        }

        public async Task<Result> Handle(GetUsersByCooperativeIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.CooperativeMembers
                    .Where(x => x.CooperativeId == request.CooperativeId)
                    .ToListAsync();

                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Operation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }


    }
}
