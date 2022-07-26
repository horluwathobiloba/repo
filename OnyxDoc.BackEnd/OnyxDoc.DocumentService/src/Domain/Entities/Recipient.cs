using OnyxDoc.DocumentService.Domain.Common;
using OnyxDoc.DocumentService.Domain.Enums;
using System.Collections.Generic;

namespace OnyxDoc.DocumentService.Domain.Entities
{
    public class Recipient : AuditableEntity //Change to folder recipients
    {
        public int DocumentId { get; set; }
        //public int FolderId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string DocumentMessage { get; set; }
        public int Rank { get; set; }
        public string RecipientCategory { get; set; }
        public string DocumentSigningUrl { get; set; }
        public ICollection<RecipientAction> RecipientActions { get; set; } = new List<RecipientAction>();

        public FilePermission FilePermission { get; set; }
        public FileType FileType { get; set; }
        public Folder Folder { get; set; }

    }
}
