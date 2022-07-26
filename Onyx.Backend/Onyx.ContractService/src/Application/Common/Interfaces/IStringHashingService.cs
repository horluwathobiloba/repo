using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.Common.Interfaces
{
   public interface IStringHashingService
    {
        public string CreateDESStringHash(string input);
        public object DecodeDESStringHash(string input);
    }
}
