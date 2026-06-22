using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Api.Dtos;
using Talabat.Api.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;

namespace Talabat.Api.Controllers
{
  
    public class BrandsController : BaseController
    {
        private readonly IGenericRepository<ProductBrand> _BrandRepo;

        public BrandsController(IGenericRepository<ProductBrand> BrandRepo)
        {
            _BrandRepo = BrandRepo;
        }
        [ProducesResponseType(typeof(ProductBrand), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseToNotFound_Badrequest_Unauthorized), StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var Brands = await _BrandRepo.GetAllAsync();
            return Ok(Brands);
        }
    }
}
