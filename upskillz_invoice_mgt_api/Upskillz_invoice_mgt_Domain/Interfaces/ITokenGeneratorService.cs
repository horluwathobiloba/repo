using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upskillz_invoice_mgt_Domain.Entities;

namespace Upskillz_invoice_mgt_Domain.Interfaces
{
    public interface ITokenGeneratorService
    {
        Task<string> GenerateToken(AppUser user);
    }
}
