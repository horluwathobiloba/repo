using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Common; 
using Microsoft.EntityFrameworkCore.Diagnostics;
using RubyReloaded.SubscriptionService.Application.Common.Interfaces;
using RubyReloaded.SubscriptionService.Application;
using Microsoft.EntityFrameworkCore;
using System.Data;
using ReventInject;

namespace RubyReloaded.SubscriptionService.Infrastructure.Persistence
{

    public class ApplicationDbContextHelper
    {
        private readonly IApplicationDbContext _context;

        public ApplicationDbContextHelper(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<T>> FromSqlQueryAsync<T>(string query, Func<DbDataReader, T> map) where T : IApplicationDbContext
        {
            try
            {
                return await _context.SqlQueryAsync<T>(query, map);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<T> FromSqlQuery<T>(string query, Func<DbDataReader, T> map) where T : IApplicationDbContext
        {
            try
            {
                return this.CastTo<T>().SqlQuery<T>(query, map);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<T>> SqlQueryAsync<T>(string query, Func<DbDataReader, T> map) where T : DbSet<T>
        {
            try
            {
                return await this.CastTo<DbSet<T>>().SqlQueryAsync<T>(query, map);
            }
            catch (Exception ex)           {
                throw ex;
            }
        }

        public List<T> SqlQuery<T>(string query, Func<DbDataReader, T> map) where T : DbSet<T>
        {
            try
            {
                return this.CastTo<DbSet<T>>().SqlQuery<T>(query, map);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ExecuteSqlCommandAsync<T>(string query, bool InvokeTxn = false) where T : IApplicationDbContext
        {
            try
            {
                return await _context.ExecuteSqlCommandAsync<T>(query, InvokeTxn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ExecuteSqlCommand<T>(string query, bool InvokeTxn = false) where T : IApplicationDbContext
        {
            try
            {
                return _context.ExecuteSqlCommand<T>(query, InvokeTxn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }      

        public  bool SqlCommand<T>(string query, Func<DbDataReader, T> map, bool InvokeTxn = false) where T : DbSet<T>
        {
            try
            {
                return this.CastTo<T>().SqlCommand<T>(query, InvokeTxn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> SqlCommandAsync<T>(string query, Func<DbDataReader, T> map, bool InvokeTxn = false) where T : DbSet<T>
        {
            try
            {
                return await this.CastTo<T>().SqlCommandAsync<T>(query, InvokeTxn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }


    public static class ApplicationDbContextExtension
    {
        public static DbTransaction Txn { get; private set; }

        public static async Task<List<T>> SqlQueryAsync<T>(this IApplicationDbContext _context, string query, Func<DbDataReader, T> map)
        {
            try
            {
                ApplicationDbContext context = (ApplicationDbContext)_context;

                using (context)
                {
                    using (var command = context.Database.GetDbConnection().CreateCommand())
                    {
                        command.CommandText = query;
                        command.CommandType = CommandType.Text;

                        context.Database.OpenConnection();
                        var entities = new List<T>();

                        await Task.Run(() =>
                        {
                            using (var result = command.ExecuteReader())
                            {
                                while (result.Read())
                                {
                                    entities.Add(map(result));
                                }
                            }
                        });
                        return entities;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<T> SqlQuery<T>(this IApplicationDbContext _context, string query, Func<DbDataReader, T> map)
        {
            try
            {
                ApplicationDbContext context = (ApplicationDbContext)_context;

                using (context)
                {
                    using (var command = context.Database.GetDbConnection().CreateCommand())
                    {
                        command.CommandText = query;
                        command.CommandType = CommandType.Text;

                        context.Database.OpenConnection();

                        using (var result = command.ExecuteReader())
                        {
                            var entities = new List<T>();

                            while (result.Read())
                            {
                                entities.Add(map(result));
                            }
                            return entities;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<List<T>> SqlQueryAsync<T>(this DbSet<T> dbSet, string query, Func<DbDataReader, T> map) where T : class
        {
            try
            {
                using (var context = dbSet.GetDbContext())
                {
                    using (var command = context.Database.GetDbConnection().CreateCommand())
                    {
                        command.CommandText = query;
                        command.CommandType = CommandType.Text;

                        context.Database.OpenConnection();
                        var entities = new List<T>();

                        await Task.Run(() =>
                        {
                            using (var result = command.ExecuteReader())
                            {
                                while (result.Read())
                                {
                                    entities.Add(map(result));
                                }
                            }
                        });
                        return entities;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<T> SqlQuery<T>(this DbSet<T> dbSet, string query, Func<DbDataReader, T> map) where T : class
        {
            try
            {
                using (var context = dbSet.GetDbContext())
                {
                    using (var command = context.Database.GetDbConnection().CreateCommand())
                    {
                        command.CommandText = query;
                        command.CommandType = CommandType.Text;

                        context.Database.OpenConnection();
                        var entities = new List<T>();

                        using (var result = command.ExecuteReader())
                        {
                            while (result.Read())
                            {
                                entities.Add(map(result));
                            }
                        }
                        return entities;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<bool> ExecuteSqlCommandAsync<T>(this IApplicationDbContext _context, string query, bool InvokeTxn = false)
        {
            DbTransaction Txn;

            try
            {
                var context = (ApplicationDbContext)_context;

                using (context)
                {
                    using (var connection = context.Database.GetDbConnection())
                    {
                        using (var command = connection.CreateCommand())
                        {
                            try
                            {
                                command.CommandText = query;
                                command.CommandType = CommandType.Text;

                                context.Database.OpenConnection();
                                var count = 0;
                                if (InvokeTxn)
                                {
                                    Txn = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                                    command.Transaction = Txn;
                                    var entities = new List<T>();

                                    count = await command.ExecuteNonQueryAsync();
                                    try
                                    {
                                        await Txn.CommitAsync(default(CancellationToken));
                                        return count > 0 ? true : false;
                                    }
                                    catch (Exception ex)
                                    {
                                        await Txn.RollbackAsync();
                                        throw ex;
                                    }
                                }
                                else
                                {
                                    count = await command.ExecuteNonQueryAsync();
                                    return count > 0 ? true : false;
                                }
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool ExecuteSqlCommand<T>(this IApplicationDbContext _context, string query, bool InvokeTxn = false)
        {
            DbTransaction Txn;

            try
            {
                var context = (ApplicationDbContext)_context;

                using (context)
                {
                    using (var connection = context.Database.GetDbConnection())
                    {
                        using (var command = connection.CreateCommand())
                        {
                            try
                            {
                                command.CommandText = query;
                                command.CommandType = CommandType.Text;
                                context.Database.OpenConnection();
                                var count = 0;

                                if (InvokeTxn)
                                {
                                    Txn = context.Database.GetDbConnection().BeginTransaction(IsolationLevel.ReadCommitted);
                                    command.Transaction = Txn;

                                    count = command.ExecuteNonQuery();

                                    try
                                    {
                                        Txn.Commit();
                                        return count > 0 ? true : false;
                                    }
                                    catch (Exception ex)
                                    {
                                        Txn.Rollback();
                                        throw ex;
                                    }
                                }
                                else
                                {
                                    count = command.ExecuteNonQuery();
                                    return count > 0 ? true : false;
                                }
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<bool> SqlCommandAsync<T>(this DbSet<T> dbSet, string query, bool InvokeTxn = false) where T : class
        {
            DbTransaction Txn;

            try
            {
                using (var context = dbSet.GetDbContext())
                {
                    using (var connection = context.Database.GetDbConnection())
                    {
                        using (var command = connection.CreateCommand())
                        {
                            try
                            {
                                command.CommandText = query;
                                command.CommandType = CommandType.Text;

                                context.Database.OpenConnection();
                                var count = 0;
                                if (InvokeTxn)
                                {
                                    Txn = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                                    command.Transaction = Txn;
                                    count = await command.ExecuteNonQueryAsync();
                                    try
                                    {
                                        await Txn.CommitAsync(default(CancellationToken));
                                        return count > 0 ? true : false;
                                    }
                                    catch (Exception ex)
                                    {
                                        await Txn.RollbackAsync();
                                        throw ex;
                                    }
                                }
                                else
                                {
                                    count = await command.ExecuteNonQueryAsync();
                                    return count > 0 ? true : false;
                                }
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool SqlCommand<T>(this DbSet<T> dbSet, string query, bool InvokeTxn = false) where T : class
        {
            DbTransaction Txn;

            try
            {
                using (var context = dbSet.GetDbContext())
                {
                    using (var connection = context.Database.GetDbConnection())
                    {
                        using (var command = connection.CreateCommand())
                        {
                            try
                            {
                                command.CommandText = query;
                                command.CommandType = CommandType.Text;
                                context.Database.OpenConnection();
                                var count = 0;

                                if (InvokeTxn)
                                {
                                    Txn = context.Database.GetDbConnection().BeginTransaction(IsolationLevel.ReadCommitted);
                                    command.Transaction = Txn;

                                    count = command.ExecuteNonQuery();

                                    try
                                    {
                                        Txn.Commit();
                                        return count > 0 ? true : false;
                                    }
                                    catch (Exception ex)
                                    {
                                        Txn.Rollback();
                                        throw ex;
                                    }
                                }
                                else
                                {
                                    count = command.ExecuteNonQuery();
                                    return count > 0 ? true : false;
                                }
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

