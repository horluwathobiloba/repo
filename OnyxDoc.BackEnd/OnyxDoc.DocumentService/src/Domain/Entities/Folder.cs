using OnyxDoc.DocumentService.Domain.Common;
using OnyxDoc.DocumentService.Domain.Enums;
using System.Collections.Generic;

namespace OnyxDoc.DocumentService.Domain.Entities
{
    public class Folder : AuditableEntity
    {
        public string FolderPath { get; set; }
        public FolderType FolderType { get; set; }
        public int RootFolderId { get; set; }
        public int ParentFolderId { get; set; }
        public FolderStatus FolderStatus { get; set; }
        public string FolderStatusDesc { get; set; }
        public bool IsDuplicated { get; set; }
        //public bool IsArchived { get; set; }
        //public bool IsDeleted { get; set; }
        public ICollection<FolderShareDetail> FolderShareDetails { get; set; }
        public ICollection<Recipient> Recipients { get; set; }
        public ICollection<Document> Documents { get; set; }
    }
}
