using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Exceptions;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.ControlPropertyItems.Queries
{
    public class GetControlPropertyItemByIdQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        /*public int ControlPropertyId { get; set; }*/
        public int Id { get; set; }
        public string UserId { get; set; }
    }

    public class GetSubscriptionControlByIdQueryHandler : IRequestHandler<GetControlPropertyItemByIdQuery, Result>
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

        public async Task<Result> Handle(GetControlPropertyItemByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);

                /*var entity = await _context.ControlPropertyItems.FirstOrDefaultAsync(a => a.SubscriberId == request.SubscriberId */
                /*&& a.ControlPropertyId ==  request.ControlPropertyId  && a.Id == request.Id);*/
                var entity = await _context.ControlPropertyItems.FirstOrDefaultAsync(a => a.SubscriberId == request.SubscriberId
                && a.Id == request.Id);
                if (entity == null)
                {
                    return Result.Failure("Invalid control property item!");
                }

                var result = _mapper.Map<ControlPropertyItemDto>(entity);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving control property item. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
