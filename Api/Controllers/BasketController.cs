using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
   
    public class BasketController : BaseApiController
    {
        //Correr Redis
        //redis-server --port 6380 --slaveof 127.0.0.1 6379
        private readonly IBasketRepository basketRepository;

        public BasketController(IBasketRepository basketRepository)
        {
            this.basketRepository = basketRepository;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string Id)
        {
            var basket = await basketRepository.GetBasketAsync(Id);
            return Ok(basket ?? new CustomerBasket(Id));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasket basketC)
        {
            var updatedBasket = await basketRepository.UpdateBasketAsync(basketC);
            return Ok(updatedBasket);
        }

        [HttpDelete]
        public async Task DeleteBasketAsync(string id)
        {
            await basketRepository.DeleteBasketAsync(id);
        }
    }
}
