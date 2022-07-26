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
    public class GetActiveBrandingBySubscriberIdQuery : IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
    }


    public class GetActiveBrandingBySubscriberIdQueryHandler : IRequestHandler<GetActiveBrandingBySubscriberIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;

        public GetActiveBrandingBySubscriberIdQueryHandler(IApplicationDbContext context, IMapper mapper, IIdentityService identityService)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
        }

        public async Task<Result> Handle(GetActiveBrandingBySubscriberIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var userCheck = await _identityService.GetUserByIdAndSubscriber(request.UserId, request.SubscriberId);

                if (userCheck.user == null)
                {
                    return Result.Failure("Invalid Subscriber and User Specified");
                }
                var entity = await _context.Brandings.Where(a =>  a.SubscriberId == request.SubscriberId && a.Status == Domain.Enums.Status.Active)
                    .FirstOrDefaultAsync();
                if (entity == null)
                {
                    throw new NotFoundException(nameof(Branding));
                }
                var result = _mapper.Map<BrandingDto>(entity);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error retrieving branding by subscriber id {ex?.Message ?? ex?.InnerException?.Message}");
            }

        }
    }
}
