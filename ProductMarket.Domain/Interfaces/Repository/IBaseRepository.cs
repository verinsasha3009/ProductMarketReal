using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarket.Domain.Interfaces.Repository
{
    public interface IBaseRepository<TEntity> : ISaveChanges where TEntity : class
    {
        Task<TEntity> CreateAsync(TEntity Entity);
        TEntity Update(TEntity Entity);
        void Remove(TEntity Entity);
        IQueryable<TEntity> GetAll();
    }
}
