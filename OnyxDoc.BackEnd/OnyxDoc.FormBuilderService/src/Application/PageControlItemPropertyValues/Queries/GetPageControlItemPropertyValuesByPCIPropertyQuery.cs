using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Domain.Enums;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.PageControlItemPropertyValues.Queries
{
    public class GetPageControlItemPropertyValuesByPCIPropertyQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int PageControlItemPropertyId { get; set; }
        public string UserId { get; set; }
    }

    public class GetPageControlItemPropertyValuesByCurrencyQueryHandler : IRequestHandler<GetPageControlItemPropertyValuesByPCIPropertyQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        public GetPageControlItemPropertyValuesByCurrencyQueryHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(GetPageControlItemPropertyValuesByPCIPropertyQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var list = await _context.PageControlItemPropertyValues
                    .Where(x => x.SubscriberId == request.SubscriberId
                    && x.PageControlItemPropertyId == request.PageControlItemPropertyId).ToListAsync();

                if (list == null || list.Count == 0)
                {
                    return Result.Failure("No record found!");
                }
                return Result.Success($"{list.Count}(s) found.", list);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get page control item property values failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
