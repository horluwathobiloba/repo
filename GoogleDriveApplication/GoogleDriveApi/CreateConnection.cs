using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Util.Store;
using System;
using System.IO;
using System.Reflection;
using System.Threading;

namespace GoogleDriveApi
{
    internal class CreateConnection
    {

        public UserCredential MakeCredentialsRequest()
        {
            UserCredential credential;
            var dirPath = Assembly.GetExecutingAssembly().CodeBase;
            dirPath = Path.GetDirectoryName(dirPath);
            string pathfile= Path.Combine(dirPath, "client_secret.json");
            string localPath = new Uri(pathfile).LocalPath;
            using (var stream = new FileStream(localPath, FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

                credPath = Path.Combine(credPath, ".credentials/drive-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets,
                    ApplicationConfig.Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                return credential;
            }
        }
       
    }
}
