using OnyxDoc.DocumentService.Domain.Common;
using OnyxDoc.DocumentService.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnyxDoc.DocumentService.Domain.Entities
{
   
    public class FolderShareDetail : AuditableEntity
    {
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public string SharedWithName { get; set; }
        public string Email { get; set; }
        public FilePermission FilePermission { get; set; }
        public int FolderId { get; set; }
    }
}