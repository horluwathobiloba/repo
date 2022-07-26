using Onyx.ContractService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Onyx.ContractService.Domain.Enums;
using System.Data.Common;
using System;
using System.Collections.Generic;

namespace Onyx.ContractService.Application.Common.Interfaces
{
    public interface IApplicationDbContextHelper
    {
        Task<List<T>> SqlQueryAsync<T>(string query, Func<DbDataReader, T> map);
        List<T> SqlQuery<T>(string query, Func<DbDataReader, T> map);
        Task<List<T>> ExecuteSqlCommandAsync<T>(string query, bool InvokeTxn = false);
        List<T> ExecuteSqlCommand<T>(string query, bool InvokeTxn = false);

        //Task<object> SqlQueryAsync<T>(DbSet<T> dbSet, string query, Func<DbDataReader, T> map) where T : class;
        //object SqlQuery<T>(DbSet<T> dbSet, string query, Func<DbDataReader, T> map) where T : class;
        //Task<object> SqlCommandAsync<T>(DbSet<T> dbSet, string query, bool InvokeTxn = false) where T : class;
        //object SqlCommand<T>(DbSet<T> dbSet, string query, bool InvokeTxn = false) where T : class;

    }
}
