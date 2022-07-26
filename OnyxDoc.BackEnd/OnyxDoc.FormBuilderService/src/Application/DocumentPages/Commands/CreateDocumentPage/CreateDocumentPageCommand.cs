using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.DocumentPages.Queries;
using OnyxDoc.FormBuilderService.Application.PageControlItems;
using OnyxDoc.FormBuilderService.Application.PageControlItems.Commands;
using OnyxDoc.FormBuilderService.Domain.Entities;
using OnyxDoc.FormBuilderService.Domain.Enums;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.DocumentPages.Commands
{
    public class CreateDocumentPageCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Watermark { get; set; }
        public string PageTips { get; set; }
        public int DocumentId { get; set; }
        public int PageIndex { get; set; }
        public int PageNumber { get; set; }
        public PageLayout PageLayout { get; set; }

        public List<UpdatePageControlItemRequest> PageControlItems { get; set; }

        #region Page Dimension
        public string Height { get; set; }
        public string Width { get; set; }
        public string Position { get; set; }
        public string Transform { get; set; }
        #endregion

        public string HeaderData { get; set; }
        public string FooterData { get; set; }

        public string UserId { get; set; }
    }

    public class CreateSubscriptionControlCommandHandler : IRequestHandler<CreateDocumentPageCommand, Result>
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

        public async Task<Result> Handle(CreateDocumentPageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var exists = await _context.DocumentPages.AnyAsync(a => a.SubscriberId == request.SubscriberId
                && a.DocumentId == request.DocumentId && (a.Name == request.Name || a.DisplayName == request.DisplayName));

                if (exists)
                {
                    return Result.Failure($"Document page named '{request.Name}' or with display name '{request.DisplayName}' already exists.");
                }

                Document DocumentPage = await _context.Documents.FirstOrDefaultAsync(a => a.Id == request.DocumentId);

                var entity = new DocumentPage
                {
                    Name = request.Name,
                    SubscriberId = request.SubscriberId,
                    SubscriberName = _authService.Subscriber?.Name,
                    DocumentId = request.DocumentId,
                    DisplayName = request.DisplayName,
                    FooterData = request.FooterData,
                    HeaderData = request.HeaderData,
                    PageIndex = request.PageIndex,
                    PageLayout = request.PageLayout,
                    PageNumber = request.PageNumber,

                    Position = request.Position,
                    Transform = request.Transform,
                    Height = request.Height,
                    Width = request.Width,

                    Watermark = request.Watermark,
                    PageTips = request.PageTips,

                    UserId = request.UserId,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.BeginTransactionAsync();
                await _context.DocumentPages.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                if (request.PageControlItems != null && request.PageControlItems.Count > 0)
                {
                    var command = new UpdatePageControlItemsCommand
                    {
                        SubscriberId = request.SubscriberId,
                        SubscriberName = _authService.Subscriber?.Name,
                        DocumentPageId = entity.Id,
                        PageControlItems = request.PageControlItems,
                        UserId = request.UserId,
                        AccessToken = request.AccessToken
                    };
                    var helper = new PageControlItemHelper(_context, _mapper, _authService);
                    var pciProperties = await helper.GetPageControlItems(command, cancellationToken);
                    entity.PageControlItems = pciProperties;

                    #region Do not delete!
                    //var handler = new UpdatePageControlItemsCommandHandler(_context, _mapper, _authService);
                    //var pciResult = await handler.Handle(command, cancellationToken);
                    ////  var pciResult = _mediator.Send(command).Result;
                    //if (pciResult.Succeeded == false)
                    //{
                    //    throw new Exception(pciResult.Error + pciResult.Message);
                    //}
                    #endregion
                }
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<DocumentPageDto>(entity);
                return Result.Success("Document page created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Document page creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
