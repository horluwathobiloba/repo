﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Common.Interfaces
{
   public interface IBearerTokenService
    {
        Task<string> GetBearerToken(string apiUrl, object requestObject);
    }
}
