using AutoMapper;
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

namespace OnyxDoc.FormBuilderService.Application.ControlProperties.Queries
{
    public class GetControlPropertyByIdQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
    }

    public class GetSubscriptionControlByIdQueryHandler : IRequestHandler<GetControlPropertyByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetSubscriptionControlByIdQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetControlPropertyByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                /*var SubscriptionCurrency = await _context.ControlProperties.FirstOrDefaultAsync(a => a.SubscriberId == request.SubscriberId 
                && a.ControlId ==  request.ControlId  && a.Id == request.Id);*/
                var subscriptionControl = await _context.ControlProperties
                    .Where(a => a.SubscriberId == request.SubscriberId && a.Id == request.Id) 
                    .Include(b => b.ControlPropertyItems) 
                    .FirstOrDefaultAsync();
                if (subscriptionControl == null)
                {
                    return Result.Failure("Invalid control property unique identifier!");
                }

                var result = _mapper.Map<ControlPropertyDto>(subscriptionControl);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while getting control property. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
