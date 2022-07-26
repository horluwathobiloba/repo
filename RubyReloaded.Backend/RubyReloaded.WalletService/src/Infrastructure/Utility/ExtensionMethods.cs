using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace RubyReloaded.WalletService.Infrastructure.Utility
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

        public static bool ValidateToken(this JwtSecurityToken accessToken, string userId, int subscriberId = 0)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(userId))
                {
                    throw new Exception("Invalid User Id");
                }
                var tokenId = accessToken.Claims.First(claim => claim.Type == "userid")?.Value;
                //  var roleAccessLevel = accessToken.Claims.First(claim => claim.Type == "roleaccesslevel")?.Value;
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
        public static bool ValidateBVN(this JwtSecurityToken accessToken)
        {
            try
            {
                var bvn = accessToken.Claims.First(claim => claim.Type == "bvn")?.Value;
                //  var roleAccessLevel = accessToken.Claims.First(claim => claim.Type == "roleaccesslevel")?.Value;
                if (string.IsNullOrEmpty(bvn))
                {
                    throw new Exception("Invalid BVN Details.Kindly setup your BVN");
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool CheckAccessLevels(this JwtSecurityToken accessToken, string userId, int subscriberId = 0)
        {
            try
            {
                var roleAccessLevel = accessToken.Claims.First(claim => claim.Type == "roleaccesslevel")?.Value;
                if (roleAccessLevel != AccessLevel.SystemOwner.ToString())
                {
                    return false;
                    //throw new Exception("Access Deni");
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