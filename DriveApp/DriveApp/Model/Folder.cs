using System.Collections.Generic;

namespace DriveApp.Model
{
    public class Folder : BaseEntity
    {       
        public string Name { get; set; }
        public int FolderId { get; set; }
        public ICollection<Folder> Folders { get; set; }
        public ICollection<File> Files { get; set; }
    }
}
