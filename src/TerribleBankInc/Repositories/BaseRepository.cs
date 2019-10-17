using System;
using System.Linq;
using System.Threading.Tasks;
using TerribleBankInc.Data;
using TerribleBankInc.Models;
using TerribleBankInc.Models.Entities;
using TerribleBankInc.Repositories.Interfaces;

namespace TerribleBankInc.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T: BaseEntity
    {
        private readonly TerribleBankDbContext _context;

        public BaseRepository(TerribleBankDbContext context)
        {
            _context = context ?? throw new ArgumentNullException();
        }

        public async Task<T> FindAsync(int id)
        {
            return await _context.FindAsync<T>(id);
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsQueryable();
        }

        public async Task<T> UpdateAsync(T entity)
        {
            var result = _context.Update(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<T> AddAsync(T entity)
        {
            var result = await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            var found = (await FindAsync(entity.ID)) != null;

            if (found)
            {
                _context.Remove(entity);
                await _context.SaveChangesAsync();
            }

            return found;
        }
    }
}