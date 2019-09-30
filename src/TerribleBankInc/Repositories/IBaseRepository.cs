using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TerribleBankInc.Models;

namespace TerribleBankInc.Repositories
{
    public interface IBaseRepository<T> where T: BaseEntity
    {
        Task<T> FindAsync(int id);
        IQueryable<T> GetAll();
        Task<T> UpdateAsync(T entity);
        Task<T> AddAsync(T entity);
        Task<bool> DeleteAsync(T entity);
    }
}