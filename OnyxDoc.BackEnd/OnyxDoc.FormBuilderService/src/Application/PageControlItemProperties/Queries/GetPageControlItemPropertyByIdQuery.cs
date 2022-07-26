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

namespace OnyxDoc.FormBuilderService.Application.PageControlItemProperties.Queries
{
    public class GetPageControlItemPropertyByIdQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int PageControlItemId { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
    }

    public class GetSubscriptionCurrencyByIdQueryHandler : IRequestHandler<GetPageControlItemPropertyByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetSubscriptionCurrencyByIdQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetPageControlItemPropertyByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var SubscriptionCurrency = await _context.PageControlItemProperties
                    .Where(a => a.SubscriberId == request.SubscriberId && a.PageControlItemId == request.PageControlItemId && a.Id == request.Id)
                    .Include(b => b.PageControlItemPropertyValues)
                    .FirstOrDefaultAsync();
                if (SubscriptionCurrency == null)
                {
                    return Result.Failure("No record found!");
                }

                var result = _mapper.Map<PageControlItemPropertyDto>(SubscriptionCurrency);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving page control item property. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
