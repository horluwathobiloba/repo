﻿using AutoMapper;
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

namespace OnyxDoc.FormBuilderService.Application.PageControlItems.Queries
{
    public class GetActivePageControlItemsQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int DocumentPageId { get; set; } 
        public string UserId { get; set; }
    }

    public class GetActivePageControlItemsQueryHandler : IRequestHandler<GetActivePageControlItemsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetActivePageControlItemsQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetActivePageControlItemsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);

                var list = await _context.PageControlItems.Where(a => a.SubscriberId == request.SubscriberId && a.DocumentPageId == request.DocumentPageId && a.Status == Status.Active).ToListAsync();
                var result = _mapper.Map<List<PageControlItemDto>>(list);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving page control items. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
