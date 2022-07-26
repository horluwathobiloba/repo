using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Threading.Tasks;

namespace ReventInject.Security
{

    /// <summary>
    /// Wrapper class for Triple DES encryption
    /// </summary>
    /// <remarks>
    /// Author : Paul Hayman<br></br>
    /// Date : Feb 2006<br></br>
    /// info@PaulHayman.com
    /// </remarks>
    public class TrippleDES
    {
        public static string CryptoExceptionMessage = null;

        private static TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
        private static string defaultPass = "p@ssw0rd2$$";

        private static UTF8Encoding utf8 = new UTF8Encoding();
        private static byte[] keyValue;

        private static byte[] iVValue;
        public TrippleDES()
        {
            keyValue = new byte[] {
				4,
				8,
				2,
				2,
				7,
				7,
				8,
				7
			};
            iVValue = new byte[] {
				4,
				8,
				2,
				2,
				7,
				7,
				8,
				7
			};
        }

        /// <summary>
        /// Key to use during encryption and decryption
        /// </summary>
        /// <remarks>
        /// <example>
        /// byte[] key = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
        /// </example>
        /// </remarks>
        private byte[] Key
        {
            get { return keyValue; }
            set { keyValue = value; }
        }

        /// <summary>
        /// Initialization vector to use during encryption and decryption
        /// </summary>
        /// <remarks>
        /// <example>
        /// byte[] iv = { 8, 7, 6, 5, 4, 3, 2, 1 };
        /// </example>
        /// </remarks>
        private byte[] iV
        {
            get { return iVValue; }
            set { iVValue = value; }
        }

        /// <summary>
        /// Constructor, allows the key and initialization vetor to be provided
        /// </summary>
        /// <param name="key"><see cref="Key"/></param>
        /// <param name="iV"><see cref="iV"/></param>
        public TrippleDES(byte[] key, byte[] iV)
        {
            keyValue = key;
            iVValue = iV;
        }

        /// <summary>
        /// Decrypt bytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns>Decrypted data as bytes</returns>
        private static byte[] Decrypt(byte[] bytes)
        {
            return Transform(bytes, des.CreateDecryptor(keyValue, iVValue));
        }

        /// <summary>
        /// Encrypt bytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns>Encrypted data as bytes</returns>
        private static byte[] Encrypt(byte[] bytes)
        {
            return Transform(bytes, des.CreateEncryptor(keyValue, iVValue));
        }

        /// <summary>
        /// Decrypt a string
        /// </summary>
        /// <param name="text"></param>
        /// <returns>Decrypted data as string</returns>
        private static string Decrypt(string text)
        {
            byte[] input = Convert.FromBase64String(text);
            byte[] output = Transform(input, des.CreateDecryptor(keyValue, iVValue));
            return utf8.GetString(output);
        }

        /// <summary>
        /// Encrypt a string
        /// </summary>
        /// <param name="text"></param>
        /// <returns>Encrypted data as string</returns>
        private static string Encrypt(string text)
        {
            byte[] input = utf8.GetBytes(text);
            byte[] output = Transform(input, des.CreateEncryptor(keyValue, iVValue));
            return Convert.ToBase64String(output);
        }

        /// <summary>
        /// Encrypt or Decrypt bytes.
        /// </summary>
        /// <remarks>
        /// This is used by the public methods
        /// </remarks>
        /// <param name="input">Data to be encrypted/decrypted</param>
        /// <param name="cryptoTransform">
        /// <example>des.CreateEncryptor(this.keyValue, this.iVValue)</example>
        /// </param>
        /// <returns>Byte data containing result of opperation</returns>
        private static byte[] Transform(byte[] input, ICryptoTransform cryptoTransform)
        {
            try
            {
                // Create the necessary streams
                MemoryStream memory = new MemoryStream();
                CryptoStream stream = new CryptoStream(memory, cryptoTransform, CryptoStreamMode.Write);

                // Transform the bytes as requesed
                stream.Write(input, 0, input.Length);
                stream.FlushFinalBlock();

                // Read the memory stream and convert it back into byte array
                memory.Position = 0;
                byte[] result = new byte[memory.Length];
                memory.Read(result, 0, result.Length);

                // Clean up
                memory.Close();
                stream.Close();

                // Return result
                return result;
            }
            catch (CryptographicException ex)
            {
                CryptoExceptionMessage = ex.ExceptionMessage();
                throw new Exception(CryptoExceptionMessage, ex.InnerException);
            }
            catch (Exception ex2)
            {
                CryptoExceptionMessage = ex2.ExceptionMessage();
                throw new Exception(CryptoExceptionMessage, ex2.InnerException);
            }


        }

