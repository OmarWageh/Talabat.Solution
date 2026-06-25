using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository.Data;

namespace Talabat.Repository.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext  _appDbContext;
        private Dictionary<string, object>_repositories;
        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;

            _repositories = new Dictionary<string, object>();
        }
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var key = typeof(TEntity).Name;
            if(!_repositories.ContainsKey(key))
            {
                var repository = new GenericRepository<TEntity>(_appDbContext);

                _repositories.Add(key, repository);
            }
            return _repositories[key] as IGenericRepository<TEntity>;
        }
        public async Task<int> CompleteAsync() => await _appDbContext.SaveChangesAsync();




        public async ValueTask DisposeAsync() =>await _appDbContext.DisposeAsync();
       

        
    }
}
