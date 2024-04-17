using Microsoft.EntityFrameworkCore;
using ProductMarket.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarket.DAL.Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;
        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<TEntity> CreateAsync(TEntity Entity)
        {
            if (Entity == null) { throw new ArgumentNullException(nameof(Entity)); }
            await _context.AddAsync(Entity);
            await _context.SaveChangesAsync();
            return Entity;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>();
        }

        public void Remove(TEntity Entity)
        {
            if(Entity == null) { throw new ArgumentNullException( nameof(Entity)); }
            _context.Remove(Entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public TEntity Update(TEntity Entity)
        {
            if(Entity == null) { throw new ArgumentNullException(nameof(Entity)); }
            _context.Update(Entity);
            return Entity;
        }
    }
}
