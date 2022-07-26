using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.PageControlItemProperties;
using OnyxDoc.FormBuilderService.Application.PageControlItemProperties.Commands;
using OnyxDoc.FormBuilderService.Application.PageControlItems.Queries;
using OnyxDoc.FormBuilderService.Domain.Entities;
using OnyxDoc.FormBuilderService.Domain.Enums;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.PageControlItems.Commands
{
    public class CreatePageControlItemCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int DocumentPageId { get; set; }
        public int ControlId { get; set; }

        #region Value captured based on the control's input value type
        public string TextValue { get; set; }
        public long NumberValue { get; set; }
        public decimal FloatValue { get; set; }
        public bool BooleanValue { get; set; }
        public DateTime DateTimeValue { get; set; }
        public string BlobValue { get; set; }
        #endregion

        #region Page ControlItem Dimension
        public string Height { get; set; }
        public string Width { get; set; }
        public string Position { get; set; }
        public string Transform { get; set; }
        #endregion

        public List<UpdatePageControlItemPropertyRequest> PageControlItemProperties { get; set; }

        public string UserId { get; set; }
    }

    public class CreateSubscriptionControlCommandHandler : IRequestHandler<CreatePageControlItemCommand, Result>
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

        public async Task<Result> Handle(CreatePageControlItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                //There is no need for this check since controls of the same type can be created within a page.
                //var exists = await _context.PageControlItems.AnyAsync(a => a.SubscriberId == request.SubscriberId
                //&& a.DocumentPageId == request.DocumentPageId && a.ControlId == request.ControlId && a.Name == request.Name);

                //if (exists)
                //{
                //    return Result.Failure($"Page control item '{request.Name}' already exists.");
                //}

                DocumentPage documentPage = await _context.DocumentPages.FirstOrDefaultAsync(a => a.Id == request.DocumentPageId);
                Control control = await _context.Controls.FirstOrDefaultAsync(a => a.Id == request.ControlId);

                if (documentPage == null)
                {
                    throw new Exception("Document page specified was not found!");
                }
                if (control == null)
                {
                    throw new Exception("Control specified was not found!");
                }

                var entity = new PageControlItem
                {
                    SubscriberId = request.SubscriberId,
                    SubscriberName = _authService.Subscriber?.Name,
                    DocumentPageId = request.DocumentPageId,
                    ControlId = request.ControlId,
                    Height = request.Height,
                    Width = request.Width,
                    Transform = request.Transform,
                    Position = request.Position,

                    UserId = request.UserId,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.BeginTransactionAsync();
                await _context.PageControlItems.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

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
                    entity.PageControlItemProperties = pciProperties;

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

                //create the logical name for the page control item and update the entity
                entity.Name = $"{documentPage.Name}_{control.Name}_{entity.Id}";
                entity.Description = $"{documentPage.Name} {control.Name} {entity.Id}";
                var pageControlPropertiesEntityCheck = entity.PageControlItemProperties;
                await _context.SaveChangesAsync(cancellationToken);

                await _context.CommitTransactionAsync();
                var result = _mapper.Map<PageControlItemDto>(entity);
                return Result.Success("Page control item created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Page control item creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
