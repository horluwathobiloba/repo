﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.Controls.Queries
{
    public class GetControlsQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
    }

    public class GetControlsQueryHandler : IRequestHandler<GetControlsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetControlsQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetControlsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);

                var list = await _context.Controls.Include(a => a.ControlProperties).Where(a => a.SubscriberId == request.SubscriberId).ToListAsync();                
                var result = _mapper.Map<List<ControlDto>>(list);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving Controls. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
