using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace RubyReloaded.AuthService.Infrastructure.Utility
{
    public static class ExtensionMethods
    {
        public static JwtSecurityToken ExtractToken(this string str)
        {
            var stream = str.Remove(0, 7);

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var token = jsonToken as JwtSecurityToken;
            //var jti = tokenS.Claims.First(claim => claim.Type == "Sub").Value;
            return token;
        }


        public static bool ValidateToken(this JwtSecurityToken accessToken, string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    throw new Exception("Invalid User Id");
                }
                var tokenId = accessToken.Claims.First(claim => claim.Type == "userid")?.Value;
                
                    if (userId != tokenId)
                    {
                        throw new Exception("Invalid Token Credentials");
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
