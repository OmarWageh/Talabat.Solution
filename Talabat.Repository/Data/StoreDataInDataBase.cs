using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data
{
    public static  class StoreDataInDataBase
    {
        public  async static Task SeedingDataAsync(AppDbContext appdb)
        {
            if(!appdb.ProductBrands.Any())
            {
                var BrandData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/brands.json");
                var brand = JsonSerializer.Deserialize<List<ProductBrand>>(BrandData);
                if (brand is not null && brand.Count() > 0)
                {
                    foreach (var item in brand)
                    {
                        appdb.Set<ProductBrand>().Add(item);
                    }
                    await appdb.SaveChangesAsync();
                }
            }
        
            if(!appdb.Categories.Any())
            {
                var CategoryData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/categories.json");
                var categories = JsonSerializer.Deserialize<List<Category>>(CategoryData);
                if (categories is not null && categories.Count() > 0)
                {
                    foreach (var item in categories)
                    {
                        appdb.Set<Category>().Add(item);
                    }
                    await appdb.SaveChangesAsync();
                }
            }
            if (!appdb.Products.Any())
            {
                var productData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productData);
                if (products is not null && products.Count() > 0)
                {
                    foreach (var item in products)
                    {
                        appdb.Set<Product>().Add(item);
                    }
                    await appdb.SaveChangesAsync();
                }
            }
            if (!appdb.DeliveryMethods.Any())
            {
                var DeliveryMethodData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/delivery.json");
                var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodData);
                if (DeliveryMethods is not null && DeliveryMethods.Count() > 0)
                {
                    foreach (var DeliveryMethod in DeliveryMethods)
                    {
                        appdb.Set<DeliveryMethod>().Add(DeliveryMethod);
                    }
                    await appdb.SaveChangesAsync();
                }
            }
        }

    }
}
