using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Upskillz_invoice_mgt_Application.Common;
using Upskillz_invoice_mgt_Application.InviteUsers.Command;
using Upskillz_invoice_mgt_Domain.Common;
using Upskillz_invoice_mgt_Domain.Entities;
using Upskillz_invoice_mgt_Domain.Interfaces;

namespace Upskillz_invoice_mgt_Application.InviteUsers.Handler
{
    public class SendInviteCommandHandler : IRequestHandler<SendInviteCommand, Response<bool>>
    {
        private readonly IMailService _mailService;
        private readonly UserManager<AppUser> _userManager;

        public SendInviteCommandHandler(IMailService mailService,
            UserManager<AppUser> userManager)
        {
            _mailService = mailService;
            _userManager = userManager;
        }
        public async Task<Response<bool>> Handle(SendInviteCommand request, CancellationToken cancellationToken)
        {
            //AppUser loggedAdmin = _userManager.Users.FirstOrDefault(x => x.Id == request.LoggedUserId);
            request.UsersEmail = request.UsersEmail.Where( y => (_userManager.FindByEmailAsync(y)).Result == null).ToList();
            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            bool result = false;

            List<MailRequest> mailRequests = new();
            foreach (var email in request.UsersEmail)
            {
                var mailBody = await EmailBodyBuilder.GetEmailBody(emailTempPath: "StaticsFiles/Html/UserInvite.html","JustFoodie", "123", email);
                MailRequest mailRequest = new()
                {
                    Subject = $"Invitation To Join ",//{loggedAdmin.Company.CompanyName}
                    Body = mailBody,
                    ToEmail = email
                };
                mailRequests.Add(mailRequest);
            }

            result = await _mailService.SendEmailAsync(mailRequests);
            if (result)
            {
                transaction.Complete();
                return Response<bool>.Success("Mail sent successfully", true, StatusCodes.Status200OK);
            }
            transaction.Dispose();
            return Response<bool>.Fail("Mail service failed", StatusCodes.Status400BadRequest);
        }
    }
}
