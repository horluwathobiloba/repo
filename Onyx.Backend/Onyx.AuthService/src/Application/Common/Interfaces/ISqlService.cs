using Onyx.AuthService.Application.Common.Models;
using Onyx.AuthService.Domain.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Onyx.AuthService.Application.Common.Interfaces
{
    public interface ISqlService
    {
        Task<Result> InsertAdminPermissions(string tableName, List<RolePermissionsDT> permissions);
    }
}