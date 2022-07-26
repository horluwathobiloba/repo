using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System.Data.Common;
using System;
using System.Collections.Generic;

namespace OnyxDoc.SubscriptionService.Application.Common.Interfaces
{
    public interface IApplicationDbContextHelper
    {
        Task<List<T>> SqlQueryAsync<T>(string query, Func<DbDataReader, T> map);
        List<T> SqlQuery<T>(string query, Func<DbDataReader, T> map);
        Task<List<T>> ExecuteSqlCommandAsync<T>(string query, bool InvokeTxn = false);
        List<T> ExecuteSqlCommand<T>(string query, bool InvokeTxn = false);

    }
}
