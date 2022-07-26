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

namespace OnyxDoc.FormBuilderService.Application.PageControlItems.Queries
{
    public class GetPageControlItemByIdQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int DocumentId { get; set; } 
        public int Id { get; set; }
        public string UserId { get; set; }
    }

    public class GetSubscriptionControlByIdQueryHandler : IRequestHandler<GetPageControlItemByIdQuery, Result>
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

        public async Task<Result> Handle(GetPageControlItemByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);

                var entity = await _context.PageControlItems.Where(a => a.SubscriberId == request.SubscriberId 
                && a.DocumentPage.DocumentId ==  request.DocumentId  &&  a.Id == request.Id).Include(x=>x.PageControlItemProperties).ThenInclude(x => x.PageControlItemPropertyValues).FirstOrDefaultAsync();

                if (entity == null)
                {
                    return Result.Failure("No record found!");
                }

                var result = _mapper.Map<PageControlItemDto>(entity);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving page control item. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
