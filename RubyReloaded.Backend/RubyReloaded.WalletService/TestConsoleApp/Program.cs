//using OnyxRevamped.WindowsService.Utility;
//using System;
//using System.Configuration;
//using System.IO;

//namespace TestConsoleApp
//{
//    class Program
//    {

//        private const string _logFileLocation = @"C:\temp\servicelog.txt";
//        static void Main(string[] args)
//        {
//            try
//            {
//                Directory.CreateDirectory(Path.GetDirectoryName(_logFileLocation));

//                string appName = ConfigurationManager.AppSettings["appName"];
//                string tokenApiUrl = ConfigurationManager.AppSettings["tokenApiUrl"];
//                string expiryEmailUrl = ConfigurationManager.AppSettings["contractExpiryUrl"];

//                //get api key first
//                APIClient aPIClient = new APIClient();
//                var accessToken = aPIClient.GetBearerToken(tokenApiUrl, new { name = appName }).Result;
//                var response = aPIClient.PostAPIUrl(expiryEmailUrl, accessToken, new { name = appName }, false).Result;
//                File.AppendAllText(_logFileLocation, DateTime.Now.ToString() + " : " + accessToken + response + Environment.NewLine);
//                string apiUrl = ConfigurationManager.AppSettings["apiUrl"];
//                Console.WriteLine("Hello World!");
//            }
//            catch (Exception ex)
//            {

//                throw ex;
//            }
//        }
//    }
//}
