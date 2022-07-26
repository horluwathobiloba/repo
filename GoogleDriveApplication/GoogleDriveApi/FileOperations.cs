using Google.Apis.Drive.v3;
using System;
using System.Collections.Generic;
using System.Text;
using Models;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Requests;
using System.Net;
using Google.Apis.Download;

namespace GoogleDriveApi
{
    public class FileOperations
    {

        private FilesResource.ListRequest ParameterizedRequest()
        {
            CreateApiService createApiService = new CreateApiService();
            FilesResource.ListRequest listRequest = createApiService.CallAppiService().Files.List();
            listRequest.PageSize = 10;
            listRequest.Fields = "nextPageToken, files(id, name)";
            return listRequest;
        }
        public List<UserFiles> FetchListOfFiles()
        {
            List<UserFiles> userFiles = new List<UserFiles>();
            IList<File> files = ParameterizedRequest().Execute().Files;

            if (files != null && files.Count > 0)
            {

                foreach (var file in files)
                {
                    userFiles.Add(new UserFiles { FileName = file.Name, FileID = file.Id, CreationTime = file.CreatedTime });
                }
            }
            return userFiles;
        }
        public File UploadFile(String title, String description, String mimeType, String filename)
        {
            // File's metadata.
            File body = new File();
            body.Name = title;
            body.Description = description;
            body.MimeType = mimeType;
            // File's content.
            byte[] byteArray = System.IO.File.ReadAllBytes(filename);
            System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);
            try
            {
                CreateApiService createApiService = new CreateApiService();
                FilesResource.CreateMediaUpload request = createApiService.CallAppiService().Files.Create(body, stream, mimeType);
                request.Upload();
                File file = request.ResponseBody;
                return file;
            }
            catch (Exception e)
            {
                throw e;

            }
        }
        public void DownloadFile(string fileId, string fileExtension)
        {
            CreateApiService createApiService = new CreateApiService();
            var request = createApiService.CallAppiService().Files.Export(fileId, "application/pdf");
            request.MediaDownloader.ProgressChanged += (IDownloadProgress progress) =>{
            switch (progress.Status)
            {
                case DownloadStatus.Downloading:
                    {
                       // progress.BytesDownloaded;
                        break;
                    }
                case DownloadStatus.Completed:
                    {
                        Console.WriteLine("Download complete.");
                        break;
                    }
                case DownloadStatus.Failed:
                    {
                        Console.WriteLine("Download failed.");
                        break;
                    }
            }};
        }
        public void DeleteFile(string fileId)
        {
            CreateApiService createApiService = new CreateApiService();
            FilesResource.DeleteRequest request= createApiService.CallAppiService().Files.Delete(fileId);
            request.Execute();
        }

    }
}
