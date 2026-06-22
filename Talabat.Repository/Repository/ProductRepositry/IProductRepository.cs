using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;

namespace Talabat.Repository.Repository.ProductRepositry
{
    public interface IProductRepository: IGenericRepository<Product>
    {
     public Task<IReadOnlyList<Product>> GetProducts(string? search=null,string sort="name",string ?order="asc",int? ProductBrandId=null ,int? CategoryId=null, int pageIndex=1,int pageSize=10 );
        public Task<int> GetProductsCountAsync(string? search = null);
        
       
    }
}


