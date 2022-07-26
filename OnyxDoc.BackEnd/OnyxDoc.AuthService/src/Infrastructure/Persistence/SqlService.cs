using Microsoft.Extensions.Configuration;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Application.Common.Models;
using OnyxDoc.AuthService.Domain.Entities;
using OnyxDoc.AuthService.Domain.Enums;
using OnyxDoc.AuthService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using System.Threading.Tasks;

namespace OnyxDoc.AuthService.Infrastructure.Persistence
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
        public async Task<Result> InsertPermissions(string tableName, List<RolePermissionsDT> permissions)
        {

            try
            {
                var connectionString = _configuration["ConnectionStrings:DefaultConnection"];
                RolePermission permission = new RolePermission();
                using (var conn = new SqlConnection(connectionString))
                {
                    using (var copy = new SqlBulkCopy(connectionString))
                    {

                        DataTable dataTable = await _utilityService.ToDataTable(permissions);
                        copy.DestinationTableName = tableName;
                        // Add mappings so that the column order doesn't matter
                        copy.ColumnMappings.Add(nameof(permission.Id), "Id");
                        copy.ColumnMappings.Add(nameof(permission.SubscriberId), "SubscriberId");
                        copy.ColumnMappings.Add(nameof(permission.RoleId), "RoleId");
                        copy.ColumnMappings.Add(nameof(permission.Name), "Name");
                        copy.ColumnMappings.Add(nameof(permission.CreatedByEmail), "CreatedByEmail");
                        copy.ColumnMappings.Add(nameof(permission.CreatedDate), "CreatedDate");
                        copy.ColumnMappings.Add(nameof(permission.LastModifiedById), "LastModifiedById");
                        copy.ColumnMappings.Add(nameof(permission.LastModifiedDate), "LastModifiedDate");
                        copy.ColumnMappings.Add(nameof(permission.Status), "Status");
                        copy.ColumnMappings.Add(nameof(permission.RoleAccessLevel), "RoleAccessLevel");
                        copy.ColumnMappings.Add(nameof(permission.Permission), "Permission");

                        copy.WriteToServer(dataTable);
                    }
                }
                return Result.Success("Inserting Permissions was successful");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Inserting Permissions  was not successful", ex?.Message ?? ex?.InnerException.Message });
            }

        }
    }
}
