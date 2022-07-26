using System.Threading.Tasks;

namespace Upskillz_invoice_mgt_Domain.IRepository.Base
{
    public interface IGenericRepository<T> where T : class
    {
        Task InsertAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveAsync();
    }
}
