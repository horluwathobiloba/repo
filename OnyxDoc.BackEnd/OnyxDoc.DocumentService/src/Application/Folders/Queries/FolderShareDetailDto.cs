using AutoMapper;
using OnyxDoc.DocumentService.Domain.Enums;

namespace OnyxDoc.DocumentService.Application.Folders.Queries
{
    internal class FolderShareDetailDto
    {
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public string Email { get; set; }
        public FilePermission FilePermission { get; set; }
        public int FolderId { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.FolderShareDetail, FolderShareDetailDto>();
            profile.CreateMap<FolderShareDetailDto, Domain.Entities.FolderShareDetail>();
        }
    }
}
