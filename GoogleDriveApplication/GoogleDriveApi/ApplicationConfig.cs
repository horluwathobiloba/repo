using Google.Apis.Drive.v3;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleDriveApi
{
   internal static class ApplicationConfig
    {
       public static string[] Scopes = { DriveService.Scope.DriveReadonly };
       public static string ApplicationName = "Drive API .NET Quickstart";

    }
}
