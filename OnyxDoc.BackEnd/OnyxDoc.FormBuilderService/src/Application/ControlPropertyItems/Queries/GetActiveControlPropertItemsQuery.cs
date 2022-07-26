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

namespace OnyxDoc.FormBuilderService.Application.ControlPropertyItems.Queries
{
    public class GetActiveControlPropertyItemsQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int ControlPropertyId { get; set; }
        public string UserId { get; set; }
    }

    public class GetActiveControlPropertyItemsQueryHandler : IRequestHandler<GetActiveControlPropertyItemsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetActiveControlPropertyItemsQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetActiveControlPropertyItemsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);

                var list = await _context.ControlPropertyItems.Where(a => a.SubscriberId == request.SubscriberId && a.ControlPropertyId == request.ControlPropertyId && a.Status == Status.Active).ToListAsync();
                var result = _mapper.Map<List<ControlPropertyItemDto>>(list);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving control property items. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
