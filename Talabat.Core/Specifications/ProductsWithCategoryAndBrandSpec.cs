using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductsWithCategoryAndBrandSpec : BaseSpecification<Product>
    {
        public ProductsWithCategoryAndBrandSpec(ProductSpecParam specParams)
            : base(P => 
            ((string.IsNullOrEmpty(specParams.SearchTerm)) || (P.Name.ToLower().Contains(specParams.SearchTerm))) && 
            ((!specParams.BrandId.HasValue) || (P.BrandId == specParams.BrandId.Value)) &&
            ((!specParams.CategoryId.HasValue) || (P.CategoryId == specParams.CategoryId)))
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);
            if (specParams.Sort is not null)
            {
                switch (specParams.Sort)
                {
                    case "PriceAsc":
                        OrderBy = P => P.Price;
                        break;
                    case "PriceDesc":
                        OrderByDesc = P => P.Price;
                        break;
                    default:
                        OrderBy = P => P.Name;
                        break;
                }
            }
            else
                OrderBy = P => P.Name;
            ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
        }
        public ProductsWithCategoryAndBrandSpec(int id)
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);
            Criteria = (P => P.Id == id);
        }
    }
}
