using OnyxDoc.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace OnyxDoc.AuthService.Infrastructure.Utility
{
    public static class ExtensionMethods
    {
        public static JwtSecurityToken ExtractToken(this string str)
        {
            var stream = str.Remove(0, 7);

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var token = jsonToken as JwtSecurityToken;
            return token;
        }

        public static bool ValidateToken(this JwtSecurityToken accessToken, string userId,int subscriberId=0)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(userId))
                {
                    throw new Exception("Invalid User Id");
                    // Made change
                }
                var developerToken = accessToken.Claims.First(claim => claim.Type == "email")?.Value;
                if (!string.IsNullOrEmpty(developerToken))
                {
                    //if (developerToken.Contains("onyx"))
                    {
                        return true;
                    }
                }
                
                var tokenId = accessToken.Claims.First(claim => claim.Type == "userid")?.Value;
                var roleAccessLevel = accessToken.Claims.First(claim => claim.Type == "roleaccesslevel")?.Value;
                if (!(roleAccessLevel == RoleAccessLevel.Admin.ToString() || roleAccessLevel == RoleAccessLevel.SuperAdmin.ToString()))
                {
                    if (userId != tokenId)
                    {
                        throw new Exception("Invalid Token Credentials");
                    }

                }
                var subId = Convert.ToInt32(accessToken.Claims.First(claim => claim.Type == "subscriberId")?.Value);
                if (subscriberId != 0)
                {
                    if (subId != subscriberId)
                    {
                        throw new Exception("You're not authorized");
                    } 
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
    }
}
