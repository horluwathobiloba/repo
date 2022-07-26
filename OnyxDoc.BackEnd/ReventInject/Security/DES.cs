using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReventInject.Utilities;

namespace ReventInject.Security
{
    public class DES
    {

        public class KryptoResultText
        {
            private string _Key;

            private string _StrResult;
            public string Key
            {
                get { return _Key; }
                set { _Key = value; }
            }

            public string StrResult
            {
                get { return _StrResult; }
                set { _StrResult = value; }
            }
        }

        public KryptoResultText Encryption(string strInput, string sKey = null)
        {

            dynamic result = new KryptoResultText();
            byte[] Results = null;

            //Must be 64 bits, 8 bytes.
            //Get the key for the file to encrypt.
            //You can distribute this key to the user who will decrypt the file.
            if (sKey == null)
            {
                sKey = GenerateKey();
            }

            // For additional security, pin the key.
            GCHandle gch = GCHandle.Alloc(sKey, GCHandleType.Pinned);

            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();

            //Set secret key for DES algorithm.
            //A 64-bit key and an IV are required for this provider.
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);

            //Set the initialization vector.
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

            // Convert the input string to a byte[]
            UTF8Encoding UTF8 = new UTF8Encoding();
            byte[] DataToEncrypt = UTF8.GetBytes(strInput);

            //Create the DES encryptor from this instance.
            ICryptoTransform Encryptor = DES.CreateEncryptor();
            //Create the crypto stream that transforms the file stream by using DES encryption.   
            // Step 5. Attempt to encrypt the string
            try
            {
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);

            }
            finally
            {
                // Clear the DES and Hashprovider services of any sensitive information
                DES.Clear();
                UTF8 = null;
            }


