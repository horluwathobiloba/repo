using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Common;
using OnyxDoc.FormBuilderService.Application;
using ReventInject;
using Microsoft.EntityFrameworkCore.Diagnostics;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace OnyxDoc.FormBuilderService.Application
{
   
    public static class ApplicationDbContextGetter
    {
        public static DbContext GetDbContext<T>(this DbSet<T> dbSet) where T : class
        {
            var infrastructure = dbSet as IInfrastructure<IServiceProvider>;
            var serviceProvider = infrastructure.Instance;
            var currentDbContext = serviceProvider.GetService(typeof(ICurrentDbContext))
                                       as ICurrentDbContext;
            return currentDbContext.Context;
        }
    }
}

