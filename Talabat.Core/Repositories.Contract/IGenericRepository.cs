using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Repositories.Contract
{
    public interface IGenericRepository<T> where T :BaseEntity
    {
        public Task<T?> GetAsync(int id);
        public Task<IReadOnlyList<T>> GetAllAsync();
        public Task AddAsync(T entity);
        public void UpdateAsync(T entity);
        public void  DeleteAsync(T entity);
    }
}
