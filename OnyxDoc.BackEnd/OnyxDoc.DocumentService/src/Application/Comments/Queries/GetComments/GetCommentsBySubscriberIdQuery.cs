using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Comments.Queries
{
    public class GetCommentsBySubscriberIdQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
    }
    public class GetCommentsBySubscriberIdQueryHandler : IRequestHandler<GetCommentsBySubscriberIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        public GetCommentsBySubscriberIdQueryHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(GetCommentsBySubscriberIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
               // var respo = await _authService.ValidateSubscriberanisationData(request.AccessToken, request.SubscriberanisationId);
                var entity = await _context.Comments.Include(a=>a.Coordinate).Where(x=> x.SubscriberId == request.SubscriberId
                                                               && x.Status == Domain.Enums.Status.Active).ToListAsync();
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
