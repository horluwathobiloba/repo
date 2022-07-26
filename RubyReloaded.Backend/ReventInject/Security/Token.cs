using System;

namespace ReventInject.Security
{
    public class Token
    {

        public enum TokenXterType
        {
            Default = 0,
            Numeric = 1,
            AlphaNumeric = 2,
        }

        /// <summary>
        /// This method is used to generate token
        /// </summary>
        /// <param name="type">The c=character type</param>
        /// <param name="length"></param>
        /// <returns></returns>
        public string GenerateOTP(TokenXterType type = TokenXterType.Numeric, int length = 6)
        {
            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "0123456789";

            string characters = numbers;

            switch (type)
            {
                case TokenXterType.Numeric:
                case TokenXterType.Default:
                    {
                        characters = numbers;
                        break;
                    }
                case TokenXterType.AlphaNumeric:
                    {
                        characters = alphabets + small_alphabets + numbers;
                        break;
                    }
                default:
                    {
                        characters = numbers;
                        break;
                    }
            }

            string otp = "";
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }

            return otp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        public string EncryptToken(string token)
        {
            return TrippleDES.EncryptString(token, "fbn@234#$1&");
        }

        /// <summary>
        /// Decrypt the token value
        /// </summary>
        /// <param name="token"></param>
        public string DecryptToken(string token)
        {
            return TrippleDES.DecryptString(token, "fbn@234#$1&");
        }
    }

}
