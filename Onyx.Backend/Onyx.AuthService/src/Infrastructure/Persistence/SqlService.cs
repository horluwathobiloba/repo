using Microsoft.Extensions.Configuration;
using Onyx.AuthService.Application.Common.Interfaces;
using Onyx.AuthService.Application.Common.Models;
using Onyx.AuthService.Domain.Entities;
using Onyx.AuthService.Domain.Enums;
using Onyx.AuthService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using System.Threading.Tasks;

namespace Onyx.AuthService.Infrastructure.Persistence
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
                RolePermission permission = new RolePermission();
                using (var conn = new SqlConnection(connectionString))
                {
                    using (var copy = new SqlBulkCopy(connectionString))
                    {

                        DataTable dataTable = await _utilityService.ToDataTable(permissions);
                        copy.DestinationTableName = tableName;
                        // Add mappings so that the column order doesn't matter
                        copy.ColumnMappings.Add(nameof(permission.Id), "Id");
                        copy.ColumnMappings.Add(nameof(permission.OrganizationId), "OrganizationId");
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
                return Result.Failure(new string[] { "Inserting Admin Permissions  was not successful", ex?.Message + ex?.InnerException.Message });
            }
            //using (var conn = new SqlConnection(connectionString))
            //{

            //    using (var cmd = new SqlCommand("InsertPermissions", conn))
            //    {
            //        foreach (var item in permissions)
            //        {
            //            cmd.CommandType = CommandType.StoredProcedure;
            //            cmd.Parameters.Clear();
            //            cmd.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.Int, 100)).Value = item.OrganizationId;
            //            cmd.Parameters.Add(new SqlParameter("@roleid", SqlDbType.Int, 100)).Value = item.RoleId;
            //            cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 50)).Value = item.Name;
            //            cmd.Parameters.Add(new SqlParameter("@CreatedBy", SqlDbType.NVarChar, 50)).Value = item.CreatedBy;
            //            cmd.Parameters.Add(new SqlParameter("@CreatedDate", SqlDbType.DateTime, 100)).Value = item.CreatedDate;
            //            cmd.Parameters.Add(new SqlParameter("@LastModifiedBy", SqlDbType.NVarChar, 50)).Value = item.LastModifiedBy;
            //            cmd.Parameters.Add(new SqlParameter("@LastModifiedDate", SqlDbType.DateTime, 100)).Value = item.LastModifiedDate;
            //            cmd.Parameters.Add(new SqlParameter("@status", SqlDbType.Int, 100)).Value = Convert.ToInt32(item.Status);
            //            cmd.Parameters.Add(new SqlParameter("@AccessLevel", SqlDbType.Int, 100)).Value = item.AccessLevel;
            //            cmd.Parameters.Add(new SqlParameter("@Permission", SqlDbType.NVarChar, 100)).Value = item.Permission;
            //            cmd.Parameters.Add(new SqlParameter("@retval", SqlDbType.Int, 100));
            //            cmd.Parameters["@retval"].Direction = ParameterDirection.Output;
            //            try
            //            {
            //                await conn.OpenAsync();
            //                await cmd.PrepareAsync();
            //                count= cmd.ExecuteNonQuery();
            //                string RetVal = cmd.Parameters["@retval"].Value.ToString();
            //            }
            //            catch (SqlException sqe)
            //            {
            //                throw new Exception(sqe.Message);
            //            }

        }
    }
}
