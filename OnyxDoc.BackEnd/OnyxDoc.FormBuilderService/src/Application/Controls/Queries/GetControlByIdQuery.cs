﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.Controls.Queries
{
    public class GetControlByIdQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
    }

    public class GetControlByIdQueryHandler : IRequestHandler<GetControlByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetControlByIdQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetControlByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                Control control = await _context.Controls
                    .Where(a => a.SubscriberId == request.SubscriberId && a.Id == request.Id)
                    .Include(a => a.ControlProperties)
                    .ThenInclude(b=> b.ControlPropertyItems)
                    .FirstOrDefaultAsync();
                if (control == null)
                {
                    return Result.Failure("Invalid control Id");
                }
                var result = _mapper.Map<ControlDto>(control);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving control. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
