using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.DocumentPages.Commands;
using OnyxDoc.FormBuilderService.Application.Documents.Queries;
using OnyxDoc.FormBuilderService.Domain.Entities;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OnyxDoc.FormBuilderService.Application.DocumentPages;

namespace OnyxDoc.FormBuilderService.Application.Documents.Commands
{
    public class UpdateDocumentCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public string Watermark { get; set; }
        public decimal VersionNumber { get; set; }
        public DocumentType DocumentType { get; set; }
        public DocumentShareType DocumentShareType { get; set; }

        public List<UpdateDocumentPageRequest> DocumentPages { get; set; }
        public string UserId { get; set; }
        public bool IsHighlighted { get; set; }
    }

    public class UpdateSubscriptionCommandHandler : IRequestHandler<UpdateDocumentCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;

        public UpdateSubscriptionCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
            _configuration = configuration;
        }
        public async Task<Result> Handle(UpdateDocumentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);


                var subscriptionExists = await _context.Documents.AnyAsync(x => x.SubscriberId == request.SubscriberId
                && x.Id != request.Id && (x.Name == request.Name || x.DisplayName == request.DisplayName));
                if (subscriptionExists)
                {
                    return Result.Failure($"A document named {request.Name.ToString()} or with display name {request.DisplayName} already exists.");
                }
                var entity = await _context.Documents.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId && x.Id == request.Id);
                if (entity == null)
                {
                    return Result.Failure($"Invalid document specified.");
                }

                entity.Name = request.Name;
                entity.Description = request.Description;
                entity.DisplayName = request.DisplayName;
                entity.Watermark = request.Watermark;
                entity.VersionNumber = request.VersionNumber;
                entity.SubscriberId = request.SubscriberId;
                entity.SubscriberName = _authService.Subscriber?.SubscriberName;
                entity.DocumentType = request.DocumentType;
                entity.DocumentTypeDesc = request.DocumentType.ToString();
                entity.DocumentShareType = request.DocumentShareType;
                entity.DocumentShareTypeDesc = request.DocumentShareType.ToString();
                entity.DocumentStatus = DocumentStatus.Draft;
                entity.DocumentStatusDesc = DocumentStatus.Draft.ToString();

                entity.UserId = request.UserId;
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                await _context.BeginTransactionAsync();

                if (request.DocumentPages != null && request.DocumentPages.Count > 0)
                {
                    //save the pricings
                    var pricings = request.DocumentPages
                        .OrderBy(a => a.Name)
                        .ThenBy(a => a.IsDeleted).ToList();

                    var command = new UpdateDocumentPagesCommand
                    {
                        SubscriberId = request.SubscriberId,
                        SubscriberName = _authService.Subscriber?.Name,
                        DocumentId = request.Id,
                        DocumentPages = request.DocumentPages,
                        UserId = request.UserId,
                        AccessToken = request.AccessToken,

                    };
                    var helper = new DocumentPageHelper(_context, _mapper, _authService);
                    var documentPages = await helper.GetDocumentPages(command, cancellationToken);
                    entity.DocumentPages = documentPages;

                    #region Do not delete!
                    //var handler = new UpdateDocumentPagesCommandHandler(_context, _mapper, _authService);
                    //var subscriptionFeatureResult = await handler.Handle(command, cancellationToken);
                    ////  var recipientsResult = _mediator.Send(command).Result;
                    //if (subscriptionFeatureResult.Succeeded == false)
                    //{
                    //    throw new Exception(subscriptionFeatureResult.Error + subscriptionFeatureResult.Message);
                    //}
                    #endregion
                }

                _context.Documents.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                var result = _mapper.Map<DocumentDto>(entity);
                return Result.Success("Document update was successful!", result);
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure($"Document update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}

