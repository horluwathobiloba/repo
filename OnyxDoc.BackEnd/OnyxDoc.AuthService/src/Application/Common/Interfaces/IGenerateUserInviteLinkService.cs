using OnyxDoc.AuthService.Application.Common.Models;
using OnyxDoc.AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnyxDoc.AuthService.Application.Common.Interfaces
{
    public interface IGenerateUserInviteLinkService
    {
        Task<string> GenerateUserInviteLink(UserInviteLink inviteLink);
        Task<string> GenerateUserLink(UserInviteLink inviteLink);
        Task<UserInviteLinkVm>ConfirmUserLink(string inviteLink);
        Task<string> GenerateHashLink(UserHashCode hashCode);

    }
}
