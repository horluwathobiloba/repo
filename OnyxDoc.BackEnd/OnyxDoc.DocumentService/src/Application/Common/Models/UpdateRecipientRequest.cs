using OnyxDoc.DocumentService.Domain.Enums;

namespace OnyxDoc.DocumentService.Application.Common.Models
{
    public class UpdateRecipientRequest
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Rank { get; set; }
        public RecipientCategory RecipientCategory { get; set; }
    }
}
