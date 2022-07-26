using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Application.Common.Models
{
   public class CreateFolderListRequest
    {
     
        public int RootFolderId { get; set; }
        public int ParentFolderId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public FolderType FolderType { get; set; }
        public List<CreateFolderRecipientRequest> CreateFolderRecipientRequests { get; set; }
    }
}
