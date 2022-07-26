using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Common;
using Onyx.ContractService.Infrastructure.Persistence; 
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Onyx.ContractService.Infrastructure.Persistence
{

    //public static class ApplicationDbContextGetter
    //{
    //    public static DbContext GetDbContext<T>(this DbSet<T> dbSet) where T : class
    //    {
    //        var infrastructure = dbSet as IInfrastructure<IServiceProvider>;
    //        var serviceProvider = infrastructure.Instance;
    //        var currentDbContext = serviceProvider.GetService(typeof(ICurrentDbContext))
    //                                   as ICurrentDbContext;
    //        return currentDbContext.Context;
    //    }
    //}
}

