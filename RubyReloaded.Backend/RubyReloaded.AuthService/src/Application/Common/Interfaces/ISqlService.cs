using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Common.Interfaces
{
    public interface ISqlService
    {
        Task<Result> InsertAdminPermissions(string tableName, List<RolePermissionsDT> permissions);
    }
}