using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.PageControlItemProperties;
using OnyxDoc.FormBuilderService.Application.PageControlItemProperties.Commands;
using OnyxDoc.FormBuilderService.Application.PageControlItems.Queries;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.PageControlItems.Commands
{
    public class UpdatePageControlItemCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public int DocumentPageId { get; set; }
        public int ControlId { get; set; }

        #region Page ControlItem Dimension
        public string Height { get; set; }
        public string Width { get; set; }
        public string Position { get; set; }
        public string Transform { get; set; }
        #endregion

        public List<UpdatePageControlItemPropertyRequest> PageControlItemProperties { get; set; }

        public string UserId { get; set; }
    }

    public class UpdateSubscriptionControlCommandHandler : IRequestHandler<UpdatePageControlItemCommand, Result>
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
        public async Task<Result> Handle(UpdatePageControlItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);
               
                var entity = await _context.PageControlItems.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId
                && x.DocumentPageId == request.DocumentPageId && x.Id == request.Id);

                if (entity == null)
                {
                    return Result.Failure($"Invalid page control iten specified.");
                }

                entity.ControlId = request.ControlId;
                entity.Height = request.Height;
                entity.Width = request.Width;
                entity.Transform = request.Transform;
                entity.Position = request.Position;

                entity.UserId = request.UserId;
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                if (request.PageControlItemProperties != null && request.PageControlItemProperties.Count > 0)
                {
                    var command = new UpdatePageControlItemPropertiesCommand
                    {
                        SubscriberId = request.SubscriberId,
                        SubscriberName = _authService.Subscriber?.Name,
                        PageControlItemId = entity.Id,
                        PageControlItemProperties = request.PageControlItemProperties,
                        UserId = request.UserId,
                        AccessToken = request.AccessToken
                    };
                    var helper = new PageControlItemPropertyHelper(_context, _mapper, _authService);
                    var pciProperties = await helper.GetPageControlItemProperties(command, cancellationToken);
                    //entity.PageControlItemProperties.AddRange(pciProperties);
                    entity.PageControlItemProperties = (pciProperties);

                    #region Do not delete!
                    //var handler = new UpdatePageControlItemPropertiesCommandHandler(_context, _mapper, _authService);
                    //var subscriptionFeatureResult = await handler.Handle(command, cancellationToken);
                    ////  var recipientsResult = _mediator.Send(command).Result;
                    //if (subscriptionFeatureResult.Succeeded == false)
                    //{
                    //    throw new Exception(subscriptionFeatureResult.Error + subscriptionFeatureResult.Message);
                    //}
                    #endregion
                }

                _context.PageControlItems.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<PageControlItemDto>(entity);
                return Result.Success("Page control item update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Page control item update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

    }


}
