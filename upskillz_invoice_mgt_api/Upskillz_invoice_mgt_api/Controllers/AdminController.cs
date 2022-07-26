using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Upskillz_invoice_mgt_Application.Common;
using Upskillz_invoice_mgt_Application.InviteUsers.Command;
using Upskillz_invoice_mgt_Infrastructure.Policy;

namespace Upskillz_invoice_mgt_api.Controllers
{
    //[ApiController]
    public class AdminController : ApiController
    {
        [HttpPost("Invite-users")]
        //[Authorize(Policy = Policies.SuperAdmin)]
        public async Task<ActionResult<Response<bool>>> SendInvites(SendInviteCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
