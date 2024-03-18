using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TerribleBankInc.Data;
using TerribleBankInc.Models;
using TerribleBankInc.Models.Entities;
using TerribleBankInc.Repositories.Interfaces;

namespace TerribleBankInc.Repositories;

public class BaseRepository<T> : IBaseRepository<T>
    where T : BaseEntity
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
        bool found = await FindAsync(entity.ID) != null;

        if (found)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        return found;
    }

    public async Task<List<T>> Get(
        Expression<Func<T, bool>> predicate,
        string includeProperties = null
    )
    {
        var query = _context.Set<T>().Where(predicate);
        if (!string.IsNullOrEmpty(includeProperties))
            query = query.Include(includeProperties);

        return await query.ToListAsync();
    }
}
