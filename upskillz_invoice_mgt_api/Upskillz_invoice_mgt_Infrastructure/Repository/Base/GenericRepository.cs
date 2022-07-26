using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Upskillz_invoice_mgt_Domain.IRepository.Base;
using Upskillz_invoice_mgt_Infrastructure.ContextClass;

namespace Upskillz_invoice_mgt_Infrastructure.Repository.Base
{
    class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly UpskillDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(UpskillDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task InsertAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
