using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Comments.Queries
{
    public class GetCommentsByTypeQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int DocumentId { get; set; }
        public CommentType CommentType { get; set; }
    }
    public class GetCommentsByTypeQueryHandler : IRequestHandler<GetCommentsByTypeQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        public GetCommentsByTypeQueryHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(GetCommentsByTypeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                //var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var entity = await _context.Comments.Include(a=>a.Coordinate).Where(x=> x.SubscriberId == request.SubscriberId && x.DocumentId == request.DocumentId
                                                               && x.Status == Domain.Enums.Status.Active && x.CommentType == request.CommentType ).ToListAsync();
                if (entity == null || entity.Count == 0)
                {
                    return Result.Failure("No contract comments available");
                }
                return Result.Success($"{entity.Count}(s) found.", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get contract comments failed. Error: { ex?.Message +" "+ ex?.InnerException.Message}");
            }
        }
    }
}
