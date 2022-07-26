using System.ComponentModel.DataAnnotations;

namespace DriveApp.Dto
{
    public class FolderRequestDto
    {
        [Required]
        public string Name { get; set; }
    }
}
