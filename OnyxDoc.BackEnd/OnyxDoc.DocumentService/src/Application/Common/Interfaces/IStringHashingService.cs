using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Application.Common.Interfaces
{
   public interface IStringHashingService
    {
        public string CreateDESStringHash(string input);
        public object DecodeDESStringHash(string input);
    }
}
