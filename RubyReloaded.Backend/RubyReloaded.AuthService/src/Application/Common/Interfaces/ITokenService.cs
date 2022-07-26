using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Entities;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Common.Interfaces
{
   public interface ITokenService
    {
            Task<string> GenerateUserToken(Domain.Entities.User staff);
            Task<AuthToken> GenerateAccessToken(Domain.Entities.User staff);
            Task<AuthToken> GenerateDeveloperToken(string userName, string Id);
        Task<AuthToken> GenerateInviteToken(string email, string code, string cooperativeId, LinkType type);
    }
}