        private static string Encryption(string PlainText, string key = null)
        {
            try
            {
                TripleDES des = string.IsNullOrEmpty(key) ? CreateDES(defaultPass) : CreateDES(key);

                ICryptoTransform ct = des.CreateEncryptor();

                byte[] input = Encoding.Unicode.GetBytes(PlainText);

                return Convert.ToBase64String(ct.TransformFinalBlock(input, 0, input.Length));
            }
            catch (CryptographicException ex)
            {
                CryptoExceptionMessage = ex.ExceptionMessage();
                throw new Exception(CryptoExceptionMessage, ex.InnerException);
            }
            catch (Exception ex2)
            {
                CryptoExceptionMessage = ex2.ExceptionMessage();
                throw new Exception(CryptoExceptionMessage, ex2.InnerException);
            }


        }

        private static string Decryption(string CypherText, string key = null)
        {

            byte[] b = Convert.FromBase64String(CypherText);

            //
            TripleDES des = string.IsNullOrEmpty(key) ? CreateDES(defaultPass) : CreateDES(key);
            ICryptoTransform ct = des.CreateDecryptor();
            byte[] output = ct.TransformFinalBlock(b, 0, b.Length);
            return Encoding.Unicode.GetString(output);

        }

        private static TripleDES CreateDES(string key)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            TripleDES des = new TripleDESCryptoServiceProvider();
            des.Mode = CipherMode.CBC;
            des.Key = md5.ComputeHash(Encoding.Unicode.GetBytes(key));
            des.IV = new byte[des.BlockSize / 8];
            return des;
        }

        #region "Public  functions"

        /// <summary>
        /// This method encrypts a string using the TrippleDES algorithm
        /// and the key used to encrypt the clear string.
        /// </summary>
        /// <param name="clearstring">The clear string</param>
        /// <param name="key">The key to be used for encrypting the clear string</param>
        public static string EncryptString(string clearstring, string key = null)
        {
            byte[] Results = null;
            UTF8Encoding UTF8 = new UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            //We use the MD5 hash generator as the result is a 128 bit byte array
            //which is a valid length for the TripleDES encoder we use below


            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = string.IsNullOrEmpty(key) ? HashProvider.ComputeHash(UTF8.GetBytes(defaultPass)) : HashProvider.ComputeHash(UTF8.GetBytes(key));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the encoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            byte[] DataToEncrypt = UTF8.GetBytes(clearstring);

            // Step 5. Attempt to encrypt the string
            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the encrypted string as a base64 encoded string
            return Convert.ToBase64String(Results);
        }

        /// <summary>
        /// This method decrypts an already encrypted string using the TrippleDES algorithm
        /// and the key used to encrypt the clear string.
        /// </summary>
        /// <param name="Message">The clear string</param>
        /// <param name="key">The key to be used for encrypting the clear string</param>
        public static string DecryptString(string encryptedstring, string key = null)
        {
            byte[] Results = null;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            //We use the MD5 hash generator as the result is a 128 bit byte array
            //which is a valid length for the TripleDES encoder we use below

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = string.IsNullOrEmpty(key)
                ?
                HashProvider.ComputeHash(UTF8.GetBytes(defaultPass)) : HashProvider.ComputeHash(UTF8.GetBytes(key));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the decoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            byte[] DataToDecrypt = Convert.FromBase64String(encryptedstring);

            // Step 5. Attempt to decrypt the string
            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the decrypted string in UTF8 format
            return UTF8.GetString(Results);
        }

        #endregion

    }


}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================

