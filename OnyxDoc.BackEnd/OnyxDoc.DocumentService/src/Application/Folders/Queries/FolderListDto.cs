using AutoMapper;
using OnyxDoc.DocumentService.Application.Common.Mappings;
using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Collections.Generic;

namespace OnyxDoc.DocumentService.Application.Folders.Queries.GetFolders
{
    public class FolderListDto : IMapFrom<Domain.Entities.Folder>
    {
        public string FolderPath { get; set; }
        public FolderType FolderType { get; set; }
        public int RootFolderId { get; set; }
        public int ParentFolderId { get; set; }
        public bool IsDuplicated { get; set; }
        public bool IsArchived { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<Domain.Entities.Document> Documents { get; set; }
        public ICollection<Domain.Entities.Folder> Folders { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByEmail { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public string DeviceId { get; set; }

        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Folder, FolderListDto>();
            profile.CreateMap<FolderListDto, Domain.Entities.Folder>();
        }
    }
}
