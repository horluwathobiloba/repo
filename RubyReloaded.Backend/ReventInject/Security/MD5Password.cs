using System;
using System.Text;
using System.Security.Cryptography;

namespace ReventInject.Security
{
    /// <summary>
    /// Summary description for MD5Password
    /// </summary>
    public class MD5Crypto
    {
    
        public MD5Crypto()
        {
        }

        /// <summary>
        /// Creates the secure password.
        /// </summary>
        /// <param name="clear">The clear.</param>
        /// <returns></returns>
        public string CreateSecurePassword(string clear)
        {
            byte[] clearBytes = null;
            byte[] computedHash = null;

            clearBytes = ASCIIEncoding.ASCII.GetBytes(clear);
            computedHash = new MD5CryptoServiceProvider().ComputeHash(clearBytes);

            return ByteArrayToString(computedHash);
        }
 
      

        /// <summary>
        /// Gets the password in clear.
        /// </summary>
        /// <param name="secure">The secure.</param>
        /// <returns></returns>
        public string GetClearPassword(string secure)
        {
            throw new Exception("One way encryption service");
        }

        /// <summary>
        /// Converts ByteArray to string.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns></returns>
        private string ByteArrayToString(byte[] array)
        {

            StringBuilder output = new StringBuilder(array.Length);

            for (int index = 0; index <= array.Length - 1; index++)
            {
                output.Append(array[index].ToString("X2"));
            }
            return output.ToString();
        }

        public bool AuthenticateUser(string UserName, string ClearPassword)
        {

            dynamic flag = false;
            try
            {
                //Dim u = New UserDAO().getUserByUserName(UserName)
                //Dim crypt = CreateSecurePassword(ClearPassword)
                //If u.Password = crypt Then
                //    flag = True
                //End If

            }
            catch (Exception ex)
            {
            }

            return flag;

        }

        public bool VerifyPassword(string password)
        {
            bool flag = false;

            if (password.Length >= 8 & UAlphaExists(password) & NumberExists(password))
            {
                flag = true;
            }

            return flag;
        }

        private bool UAlphaExists(string password)
        {
            bool flag = false;
            string UAlpha = this.Alphabets.ToUpper();
            foreach (char x in password.ToCharArray())
            {
                if (UAlpha.Contains(x))
                {
                    flag = true;
                    break; // TODO: might not be correct. Was : Exit For
                }
            }

            return flag;

        }

        private bool NumberExists(string password)
        {
            dynamic flag = false;
            dynamic num = this.Numbers;
            foreach (char x in password.ToCharArray())
            {
                if (num.Contains(x))
                {
                    flag = true;
                    break; // TODO: might not be correct. Was : Exit For
                }
            }

            return flag;

        }

        private string Specialcharacters
        {
            get
            {
                string result = "@,!,@,#,$,%,^,&,*";
                return result;
            }

        }

        private string Alphabets
        {
            get
            {
                string result = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
                return result;
            }

        }

        private string Numbers
        {
            get
            {
                string result = "0,1,2,3,4,5,6,7,8,9";
                return result;
            }
        }

    }
}



