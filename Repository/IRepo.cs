using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository
{
    public interface IRepo<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(int id);
        Task<T> InsertAsync(T entity);
        Task<T> UpdateAsync(T entity, int id);
        Task<int> DeleteAsync(T entity);
    }
}
