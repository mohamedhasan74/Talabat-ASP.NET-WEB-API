using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext storeContext)
        {
            if(storeContext.Brands.Count() == 0)
            {
                var dataSeed = File.ReadAllText("../Talabat.Repository/Data/Data Seed/brands.json");
                var brands = JsonSerializer.Deserialize<IEnumerable<ProductBrand>>(dataSeed);
                if(brands is not null)
                {
                    foreach (var brand in brands)
                        await storeContext.Brands.AddAsync(brand);
                    await storeContext.SaveChangesAsync();

                }
            }
            if (storeContext.Categories.Count() == 0)
            {
                var dataSeed = File.ReadAllText("../Talabat.Repository/Data/Data Seed/categories.json");
                var categories = JsonSerializer.Deserialize<IEnumerable<ProductCategory>>(dataSeed);
                if (categories is not null)
                {
                    foreach (var category in categories)
                        await storeContext.Categories.AddAsync(category);
                    await storeContext.SaveChangesAsync();

                }
            }
            if (storeContext.Products.Count() == 0)
            {
                var dataSeed = File.ReadAllText("../Talabat.Repository/Data/Data Seed/products.json");
                var products = JsonSerializer.Deserialize<IEnumerable<Product>>(dataSeed);
                if (products is not null)
                {
                    foreach (var product in products)
                        await storeContext.Products.AddAsync(product);
                    await storeContext.SaveChangesAsync();

                }
            }
        }
    }
}
