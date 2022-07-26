using Newtonsoft.Json;
using Onyx.WorkFlowService.Application.Common.Interfaces;
using Onyx.WorkFlowService.Application.Common.Models;
using Onyx.WorkFlowService.Domain.Entities;
using Onyx.WorkFlowService.Infrastructure.Auth;
using Onyx.WorkFlowService.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Onyx.WorkFlowService.Infrastructure.Authentication
{
    public class TokenService : ITokenService
    {

        public AuthToken GenerateToken(Staff staff, List<RolePermission> rolePermissions)
        {

            try
            { 
                var permissions = JsonConvert.SerializeObject(rolePermissions);
                var staffEntity = JsonConvert.SerializeObject(staff);
                var accessLevel = JsonConvert.SerializeObject(staff.Role.AccessLevel);
                List<Claim> claims = new List<Claim>() {
                new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim (JwtRegisteredClaimNames.Email, staff.Email),
                 new Claim (JwtRegisteredClaimNames.Sub, staff.Role.Name),
                new Claim("Access Level",accessLevel),
                 new Claim (JwtRegisteredClaimNames.Sub, staff.Role.Name),
                new Claim ("StaffEntity", staffEntity),
                };
                if (rolePermissions != null && rolePermissions.Count > 0)
                {
                    claims.Add(new Claim("RolePermissions", permissions));
                }
                JwtSecurityToken token = new TokenBuilder()
                .AddAudience(TokenConstants.Audience)
                .AddIssuer(TokenConstants.Issuer)
                .AddExpiry(TokenConstants.ExpiryInMinutes)
                .AddKey(TokenConstants.key)
                .AddClaims(claims)
                .Build();

                string accessToken = new JwtSecurityTokenHandler().WriteToken(token);

                return new AuthToken()
                {
                    AccessToken = accessToken,
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
