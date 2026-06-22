using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository.Data;

namespace Talabat.Repository.Repository.ProductRepositry
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext _dbContext;

        public ProductRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IReadOnlyList<Product>> GetProducts(string? search = null, string? sort = "name", string? order = "asc", int? ProductBrandId = null, int? CategoryId = null, int pageIndex =1, int pageSize = 10)
        {
            var query = _dbContext.Products.Include(p => p.productBrand).Include(p => p.category).AsQueryable(); ; //Do Navigation

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(P => P.Name.ToLower().Contains(search.ToLower()));    //search by Name


            if (ProductBrandId.HasValue)
                query = query.Where(p => p.ProductBrandId == ProductBrandId.Value);    //The product's brand name is so-and-so

            if (CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == CategoryId.Value);       //The product's Category name is so-and-so

            sort = (sort ?? "name").ToLowerInvariant();       //default sort by name
            order = (order ?? "asc").ToLowerInvariant();        //default order asc
            bool desc = order == "desc";                        //determine the order direction

            switch (sort)             //determine the sort field
            {
                case "price":
                    query = desc ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price);
                    break;
                case "name":
                default:
                    query = desc ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name);
                    break;

            }
            // do validation for pageIndex and pageSize
            if (pageIndex < 1)
                pageIndex = 1;
            if (pageSize < 1)
                pageSize = 10;

            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);


            return await query.ToListAsync();
        }

        public Task<int> GetProductsCountAsync(string? search)
        {
            if (search == null)
            {
                return _dbContext.Products.CountAsync();
            }
            else
            {
                return _dbContext.Products.Where(p => p.Name.ToLower().Contains(search.ToLower())).CountAsync();
            }
        }

    }
}












