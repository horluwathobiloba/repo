using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.ControlPropertyItems.Queries
{
    public class GetControlPropertyItemsByControlPropertyQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int ControlPropertyId { get; set; }
        public string UserId { get; set; }
    }
    public class GetControlPropertyItemsByControlPropertyQueryHandler : IRequestHandler<GetControlPropertyItemsByControlPropertyQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        public GetControlPropertyItemsByControlPropertyQueryHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(GetControlPropertyItemsByControlPropertyQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);

                var entity = await _context.ControlPropertyItems.Where(x => x.SubscriberId == request.SubscriberId && x.ControlPropertyId == request.SubscriberId).ToListAsync();

                if (entity == null || entity.Count == 0)
                {
                    return Result.Failure("No record found");
                }
                return Result.Success($"{entity.Count}(s) found.", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get control property item failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
