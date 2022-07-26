using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Common.Interfaces
{
    public interface IUtilityService
    {
        Task<DataTable> ToDataTable<T>(List<T> items);
    }
}
