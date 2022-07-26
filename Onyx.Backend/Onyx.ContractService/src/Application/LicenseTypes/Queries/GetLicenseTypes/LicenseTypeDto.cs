﻿using AutoMapper;
using Onyx.ContractService.Application.Common.Mappings;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.LicenseTypes.Queries.GetLicenseTypes
{
    public class LicenseTypeDto : IMapFrom<Domain.Entities.LicenseType>
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string OrganisationId { get; set; }
        public string OrganisationName { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.LicenseType, LicenseTypeDto>();
            profile.CreateMap<LicenseTypeDto, Domain.Entities.LicenseType>();
        }
    }
}
