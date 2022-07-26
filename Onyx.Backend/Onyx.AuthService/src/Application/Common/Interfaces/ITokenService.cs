using Onyx.AuthService.Application.Common.Models;
using Onyx.AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.AuthService.Application.Common.Interfaces
{
   public interface ITokenService
    {
            Task<string> GenerateUserToken(User staff, List<RolePermission> rolePermissions);
            Task<AuthToken> GenerateAccessToken(User staff);
            Task<AuthToken> GenerateDeveloperToken(string userName, string Id);
            Task<LoginDetails> DecodeRefreshToken(string refreshToken);
            Task<Application.Common.Models.RefreshToken> GenerateRefreshToken(string userName, string password);
    }
}
