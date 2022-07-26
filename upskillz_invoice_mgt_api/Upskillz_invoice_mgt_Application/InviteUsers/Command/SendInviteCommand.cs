using MediatR;
using System.Collections.Generic;
using Upskillz_invoice_mgt_Application.Common;

namespace Upskillz_invoice_mgt_Application.InviteUsers.Command
{
    public class SendInviteCommand : IRequest<Response<bool>>
    {
        public ICollection<string> UsersEmail { get; set; }
        public string LoggedUserId { get; set; }
    }
}
