using Microsoft.Extensions.Configuration;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Entities;
using RubyReloaded.AuthService.Domain.Enums;
using RubyReloaded.AuthService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Infrastructure.Persistence
{
    public class SqlService : ISqlService
    {

        private readonly IConfiguration _configuration;
        private readonly IUtilityService _utilityService;
        public SqlService(IConfiguration configuration, IUtilityService utilityService)
        {
            _configuration = configuration;
            _utilityService = utilityService;
        }
        public async Task<Result> InsertAdminPermissions(string tableName, List<RolePermissionsDT> permissions)
        {

            try
            {
                var connectionString = _configuration["ConnectionStrings:DefaultConnection"];
                CooperativeRolePermission permission = new CooperativeRolePermission();
                using (var conn = new SqlConnection(connectionString))
                {
                    using (var copy = new SqlBulkCopy(connectionString))
                    {

                        DataTable dataTable = await _utilityService.ToDataTable(permissions);
                        copy.DestinationTableName = tableName;
                        // Add mappings so that the column order doesn't matter
                        copy.ColumnMappings.Add(nameof(permission.Id), "Id");
                        copy.ColumnMappings.Add(nameof(permission.CooperativeId), "CooperativeId");
                        copy.ColumnMappings.Add(nameof(permission.RoleId), "RoleId");
                        copy.ColumnMappings.Add(nameof(permission.Name), "Name");
                        copy.ColumnMappings.Add(nameof(permission.CreatedBy), "CreatedBy");
                        copy.ColumnMappings.Add(nameof(permission.CreatedDate), "CreatedDate");
                        copy.ColumnMappings.Add(nameof(permission.LastModifiedBy), "LastModifiedBy");
                        copy.ColumnMappings.Add(nameof(permission.LastModifiedDate), "LastModifiedDate");
                        copy.ColumnMappings.Add(nameof(permission.Status), "Status");
                        copy.ColumnMappings.Add(nameof(permission.AccessLevel), "AccessLevel");
                        copy.ColumnMappings.Add(nameof(permission.Permission), "Permission");

                        copy.WriteToServer(dataTable);
                    }
                }
                return Result.Success("Inserting Admin Permissions was successful");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Inserting Admin Permissions  was not successful", ex?.Message ?? ex?.InnerException.Message });
            }

        }
    }
}
