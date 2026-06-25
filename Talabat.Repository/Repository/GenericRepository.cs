using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository.Data;

namespace Talabat.Repository.Repository
{
    public class  GenericRepository<T>:IGenericRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _dbContext;

        public GenericRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(T entity)
        {
          if (entity == null)
 
                       throw new ArgumentNullException(nameof(entity));
            
            else
            {
               await _dbContext.AddAsync(entity);
               await  _dbContext.SaveChangesAsync();
            }

        }

        public void DeleteAsync(T entity)
        {
            _dbContext.Remove(entity);
        }
        public void UpdateAsync(T entity)
        {
            _dbContext.Update(entity);
        }
        public virtual async Task<IReadOnlyList<T>> GetAllAsync()
        {
            
            return await _dbContext.Set<T>().ToListAsync();
        }

        public virtual async Task<T?> GetAsync(int id)
        {
         
            return await _dbContext.Set<T>().FindAsync(id);
        }

       
    }
}
