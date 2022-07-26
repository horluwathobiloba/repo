using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.ContractComments.Commands.CreateContractComment;
using Onyx.ContractService.Application.ContractRecipientActions.Commands.LogRecipientAction;
using Onyx.ContractService.Application.ContractRecipientActions.Queries.GetContractRecipientActions;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using Onyx.ContractService.Domain.ViewModels;
using Onyx.ContractService.Infrastructure.Services;
using ReventInject;
using ReventInject.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractRecipientActions.Commands.ApproveAction
{
    public class RejectContractActionCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public int ContractId { get; set; }
        public string RejectionReason { get; set; }
        /// <summary>
        /// This can be null depending on if the approver or signatory signs.
        /// </summary>
        public FileUploadRequest ApproverSignature { get; set; } = new FileUploadRequest();
        public string AppSigningUrl { get; set; }
        public string SignedDocumentUrl { get; set; }
        public string ContractRecipientEmail { get; set; }
        public string UserId { get; set; }
    }


    public class RejectContractActionCommandHandler : IRequestHandler<RejectContractActionCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;
        private readonly IMediator _mediator;
        private readonly INotificationService _notificationService;
        public RejectContractActionCommandHandler(IApplicationDbContext context, IMapper mapper, IBlobStorageService blobService, IEmailService emailService, 
            IConfiguration configuration, IAuthService authService,IMediator mediator,INotificationService notificationService)
        {
            _context = context;
            _mapper = mapper;
            _blobService = blobService;
            _emailService = emailService;
            _configuration = configuration;
            _authService = authService;
            _mediator = mediator;
            _notificationService = notificationService;
        }

        public async Task<Result> Handle(RejectContractActionCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var contractRecipient = await _context.ContractRecipients.Include(a => a.Contract).Where(a => a.Email == request.ContractRecipientEmail
                                    && a.ContractId == request.ContractId).FirstOrDefaultAsync();
                if (contractRecipient == null)
                {
                    return Result.Failure($"Invalid Contract Recipient Specified");
                }
                //check contract recipient action 
                var recipientAction = await _context.ContractRecipientActions.Where(a => a.OrganisationId == request.OrganisationId &&
                                      a.ContractId == request.ContractId && a.ContractRecipientId == contractRecipient.Id
                                      && a.AppSigningUrl == request.AppSigningUrl).FirstOrDefaultAsync();
                if (recipientAction != null)
                {
                    return Result.Failure($"Contract Recipient has already performed the {recipientAction.RecipientAction} action");
                }
                if (contractRecipient.Contract != null)
                {
                    if (contractRecipient.Contract.NextActorEmail != request.ContractRecipientEmail)
                    {
                        return Result.Failure($"Invalid Approver Specified");
                    }
                }
               

                var logAction = new LogRecipientActionCommand
                {
                    ApproverSignature = request.ApproverSignature,
                    AppSigningUrl = request.AppSigningUrl,
                    ContractId = request.ContractId,
                    ContractRecipientId = contractRecipient.Id,
                    OrganisationId = request.OrganisationId,
                    OrganisationName = _authService.Organisation?.Name,
                    RecipientAction = RecipientAction.Reject,
                    SignedDocumentUrl = request.SignedDocumentUrl,
                    UserId = request.UserId,
                    AccessToken = request.AccessToken
                };

                var result = await new LogRecipientActionCommandHandler(_context, _mapper, _blobService, _emailService, _configuration, _authService,_mediator,_notificationService)
                   .LogAction(logAction, cancellationToken);
                if (result.Succeeded)
                {
                    await CreateComment(request);
                }
                return result;
            }
            catch (Exception ex)
            {
                return Result.Failure($"Contract recipient creation failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message} ");
            }
        }

        internal async Task<Result> CreateComment(RejectContractActionCommand request)
        {
            var handler = new CreateContractCommentCommandHandler(_context, _mapper, _authService);
            var contractCommand = new CreateContractCommentCommand
            {
                AccessToken = request.AccessToken,
                UserId = request.UserId,
                Comment = request.RejectionReason,
                ContractId = request.ContractId,
                OrganisationId = request.OrganisationId
            };
            var result = await handler.Handle(contractCommand, new CancellationToken());
            return result;
        }
    }
}

