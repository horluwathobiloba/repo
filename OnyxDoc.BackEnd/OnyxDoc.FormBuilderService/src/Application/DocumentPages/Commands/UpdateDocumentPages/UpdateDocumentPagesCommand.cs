using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Domain.Enums;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.FormBuilderService.Domain.Entities;
using OnyxDoc.FormBuilderService.Application.DocumentPages.Queries;
using FluentValidation.Results;
using OnyxDoc.FormBuilderService.Application.PageControlItems.Commands;
using OnyxDoc.FormBuilderService.Application.PageControlItems;

namespace OnyxDoc.FormBuilderService.Application.DocumentPages.Commands
{
    public class UpdateDocumentPagesCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int DocumentId { get; set; }
        public List<UpdateDocumentPageRequest> DocumentPages { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateDocumentPagesCommandHandler : IRequestHandler<UpdateDocumentPagesCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;


        public UpdateDocumentPagesCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(UpdateDocumentPagesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                var list = new List<DocumentPage>();
                await _context.BeginTransactionAsync();

                var DocumentPage = await _context.Documents
                           .AnyAsync(x => x.SubscriberId == request.SubscriberId && x.Id == request.DocumentId);

                foreach (var item in request.DocumentPages)
                {
                    this.ValidateItem(item);

                    //check if the name of the subscription type already exists and conflicts with this new name 
                    var UpdatedEntityExists = await _context.DocumentPages
                           .AnyAsync(x => x.SubscriberId == request.SubscriberId && x.DocumentId == request.DocumentId && x.Id != item.Id
                       && (x.Name == item.Name || x.DisplayName == item.DisplayName));

                    if (UpdatedEntityExists)
                    {
                        continue;
                        //return Result.Failure($"The Control code '{item.ControlCode}' is already configured for this subscription.");
                    }

                    var entity = await _context.DocumentPages
                        .Where(x => x.SubscriberId == request.SubscriberId && x.DocumentId == request.DocumentId
                        && (x.Id == item.Id || x.Name == item.Name || x.DisplayName == item.DisplayName))
                        .FirstOrDefaultAsync();

                    if (entity == null || item.Id <= 0)
                    {
                        entity = new DocumentPage
                        {
                            Name = item.Name,
                            SubscriberId = item.SubscriberId,
                            SubscriberName = _authService.Subscriber?.Name,
                            DocumentId = request.DocumentId,
                            DisplayName = item.DisplayName,
                            FooterData = item.FooterData,
                            HeaderData = item.HeaderData,
                            PageIndex = item.PageIndex,
                            PageLayout = item.PageLayout,
                            PageNumber = item.PageNumber,
                            Watermark = item.Watermark,

                            Position = item.Position,
                            Transform = item.Transform,
                            Height = item.Height,
                            Width = item.Width,

                            UserId = request.UserId,
                            CreatedBy = request.UserId,
                            CreatedDate = DateTime.Now,
                            LastModifiedBy = request.UserId,
                            LastModifiedDate = DateTime.Now,
                            Status = Status.Active,
                            StatusDesc = Status.Active.ToString()
                        };
                        await _context.DocumentPages.AddAsync(entity);
                    }
                    else
                    {
                        entity.Name = item.Name;
                        entity.SubscriberName = _authService.Subscriber?.Name;
                        entity.DocumentId = item.DocumentId;
                        entity.DisplayName = item.DisplayName;
                        entity.FooterData = item.FooterData;
                        entity.HeaderData = item.HeaderData;
                        entity.PageIndex = item.PageIndex;
                        entity.PageLayout = item.PageLayout;
                        entity.PageNumber = item.PageNumber;
                        entity.Watermark = item.Watermark;

                        entity.Position = item.Position;
                        entity.Transform = item.Transform;
                        entity.Height = item.Height;
                        entity.Width = item.Width;

                        entity.Status = item.Status;
                        entity.StatusDesc = item.Status.ToString();
                        entity.LastModifiedBy = request.UserId;
                        entity.LastModifiedDate = DateTime.Now;
                    }
                    await _context.SaveChangesAsync(cancellationToken);

                    if (item.PageControlItems != null && item.PageControlItems.Count > 0)
                    {
                        var command = new UpdatePageControlItemsCommand
                        {
                            SubscriberId = request.SubscriberId,
                            SubscriberName = _authService.Subscriber?.Name,
                            DocumentPageId = entity.Id,
                            PageControlItems = item.PageControlItems,
                            UserId = request.UserId,
                            AccessToken = request.AccessToken
                        };
                        var helper = new PageControlItemHelper(_context, _mapper, _authService);
                        var pciProperties = await helper.GetPageControlItems(command, cancellationToken);
                        entity.PageControlItems.AddRange(pciProperties);
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
                    list.Add(entity);
                }

                _context.DocumentPages.UpdateRange(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<DocumentPageDto>>(list);
                return Result.Success("Document pages update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Document page update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

        private void ValidateItem(UpdateDocumentPageRequest item)
        {
            UpdateDocumentPageRequestValidator validator = new UpdateDocumentPageRequestValidator();

            ValidationResult validateResult = validator.Validate(item);
            string validateError = null;

            if (!validateResult.IsValid)
            {
                foreach (var failure in validateResult.Errors)
                {
                    validateError += "Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage + "\n";
                }
                throw new Exception(validateError);
            }
        }


    }


}
