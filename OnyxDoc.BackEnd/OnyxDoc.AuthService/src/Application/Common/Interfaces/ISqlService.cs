using OnyxDoc.AuthService.Application.Common.Models;
using OnyxDoc.AuthService.Domain.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnyxDoc.AuthService.Application.Common.Interfaces
{
    public interface ISqlService
    {
        Task<Result> InsertPermissions(string tableName, List<RolePermissionsDT> permissions);
    }
}