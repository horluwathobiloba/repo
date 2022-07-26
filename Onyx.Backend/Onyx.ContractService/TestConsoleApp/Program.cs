using Onyx.WindowsService.Utility;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestConsoleApp
{
    //class Program
    //{

    //    private const string _logFileLocation = @"C:\temp\servicelog.txt";
    //    static void Main(string[] args)
    //    {
    //        try
    //        {
    //            Directory.CreateDirectory(Path.GetDirectoryName(_logFileLocation));

    //            string appName = ConfigurationManager.AppSettings["appName"];
    //            string tokenApiUrl = ConfigurationManager.AppSettings["tokenApiUrl"];
    //            string expiryEmailUrl = ConfigurationManager.AppSettings["contractExpiryUrl"];

    //            APIClient aPIClient = new APIClient();
    //            var accessToken = aPIClient.GetBearerToken(tokenApiUrl, new { name = appName }).Result;
    //            var response = aPIClient.PostAPIUrl(expiryEmailUrl, accessToken, new { name = appName }, false).Result;
    //            File.AppendAllText(_logFileLocation, DateTime.Now.ToString() + response + ":" + Environment.NewLine);

    //             appName = ConfigurationManager.AppSettings["appName"];
    //             tokenApiUrl = ConfigurationManager.AppSettings["tokenApiUrl"];
    //            var approvalReminderUrl = ConfigurationManager.AppSettings["approvalReminderUrl"];
    //            //APIClient aPIClient = new APIClient();
    //             accessToken = aPIClient.GetBearerToken(tokenApiUrl, new { name = appName }).Result;

    //            accessToken = aPIClient.GetBearerToken(tokenApiUrl, new { name = appName }).Result;
    //            response = aPIClient.PostAPIUrl(approvalReminderUrl, accessToken, new { name = appName }, false).Result;
    //            File.AppendAllText(_logFileLocation, DateTime.Now.ToString() + response + ":" + Environment.NewLine);
    //        }
    //        catch (Exception ex)
    //        {

    //            throw ex;
    //        }
    //    }
    //}

    public class Program
    {
        public static async Task Main()
        {
            ////timer1
            //Stopwatch timer = new Stopwatch();
            //timer.Start();
            //Task<int> downloading = DownloadDocsMainPageAsync();
            //Console.WriteLine($"{nameof(Main)}: Launched downloading.");
            //int bytesLoaded = await downloading;
            //Console.WriteLine($"{nameof(Main)}: Downloaded {bytesLoaded} bytes.");
            //timer.Stop();
            //Console.WriteLine($"Timer: {timer.Elapsed.TotalSeconds}");
            ////timer2
            //Stopwatch timer2 = new Stopwatch();
            //timer2.Start();
            //Task<int> downloading2 = DownloadDocsMainPageAsync();
            //Console.WriteLine($"{nameof(Main)}: Launched downloading.");
            //await downloading2;
            //timer2.Stop();
            //Console.WriteLine($"Timer: {timer2.Elapsed.TotalSeconds}");
            ////timer3
            //Stopwatch timer3 = new Stopwatch();
            //timer3.Start();
            //Task<int> downloading3 = DownloadDocsMainPageAsync();
            //Console.WriteLine($"{nameof(Main)}: Launched downloading.");
            //timer3.Stop();
            //Console.WriteLine($"{nameof(Main)}: Downloaded {downloading3.Result} bytes.");
            //Console.WriteLine($"Timer: {timer3.Elapsed.TotalSeconds}");

            for (int i = 0; i < 10; i++)
            {
                //timer1
                Stopwatch timer = new Stopwatch();
                timer.Start();
                Task<int> downloading = DownloadDocsMainPageAsync();
                Console.WriteLine($"{nameof(Main)}: Launched downloading.");
                int bytesLoaded = await downloading;
                Console.WriteLine($"{nameof(Main)}: Downloaded {bytesLoaded} bytes.");
                timer.Stop();
                Console.WriteLine($"Timer: {timer.Elapsed.TotalSeconds}");
                Console.WriteLine($"...............................................");
                //timer2
                Stopwatch timer2 = new Stopwatch();
                timer2.Start();
                Task<int> downloading2 = DownloadDocsMainPageAsync();
                Console.WriteLine($"{nameof(Main)}: Launched downloading.");
                await downloading2;
                timer2.Stop();
                Console.WriteLine($"Timer: {timer2.Elapsed.TotalSeconds}");
                Console.WriteLine($"...............................................");
                //timer3
                Stopwatch timer3 = new Stopwatch();
                timer3.Start();
                Task<int> downloading3 = DownloadDocsMainPageAsync();
                Console.WriteLine($"{nameof(Main)}: Launched downloading.");
                timer3.Stop();
                Console.WriteLine($"{nameof(Main)}: Downloaded {downloading3.Result} bytes.");
                Console.WriteLine($"Timer: {timer3.Elapsed.TotalSeconds}");
                Console.WriteLine($"...............................................");
                Console.WriteLine();
                Console.WriteLine($"....................Running Next Batch...........................");
            }
        }
        private static async Task<int> DownloadDocsMainPageAsync()
        {
            Console.WriteLine($"{nameof(DownloadDocsMainPageAsync)}: About to start downloading.");
            var client = new HttpClient();
            byte[] content = await client.GetByteArrayAsync("https://docs.microsoft.com/en-us/");
            Console.WriteLine($"{nameof(DownloadDocsMainPageAsync)}: Finished downloading.");
            return content.Length;
        }
    }
}
