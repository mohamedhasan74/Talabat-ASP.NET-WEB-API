using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Api.Errors;
using Talabat.Core.Entities;
using Talabat.Core.IRepository;

namespace Talabat.Api.Controllers
{
    public class BasketController : ApiBaseController
    {
        private readonly IBasketRepository _basketRepository;

        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            return Ok(basket is null ? new CustomerBasket(id)  : basket);
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteBasket(string id)
        {
            var delBasket = await _basketRepository.DeleteBasketAsync(id);
            if (delBasket is false) return BadRequest(new ApiErrorResponse(400));
            return Ok(new { message = "Basket Is Deleted"});
        }
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateOrCreatedBasket(CustomerBasket basket)
        {
            var updatedOrCreatedBasket = await _basketRepository.UpdateOrCreateBasketAsync(basket);
            if (updatedOrCreatedBasket is null) return BadRequest(new ApiErrorResponse(400));
            return Ok(updatedOrCreatedBasket);
        }
    }
}
