using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;

namespace GoogleDriveApi
{
   internal class CreateApiService
    {
        CreateConnection CreateConnection = new CreateConnection();
        public DriveService CallAppiService()
        {
           
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = CreateConnection.MakeCredentialsRequest(),
                ApplicationName = ApplicationConfig.ApplicationName
                
            });

            return service;
        }
    }
}
