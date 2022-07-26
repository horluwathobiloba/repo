using OnyxDoc.DocumentService.Domain.Enums;

namespace OnyxDoc.DocumentService.Application.Common.Models
{
    public class CreateFolderRecipientRequest
    {
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public string SharedWithName { get; set; }
        public string Email { get; set; }
        public FilePermission FilePermission { get; set; }
    }
}
