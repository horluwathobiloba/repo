using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upskillz_invoice_mgt_Domain.Entities;
using Upskillz_invoice_mgt_Domain.IRepository.Base;

namespace Upskillz_invoice_mgt_Domain.IRepository
{
    public interface IAdminRepository : IGenericRepository<AppUser>
    {
    }
}
