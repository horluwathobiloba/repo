using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
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
    public class SignContractActionCommand : AuthToken, IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public int ContractId { get; set; }
        /// <summary>
        /// This can be null depending on if the approver or signatory signs.
        /// </summary>
        public FileUploadRequest ApproverSignature { get; set; } = new FileUploadRequest();
        public string AppSigningUrl { get; set; }
        public string SignedDocumentUrl { get; set; }
        public int ContractRecipientId { get; set; }
        public string UserId { get; set; }
    }


    public class SignContractActionCommandHandler : IRequestHandler<SignContractActionCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;
        private readonly IMediator _mediator;
        private readonly INotificationService _notificationService;

        public SignContractActionCommandHandler(IApplicationDbContext context, IMapper mapper, IBlobStorageService blobService, 
            IEmailService emailService, IConfiguration configuration, IAuthService authService,IMediator mediator,INotificationService notificationService)
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

        public async Task<Result> Handle(SignContractActionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);
                var logAction = new LogRecipientActionCommand
                {
                    ApproverSignature = request.ApproverSignature,
                    AppSigningUrl = request.AppSigningUrl,
                    ContractId = request.ContractId,
                    ContractRecipientId = -request.ContractRecipientId,
                    OrganisationId = request.OrganisationId,
                    OrganisationName = _authService.Organisation?.Name,
                    RecipientAction = RecipientAction.Sign,
                    SignedDocumentUrl = request.SignedDocumentUrl,
                    UserId = request.UserId,
                    AccessToken = request.AccessToken
                };

                var result = await new LogRecipientActionCommandHandler(_context, _mapper, _blobService, _emailService, _configuration, _authService,_mediator,_notificationService)
                   .LogAction(logAction, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                return Result.Failure($"Contract recipient creation failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message} ");
            }
        }


    }
}

