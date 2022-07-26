using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.ControlPropertyItems.Queries;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.ControlPropertyItems.Commands
{
    public class UpdateControlPropertyItemCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int ControlPropertyId { get; set; }
        public int Index { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public bool IsDefaultValue { get; set; }

        public string UserId { get; set; }
    }

    public class UpdateSubscriptionControlCommandHandler : IRequestHandler<UpdateControlPropertyItemCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UpdateSubscriptionControlCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdateControlPropertyItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var UpdatedEntityExists = await _context.ControlPropertyItems
                       .AnyAsync(x => x.SubscriberId == request.SubscriberId && x.ControlPropertyId == request.ControlPropertyId 
                       && (x.Value == request.Value || x.Index == request.Index));

                if (UpdatedEntityExists)
                {
                    return Result.Failure($"The item value '{request.Value}' is already configured for this control property.");
                }

                var entity = await _context.ControlPropertyItems.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId
                && x.ControlPropertyId == request.ControlPropertyId && x.Id == request.Id);

                if (entity == null)
                {
                    return Result.Failure($"Invalid control property item specified.");
                }

                entity.Name = request.Value;
                entity.Description = request.Description;
                entity.SubscriberName = _authService.Subscriber?.Name;
                entity.ControlPropertyId = request.ControlPropertyId;
                entity.Index = request.Index;
                entity.Value = request.Value;
                entity.IsDefaultValue = request.IsDefaultValue;

                entity.UserId = request.UserId;
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                _context.ControlPropertyItems.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<ControlPropertyItemDto>(entity);
                return Result.Success("Control property item update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Control property item update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

    }


}
