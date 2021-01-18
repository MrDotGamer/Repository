using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class Repo<T> : IRepo<T> where T : class
    {
        private readonly DbContext _context;
        private DbSet<T> _entities;

        public Repo(DbContext context)
        {
            this._context = context;
            _entities = context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }
        public async Task<T> GetAsync(int id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task<T> InsertAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _entities.Add(entity);
            await _context.SaveChangesAsync();
            return entity;

        }

        public async Task<T> UpdateAsync(T updated, int id)
        {
            if (updated == null)
                throw new ArgumentNullException("entity");
            var entity = await _entities.FindAsync(id);
            if (entity != null)
            {
                _context.Entry(entity).CurrentValues.SetValues(updated);
                await _context.SaveChangesAsync();
            }
            return entity;
        }

        public async Task<int> DeleteAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _entities.Remove(entity);
            return await _context.SaveChangesAsync();
        }
    }
}
