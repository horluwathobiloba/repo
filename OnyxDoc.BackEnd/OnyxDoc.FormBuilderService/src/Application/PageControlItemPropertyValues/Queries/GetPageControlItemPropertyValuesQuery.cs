using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.PageControlItemPropertyValues.Queries
{
    public class GetPageControlItemPropertyValuesQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
    }

    public class GetPageControlItemPropertyValuesQueryHandler : IRequestHandler<GetPageControlItemPropertyValuesQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetPageControlItemPropertyValuesQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetPageControlItemPropertyValuesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
 
                var list = await _context.PageControlItemPropertyValues.Where(a => a.SubscriberId == request.SubscriberId).ToListAsync();
                var result = _mapper.Map<List<PageControlItemPropertyValueDto>>(list);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving page control item property values. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
