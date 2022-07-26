using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using OnyxDoc.FormBuilderService.Domain.ViewModels;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;
using OnyxDoc.FormBuilderService.Domain.Enums;
using OnyxDoc.FormBuilderService.Domain.Entities;
using OnyxDoc.FormBuilderService.Application.DocumentPages.Commands;
using OnyxDoc.FormBuilderService.Application.Documents.Queries;
using OnyxDoc.FormBuilderService.Application.PageControlItems;
using OnyxDoc.FormBuilderService.Application.DocumentPages;

namespace OnyxDoc.FormBuilderService.Application.Documents.Commands
{
    public class CreateDocumentCommand : AuthToken, IRequest<Result>
    {
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
    }

    public class CreateSubscriptionCommandHandler : IRequestHandler<CreateDocumentCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IAuthService _authService;
        private IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public CreateSubscriptionCommandHandler(IApplicationDbContext context, IMapper mapper, IMediator mediator,
            IAuthService authService, IConfiguration configuration, IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
            _authService = authService;
            _configuration = configuration; 
        }

        public async Task<Result> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                //get user object
                var user = await _authService.GetUserAsync(request.AccessToken, request.SubscriberId, request.UserId, request.UserId);

                if (user == null)
                {
                    return Result.Failure("UserId is not valid");
                }
                var exists = await _context.Documents.AnyAsync(x => x.SubscriberId == request.SubscriberId && x.Name.ToLower().Trim() == request.Name.ToLower().Trim());

                if (exists)
                {
                    return Result.Failure($"Document name already exists!");
                }

                var entity = new Document
                {
                    SubscriberId = request.SubscriberId,
                    SubscriberName = _authService.Subscriber?.SubscriberName,
                    Name = request.Name,
                    Description = request.Description,
                    DisplayName = request.DisplayName,
                    Watermark = request.Watermark,
                    VersionNumber = request.VersionNumber,
                    DocumentType = request.DocumentType,
                    DocumentTypeDesc = request.DocumentType.ToString(),
                    DocumentShareType = request.DocumentShareType,
                    DocumentShareTypeDesc = request.DocumentShareType.ToString(),
                    DocumentStatus = DocumentStatus.Draft,
                    DocumentStatusDesc = DocumentStatus.Draft.ToString(),

                    UserId = request.UserId,
                    CreatedByEmail = user.Entity.Email,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.BeginTransactionAsync();
                await _context.Documents.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                if (request.DocumentPages != null && request.DocumentPages.Count > 0)
                {
                    var command = new UpdateDocumentPagesCommand
                    {
                        SubscriberId = request.SubscriberId,
                        SubscriberName = _authService.Subscriber?.Name,
                        DocumentId = entity.Id,
                        DocumentPages = request.DocumentPages,
                        UserId = request.UserId,
                        AccessToken = request.AccessToken
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
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                var result = _mapper.Map<DocumentDto>(entity);
                return Result.Success("Document request created successfully!", result);
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure($"Document request creation failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
