using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductWithFilterationCountSpec : BaseSpecification<Product>
    {
        public ProductWithFilterationCountSpec(ProductSpecParam specParams)
            : base(P =>
            ((string.IsNullOrEmpty(specParams.SearchTerm)) || (P.Name.ToLower().Contains(specParams.SearchTerm))) &&
            ((!specParams.BrandId.HasValue) || (P.BrandId == specParams.BrandId.Value)) &&
            ((!specParams.CategoryId.HasValue) || (P.CategoryId == specParams.CategoryId)))
        {
            
        }
    }
}
