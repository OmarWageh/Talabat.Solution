using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Api.Dtos.Basket;
using Talabat.Api.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;

namespace Talabat.Api.Controllers
{
  
    public class BasketController : BaseController
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IMapper _mapper;
       
        public BasketController(IBasketRepository basketRepo, IMapper mapper)
        {
            _basketRepo = basketRepo;
            _mapper= mapper;

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerBasket>> GetBasket(string id )
        {
            var basket=await _basketRepo.GetBasketAsync(id);
            return Ok(basket ?? new CustomerBasket(id));
        }
       
        [HttpPost] 
        public async Task<ActionResult<CustomerBasket>> CreateBasket([FromBody] CustomerBasketDto basket)
        {
            var mappedBasket=_mapper.Map<CustomerBasketDto, CustomerBasket>(basket);
            var CreatedOrUpdatedBasket = await _basketRepo.UpdateBasketAsync(mappedBasket);
            if (CreatedOrUpdatedBasket is null)
                return BadRequest(new ApiResponseToNotFound_Badrequest_Unauthorized(400));
            return Ok(CreatedOrUpdatedBasket);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBasket(string id)
        {
            var deleted=await _basketRepo.DeleteBasketAsync(id);
            if (deleted)
                return NoContent();
            var response = new ApiResponseToNotFound_Badrequest_Unauthorized(404, "Basket Not Found");
            return NotFound(response);
  

        }
    }
}
