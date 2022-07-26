using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Onyx.DocumentService.Application.RecipientActions.Commands.LogRecipientAction;
using OnyxDoc.DocumentService.Application.Comments.Commands.CreateComment;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Domain.Enums;
using OnyxDoc.DocumentService.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.DocumentService.Application.RecipientActions.Commands.RejectAction
{
    public class RejectActionCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int DocumentId { get; set; }
        public string RejectionReason { get; set; }
        public int RecipientId { get; set; }
        public string RecipientEmail { get; set; }
        public string AppSigningUrl { get; set; }
        public string UserId { get; set; }
    }


    public class RejectActionCommandHandler : IRequestHandler<RejectActionCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;
        public RejectActionCommandHandler(IApplicationDbContext context, IMapper mapper, IBlobStorageService blobService, IEmailService emailService, 
            IConfiguration configuration, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _blobService = blobService;
            _emailService = emailService;
            _configuration = configuration;
            _authService = authService;
        }

        public async Task<Result> Handle(RejectActionCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var recipient = await _context.Recipients.Where(a => a.Email == request.RecipientEmail
                                   && a.Id == request.RecipientId).FirstOrDefaultAsync();
                if (recipient == null)
                {
                    return Result.Failure($"Invalid  Recipient Specified");
                }
                //check contract recipient action 
                var recipientAction = await _context.RecipientActions.Where(a => a.DocumentId == request.DocumentId && a.RecipientId == request.RecipientId).FirstOrDefaultAsync();
                if (recipientAction != null)
                {
                    return Result.Failure($" Recipient has already performed the {recipientAction.RecipientActionDesc} action");
                }
                var logAction = new LogRecipientActionCommand
                {
                    AppSigningUrl = request.AppSigningUrl,
                    DocumentId = request.DocumentId,
                    RecipientId = recipient.Id,
                    SubscriberId = request.SubscriberId,
                    DocumentRecipientAction = DocumentRecipientAction.Reject,
                    UserId = request.UserId,
                    AccessToken = request.AccessToken
                };

                var result = await new LogRecipientActionCommandHandler(_context, _mapper, _blobService, _emailService, _configuration, _authService)
                   .LogAction(logAction, cancellationToken);
                if (result.Succeeded)
                {
                    await CreateComment(request);
                }
                //return result;
                return Result.Success($"Document was rejected successfully");
            }
            catch (Exception ex)
            {
                return Result.Failure($" Document rejection failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message} ");
            }
        }

        internal async Task<Result> CreateComment(RejectActionCommand request)
        {
            var handler = new CreateCommentCommandHandler(_context, _mapper, _authService);
            var command = new CreateCommentCommand
            {
                AccessToken = request.AccessToken,
                UserId = request.UserId,
                Comment = request.RejectionReason,
                DocumentId = request.DocumentId
            };
            var result = await handler.Handle(command, new CancellationToken());
            return result;
        }
    }
}

