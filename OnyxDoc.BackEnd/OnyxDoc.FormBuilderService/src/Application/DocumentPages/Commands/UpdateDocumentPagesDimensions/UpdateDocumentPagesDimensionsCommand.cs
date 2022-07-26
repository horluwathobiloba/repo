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
    public class UpdateDocumentPagesDimensionsCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int DocumentId { get; set; }
        public List<UpdateDocumentPageDimensionRequest> DocumentPages { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateDocumentPagesDimensionsCommandHandler : IRequestHandler<UpdateDocumentPagesDimensionsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;


        public UpdateDocumentPagesDimensionsCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(UpdateDocumentPagesDimensionsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                var list = new List<DocumentPage>();
                await _context.BeginTransactionAsync();

                var document = await _context.Documents.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId && x.Id == request.DocumentId);

                if (document == null || document.Id == 0)
                {
                    return Result.Failure($"Invalid document specified.");
                }

                foreach (var item in request.DocumentPages)
                {
                    this.ValidateItem(item);

                    var entity = await _context.DocumentPages
                        .Where(x => x.SubscriberId == request.SubscriberId && x.DocumentId == request.DocumentId && x.Id == item.Id)
                        .FirstOrDefaultAsync();

                    if (entity == null)
                    {
                        return Result.Failure($"Invalid document page specified.");
                    }

                    entity.Position = item.Position;
                    entity.Transform = item.Transform;
                    entity.Height = item.Height;
                    entity.Width = item.Width;

                    entity.UserId = request.UserId;
                    entity.LastModifiedBy = request.UserId;
                    entity.LastModifiedDate = DateTime.Now;

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
                return Result.Success("Document pages dimensions update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Document pages dimensions update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

        private void ValidateItem(UpdateDocumentPageDimensionRequest item)
        {
            UpdateDocumentPageDimensionRequestValidator validator = new UpdateDocumentPageDimensionRequestValidator();

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
