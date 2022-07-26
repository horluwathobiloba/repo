using AutoMapper;
using MediatR;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Domain.Entities;
using OnyxDoc.DocumentService.Domain.Enums;
using OnyxDoc.DocumentService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Inboxes.Commands.CreateInboxes
{
    public class CreateInboxesCommand : AuthToken,IRequest<Result>
    {
        public List<InboxVm> InboxVms { get; set; }
    }

    public class CreateInboxesCommandHandler : IRequestHandler<CreateInboxesCommand, Result>
    {

        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        public CreateInboxesCommandHandler(IApplicationDbContext context, IAuthService authService)
        {
            _authService = authService;
            _context = context;

        }
        public async Task<Result> Handle(CreateInboxesCommand request, CancellationToken cancellationToken)
        {

            try
            {
                await _context.BeginTransactionAsync();
                var inboxes = new List<Inbox>();
                foreach (var inboxVm in request.InboxVms)
                {
                    var inbox = new Inbox
                    {
                        EmailAction = inboxVm.EmailAction,
                        DocumentId = inboxVm.DocumentId,
                        Sender = inboxVm.Sender,
                        SenderEmail = inboxVm.SenderEmail,
                        Status = Status.Active,
                        StatusDesc = Status.Active.ToString(),
                        Subject = inboxVm.Subject,
                        Description = inboxVm.Description,
                        DocumentUrl = inboxVm.DocumentUrl,
                        CreatedDate = DateTime.Now,
                        SubscriberId = inboxVm.SubscriberId,
                        SubscriberName = inboxVm.SubscriberName,
                        RecipientNames = inboxVm.Recipients,
                        Email = inboxVm.Email,
                        Read = false
                    };
                    inboxes.Add(inbox);
                }
                await _context.Inboxes.AddRangeAsync(inboxes);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                return Result.Success("Inbox was created successfully",inboxes);
            }
            catch (Exception ex)
            {
                 _context.RollbackTransaction();
                return Result.Failure($"Creating Inbox failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
