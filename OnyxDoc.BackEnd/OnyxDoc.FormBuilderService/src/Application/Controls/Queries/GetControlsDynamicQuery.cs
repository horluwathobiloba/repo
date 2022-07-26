﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Exceptions;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ReventInject;

namespace OnyxDoc.FormBuilderService.Application.Controls.Queries
{
    public class GetControlsDynamicQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SearchText { get; set; }
        public string UserId { get; set; }
    }

    public class GetControlsDynamicQueryHandler : IRequestHandler<GetControlsDynamicQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetControlsDynamicQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetControlsDynamicQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                var list = await _context.Controls.Include(a => a.ControlProperties).Where(a => a.SubscriberId == request.SubscriberId).ToListAsync();

                if (!string.IsNullOrEmpty(request.SearchText))
                {
                    list = list.Where(a => request.SearchText.IsIn(a.Name)
                    || request.SearchText.IsIn(a.SubscriberName)
                    || request.SearchText.IsIn(a.ControlTypeDesc)).ToList();
                }

                if (list == null)
                {
                    throw new NotFoundException(nameof(Control));
                }
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
