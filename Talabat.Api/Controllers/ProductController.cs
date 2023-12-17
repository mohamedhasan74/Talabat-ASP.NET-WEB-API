using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Api.Dtos;
using Talabat.Api.Errors;
using Talabat.Core.Entities;
using Talabat.Core.IRepository;
using Talabat.Core.Specifications;

namespace Talabat.Api.Controllers
{
    public class ProductController : ApiBaseController
    {
        private readonly IGenericRepository<Product> _genericRepository;
        private readonly IMapper _mapper;

        public ProductController(IGenericRepository<Product> genericRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductDto>>> GetAll([FromQuery] ProductSpecParam specParams)
        {
            var spec = new ProductsWithCategoryAndBrandSpec(specParams);
            var products = await _genericRepository.GetAllWithSpecAsync(spec);
            var productsMapped = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);
            var specCount = new ProductWithFilterationCountSpec(specParams);
            var count = await _genericRepository.GetCountAsync(specCount);
            var productsPagination = new Pagination<ProductDto>(specParams.PageIndex, specParams.PageSize, count, productsMapped);
            return Ok(productsPagination);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDto), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 404)]
        public async Task<ActionResult<ProductDto>> Get(int id)
        {
            var spec = new ProductsWithCategoryAndBrandSpec(id);
            var product = await _genericRepository.GetByIdWithSpecAsync(spec);
            if (product is null) return NotFound(new ApiErrorResponse(404));
            var productMapped = _mapper.Map<Product, ProductDto>(product);
            return Ok(productMapped);
        }
    }
}
