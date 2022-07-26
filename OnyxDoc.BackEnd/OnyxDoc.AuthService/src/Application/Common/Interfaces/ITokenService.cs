using OnyxDoc.AuthService.Application.Common.Models;
using OnyxDoc.AuthService.Domain.Entities;
using OnyxDoc.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnyxDoc.AuthService.Application.Common.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateUserToken(string subscriberType,User user, object rolePermissions, Domain.Entities.Branding branding);
        Task<AuthToken> GenerateAccessToken(User user);
        Task<AuthToken> GenerateDeveloperToken(string userName, string Id);
    }
}