            // Step 6. Return the encrypted string as a base64 encoded string
            result.Key = sKey;
            result.StrResult = Convert.ToBase64String(Results);
            return result;

        }

        public KryptoResultText Decryption(string strInput, string sKey)
        {

            dynamic result = new KryptoResultText();

            byte[] Results = null;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            //A 64-bit key and an IV are required for this provider.
            //Set the secret key for the DES algorithm.
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            //Set the initialization vector.
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

            //Convert the input string to a byte[]
            byte[] DataToDecrypt = Convert.FromBase64String(strInput);

            //Attempt to decrypt the string
            try
            {
                ICryptoTransform Decryptor = DES.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);

            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                DES.Clear();
            }

            result.Key = sKey;
            result.StrResult = UTF8.GetString(Results);
            // Return the decrypted string in UTF8 format
            return result;

        }


        public string Encrypt(string strInput, string sKey)
        {

            byte[] Results = null;

            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();

            //Set secret key for DES algorithm.
            //A 64-bit key and an IV are required for this provider.
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);

            //Set the initialization vector.
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

            // Convert the input string to a byte[]
            UTF8Encoding UTF8 = new UTF8Encoding();
            byte[] DataToEncrypt = UTF8.GetBytes(strInput);


            //Create the DES encryptor from this instance.
            ICryptoTransform Encryptor = DES.CreateEncryptor();
            //Create the crypto stream that transforms the file stream by using DES encryption.
            dynamic result = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);

            // Step 5. Attempt to encrypt the string
            try
            {
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                // Clear the DES and Hashprovider services of any sensitive information
                DES.Clear();
                UTF8 = null;
            }

            // Step 6. Return the encrypted string as a base64 encoded string
            return Convert.ToBase64String(Results);

        }

        public string Decrypt(string strInput, string sKey)
        {

            byte[] Results = null;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            //A 64-bit key and an IV are required for this provider.
            //Set the secret key for the DES algorithm.
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            //Set the initialization vector.
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

            //Convert the input string to a byte[]
            byte[] DataToDecrypt = Convert.FromBase64String(strInput);

            //Attempt to decrypt the string
            try
            {
                ICryptoTransform Decryptor = DES.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                DES.Clear();
            }

            // Return the decrypted string in UTF8 format
            return UTF8.GetString(Results);

        }
        [DllImport("kernel32.dll", EntryPoint = "RtlZeroMemory", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]


        // Call this function to remove the key from memory after it is used for security.
        private static extern void ZeroMemory(string Destination, int Length);

        // Call this function to remove the key from memory after it is used for security.
        [DllImport("kernel32.dll")]
        public static extern void ZeroMemory(IntPtr addr, int size);

        // Function to generate a 64-bit key.
        public string GenerateKey()
        {
            // Create an instance of a symmetric algorithm. The key and the IV are generated automatically.
            DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();

            // Use the automatically generated key for encryption. 
            return ASCIIEncoding.ASCII.GetString(desCrypto.Key);

        }

        public KryptoResultFile EnkryptFile(string sInputFilename, string sOutputFilename, string sKey = null)
        {

            dynamic result = new KryptoResultFile();

            try
            {
                //Must be 64 bits, 8 bytes.
                //Get the key for the file to encrypt.
                //You can distribute this key to the user who will decrypt the file.
                if (sKey == null)
                {
                    sKey = GenerateKey();
                }

                // For additional security, pin the key.
                GCHandle gch = GCHandle.Alloc(sKey, GCHandleType.Pinned);

                FileStream fsInput = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read);
                FileStream fsEncrypted = new FileStream(sOutputFilename, FileMode.Create, FileAccess.Write);

                DESCryptoServiceProvider DES = new DESCryptoServiceProvider();

                //Set secret key for DES algorithm.
                //A 64-bit key and an IV are required for this provider.
                DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);

                //Set the initialization vector.
                DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

                //Create the DES encryptor from this instance.
                ICryptoTransform desencrypt = DES.CreateEncryptor();
                //Create the crypto stream that transforms the file stream by using DES encryption.
                CryptoStream cryptostream = new CryptoStream(fsEncrypted, desencrypt, CryptoStreamMode.Write);

                //Read the file text to the byte array.
                byte[] bytearrayinput = new byte[fsInput.Length];
                fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
                //Write out the DES encrypted file.
                cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
                cryptostream.Close();

                //compile the result 
                result.Key = sKey;
                result.OutputFilePath = sOutputFilename;

            }
            catch (Exception ex)
            {
                result = null;
               throw ex;
            }

            return result;
        }

        public KryptoResultFile DekryptFile(string sInputFilename, string sOutputFilename, string sKey)
        {

            dynamic result = new KryptoResultFile();


            try
            {
                DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
                //A 64-bit key and an IV are required for this provider.
                //Set the secret key for the DES algorithm.
                DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                //Set the initialization vector.
                DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

                //Create the file stream to read the encrypted file back.
                FileStream fsread = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read);
                //Create the DES decryptor from the DES instance.
                ICryptoTransform desdecrypt = DES.CreateDecryptor();
                //Create the crypto stream set to read and to do a DES decryption transform on incoming bytes.
                CryptoStream cryptostreamDecr = new CryptoStream(fsread, desdecrypt, CryptoStreamMode.Read);
                //Print out the contents of the decrypted file.
                StreamWriter fsDecrypted = new StreamWriter(sOutputFilename);
                fsDecrypted.Write(new StreamReader(cryptostreamDecr).ReadToEnd());
                fsDecrypted.Flush();
                fsDecrypted.Close();

                result.Key = sKey;
                result.OutputFilePath = sOutputFilename;

            }
            catch (Exception ex)
            {
                result = null;
               throw ex;
            }

            return result;
        }


        public void EncryptFile(string sInputFilename, string sOutputFilename, string sKey)
        {
            FileStream fsInput = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read);
            FileStream fsEncrypted = new FileStream(sOutputFilename, FileMode.Create, FileAccess.Write);

            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();

            //Set secret key for DES algorithm.
            //A 64-bit key and an IV are required for this provider.
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);

            //Set the initialization vector.
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

            //Create the DES encryptor from this instance.
            ICryptoTransform desencrypt = DES.CreateEncryptor();
            //Create the crypto stream that transforms the file stream by using DES encryption.
            CryptoStream cryptostream = new CryptoStream(fsEncrypted, desencrypt, CryptoStreamMode.Write);

            //Read the file text to the byte array.
            byte[] bytearrayinput = new byte[fsInput.Length];
            fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
            //Write out the DES encrypted file.
            cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.Close();


        }


        public void DecryptFile(string sInputFilename, string sOutputFilename, string sKey)
        {
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            //A 64-bit key and an IV are required for this provider.
            //Set the secret key for the DES algorithm.
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            //Set the initialization vector.
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

            //Create the file stream to read the encrypted file back.
            FileStream fsread = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read);
            //Create the DES decryptor from the DES instance.
            ICryptoTransform desdecrypt = DES.CreateDecryptor();
            //Create the crypto stream set to read and to do a DES decryption transform on incoming bytes.
            CryptoStream cryptostreamDecr = new CryptoStream(fsread, desdecrypt, CryptoStreamMode.Read);
            //Print out the contents of the decrypted file.
            StreamWriter fsDecrypted = new StreamWriter(sOutputFilename);
            fsDecrypted.Write(new StreamReader(cryptostreamDecr).ReadToEnd());
            fsDecrypted.Flush();
            fsDecrypted.Close();

        }

        public class KryptoResultFile
        {
            private string _Key;

            private string _OutputFilePath;
            public string Key
            {
                get { return _Key; }
                set { _Key = value; }
            }

            public string OutputFilePath
            {
                get { return _OutputFilePath; }
                set { _OutputFilePath = value; }
            }

        }

        public void TestFileEncryption()
        {
            //Must be 64 bits, 8 bytes.
            string sSecretKey = null;

            // Get the key for the file to encrypt.
            // You can distribute this key to the user who will decrypt the file.
            sSecretKey = GenerateKey();

            // For additional security, pin the key.
            GCHandle gch = GCHandle.Alloc(sSecretKey, GCHandleType.Pinned);


            // Encrypt the file.        
            EncryptFile("C:\\FirstPay\\Raw\\csv1.csv", "C:\\FirstPay\\Processed\\csv1.csv", sSecretKey);
            Console.WriteLine("Encryption Successful!");

            // Decrypt the file.
            DecryptFile("C:\\FirstPay\\Processed\\csv1.csv", "C:\\FirstPay\\Hacked\\csv1.csv", sSecretKey);
            Console.WriteLine("Decryption Successful!");

            // Remove the key from memory. 
            ZeroMemory(gch.AddrOfPinnedObject(), sSecretKey.Length * 2);
            gch.Free();
        }

    }
}