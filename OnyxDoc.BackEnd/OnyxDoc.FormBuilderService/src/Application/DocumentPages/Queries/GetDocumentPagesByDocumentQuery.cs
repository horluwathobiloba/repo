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

namespace OnyxDoc.FormBuilderService.Application.DocumentPages.Queries
{
    public class GetDocumentPagesByDocumentQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int DocumentId { get; set; }
        public string UserId { get; set; }
    }
    public class GetDocumentPagesByFeatureIdQueryHandler : IRequestHandler<GetDocumentPagesByDocumentQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        public GetDocumentPagesByFeatureIdQueryHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(GetDocumentPagesByDocumentQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);

                /*var entity = await _context.DocumentPages.Where(x => x.SubscriberId == request.SubscriberId && x.DocumentId == request.SubscriberId).ToListAsync();*/
                var entity = await _context.DocumentPages.Where(x => x.SubscriberId == request.SubscriberId && x.DocumentId == request.DocumentId).ToListAsync();

                if (entity == null || entity.Count == 0)
                {
                    return Result.Failure("No record found");
                }
                return Result.Success($"{entity.Count}(s) found.", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get document pages failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
