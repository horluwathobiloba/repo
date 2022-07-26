using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.ControlPropertyItems.Queries;
using OnyxDoc.FormBuilderService.Domain.Entities;
using OnyxDoc.FormBuilderService.Domain.Enums;
using ReventInject;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.ControlPropertyItems.Commands
{
    public class CreateControlPropertyItemCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int ControlPropertyId { get; set; }
        public int Index { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public bool IsDefaultValue { get; set; }

        public string UserId { get; set; }
    }

    public class CreateSubscriptionControlCommandHandler : IRequestHandler<CreateControlPropertyItemCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreateSubscriptionControlCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _authService = authService;
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreateControlPropertyItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var exists = await _context.ControlPropertyItems.AnyAsync(a => a.SubscriberId == request.SubscriberId
                && a.ControlPropertyId == request.ControlPropertyId && (a.Index == request.Index || a.Value == request.Value));

                if (exists)
                {
                    return Result.Failure($"Control property item '{request.Value}' already exists.");
                }

                ControlProperty ControlProperty = await _context.ControlProperties.FirstOrDefaultAsync(a => a.Id == request.ControlPropertyId);

                var entity = new ControlPropertyItem
                {
                    Name = request.Value,
                    Description = request.Description,
                    SubscriberId = request.SubscriberId,
                    SubscriberName = _authService.Subscriber?.Name,
                    ControlPropertyId = request.ControlPropertyId,
                    Index = request.Index,
                    Value = request.Value,
                    IsDefaultValue = request.IsDefaultValue,

                    UserId = request.UserId,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.ControlPropertyItems.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<ControlPropertyItemDto>(entity);
                return Result.Success("Control property item created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Control property item creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
