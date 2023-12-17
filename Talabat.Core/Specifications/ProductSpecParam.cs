using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications
{
    public class ProductSpecParam
    {
        private const int MaxPageSize = 10;
        public string? Sort { get; set; }
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
        private string? searchTerm;
        public string? SearchTerm
        {
            get { return searchTerm; }
            set { searchTerm = value?.ToLower(); }
        }
        public int PageIndex { get; set; } = 1;
        private int pageSize;
        public int PageSize { get { return pageSize; } set { pageSize = value > MaxPageSize ? MaxPageSize : value; } }
    }
}
