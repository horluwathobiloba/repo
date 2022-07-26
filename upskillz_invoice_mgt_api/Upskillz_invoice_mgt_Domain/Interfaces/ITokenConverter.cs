using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upskillz_invoice_mgt_Domain.Interfaces
{
    public interface ITokenConverter
    {
        public string EncodeToken(string token);
        public string DecodeToken(string token);
    }
}
