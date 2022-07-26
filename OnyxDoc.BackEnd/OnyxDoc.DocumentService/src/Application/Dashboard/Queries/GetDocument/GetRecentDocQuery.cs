using AutoMapper;
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

namespace OnyxDoc.DocumentService.Application.Dashboard.Queries.GetDocument
{
    public class GetRecentDocQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
    }

    public class GetRecentDocQueryHandler : IRequestHandler<GetRecentDocQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public GetRecentDocQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetRecentDocQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                if (_authService.Subscriber == null)
                {
                    return Result.Failure(new string[] { "Invalid subscriber details specified." });
                }
                var entity = await _context.Documents.Where(x => x.SubscriberId == request.SubscriberId && x.CreatedBy == request.UserId).OrderByDescending(c => c.CreatedDate).Take(4).ToListAsync();
                if(entity == null || entity.Count == 0)
                {
                    List<Domain.Entities.Document> emptyArray = new List<Domain.Entities.Document>();
                    return Result.Success("No documents available", emptyArray);
                }
                var result = _mapper.Map<List<RecentDocDto>>(entity);
                return Result.Success($"{entity.Count}(s) found.", result);
            }
            catch (Exception ex)
            {

                return Result.Failure($"Get Recent Documents failed. Error: { ex?.Message + " " + ex?.InnerException.Message}");
            }
        }
    }
}
