using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.AuthService.Application.Common.Exceptions;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.AuthService.Application.Branding.Queries.GetBranding
{
    public class GetBrandingByIdQuery : IRequest<Result>
    {
        public int Id { get; set; }
        public string UserId { get; set; }
    }


    public class GetBrandingByIdQueryHandler : IRequestHandler<GetBrandingByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;

        public GetBrandingByIdQueryHandler(IApplicationDbContext context, IMapper mapper, IIdentityService identityService)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
        }

        public async Task<Result> Handle(GetBrandingByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var userCheck = await _identityService.GetUserById(request.UserId);

                if (userCheck.user == null)
                {
                    return Result.Failure("Invalid Subscriber and User Specified");
                }
                var entity = await _context.Brandings.Where(a =>  a.Id == request.Id)
                    .FirstOrDefaultAsync();
                if (entity == null)
                {
                    throw new NotFoundException(nameof(Branding), request.Id);
                }
                var result = _mapper.Map<BrandingDto>(entity);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error retrieving branding by id {ex?.Message ?? ex?.InnerException?.Message}");
            }

        }
    }
}
