using AutoMapper;
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
    public class GetActiveDocumentPagesQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int DocumentId { get; set; }
        public string UserId { get; set; }
    }

    public class GetActiveDocumentPagesQueryHandler : IRequestHandler<GetActiveDocumentPagesQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetActiveDocumentPagesQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetActiveDocumentPagesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);

                var list = await _context.DocumentPages.Where(a => a.SubscriberId == request.SubscriberId && a.DocumentId == request.DocumentId && a.Status == Status.Active).ToListAsync();
                var result = _mapper.Map<List<DocumentPageDto>>(list);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving document pages. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
