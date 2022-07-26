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

namespace OnyxDoc.FormBuilderService.Application.Documents.Queries
{
    public class GetDocumentsByTypeQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; } 
        public DocumentType DocumentType { get; set; } 
        public string UserId { get; set; }
    }
    public class GetDocumentsByTypeQueryHandler : IRequestHandler<GetDocumentsByTypeQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        public GetDocumentsByTypeQueryHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(GetDocumentsByTypeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var entity = await _context.Documents.Include(a => a.DocumentPages)
                    .Where(x => x.SubscriberId == request.SubscriberId
                    && x.DocumentType == request.DocumentType).ToListAsync();

                if (entity == null || entity.Count == 0)
                {
                    return Result.Failure("No Documents available");
                }
                return Result.Success($"{entity.Count}(s) found.", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get Documents failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
