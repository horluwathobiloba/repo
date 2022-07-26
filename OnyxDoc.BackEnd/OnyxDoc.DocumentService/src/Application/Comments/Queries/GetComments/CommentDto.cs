using AutoMapper;
using OnyxDoc.DocumentService.Application.Common.Mappings;
using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Application.Comments.Queries.GetComments
{
    public class CommentDto: IMapFrom<Domain.Entities.Comment>
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public string Text { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Comment, CommentDto>();
            profile.CreateMap<CommentDto, Domain.Entities.Comment>();
        }
    }
}
