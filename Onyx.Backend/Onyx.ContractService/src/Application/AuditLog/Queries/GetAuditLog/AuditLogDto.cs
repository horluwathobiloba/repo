using AutoMapper;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.ContractDuration.Queries.GetAuditLog
{
        public class AuditLogDto
        {
        public string UserId { get; set; }
        public string Module { get; set; }
        public string Action { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public int JobFunctionId { get; set; }
        public string JobFunctionName { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.AuditLog, AuditLogDto>();
            profile.CreateMap<AuditLogDto, Domain.Entities.AuditLog>();
        }
    }
}
