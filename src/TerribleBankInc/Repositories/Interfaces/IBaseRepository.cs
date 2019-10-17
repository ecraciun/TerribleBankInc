using System.Linq;
using System.Threading.Tasks;
using TerribleBankInc.Models.Entities;

namespace TerribleBankInc.Repositories.Interfaces
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