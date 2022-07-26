using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.DocumentPages.Queries;
using OnyxDoc.FormBuilderService.Application.PageControlItems;
using OnyxDoc.FormBuilderService.Application.PageControlItems.Commands;
using OnyxDoc.FormBuilderService.Domain.Entities;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.DocumentPages.Commands
{
    public class CreateDocumentPagesCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int DocumentId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string PageTips { get; set; }
        public string Watermark { get; set; }
        public int PageIndex { get; set; }
        public int PageNumber { get; set; }
        public PageLayout PageLayout { get; set; }

        public string HeaderData { get; set; }
        public string FooterData { get; set; }
        public List<CreateDocumentPageRequest> DocumentPages { get; set; }
        public string UserId { get; set; }
    }

    public class CreateDocumentPagesCommandHandler : IRequestHandler<CreateDocumentPagesCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreateDocumentPagesCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(CreateDocumentPagesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var list = new List<DocumentPage>();

                await _context.BeginTransactionAsync();

                foreach (var item in request.DocumentPages)
                {
                    this.ValidateItem(item);
                    var exists = await _context.DocumentPages.AnyAsync(a => a.SubscriberId == request.SubscriberId
                && a.DocumentId == request.DocumentId && (a.Name == item.Name || a.DisplayName == item.DisplayName));

                    if (exists)
                    {
                        return Result.Failure($"Document page named '{item.Name}' or with display name '{item.DisplayName}' already exists.");
                    }
                    var entity = new DocumentPage
                    {

                        Name = item.Name,
                        SubscriberId = item.SubscriberId,
                        SubscriberName = _authService.Subscriber?.Name,
                        DocumentId = item.DocumentId,
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
                        /*entity.PageControlItems.AddRange(pciProperties);*/
                        entity.PageControlItems = (pciProperties);

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
                return Result.Success("Document pages created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Document pages creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }

        private void ValidateItem(CreateDocumentPageRequest item)
        {
            CreateDocumentPageRequestValidator validator = new CreateDocumentPageRequestValidator();

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
