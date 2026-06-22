using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Talabat.Api.Dtos.Pagination;
using Talabat.Api.Dtos.Product;
using Talabat.Api.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository.Repository.ProductRepositry;

namespace Talabat.Api.Controllers
{
  
    public class ProductsController : BaseController
    {
        private readonly IProductRepository _productRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductsController> _logger;
        public ProductsController(IProductRepository productRepo,IMapper mapper, ILogger<ProductsController> logger)
        {
            _productRepo = productRepo;
            _mapper = mapper;
            _logger = logger;
        }
        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseToNotFound_Badrequest_Unauthorized), StatusCodes.Status404NotFound)]           //improve response at swagger in sucess 
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts([FromQuery] string? search,
         [FromQuery] string? sort,
          [FromQuery] string? order,
         [FromQuery] int ? ProductBrandId,
         [FromQuery] int? CategoryId,
         [FromQuery] int pageIndex=1,
         [FromQuery] int pageSize = 10)
        {
            if (pageIndex <= 0 || pageSize <= 0)
            {
                return BadRequest(new ApiResponseToNotFound_Badrequest_Unauthorized(StatusCodes.Status400BadRequest, "pageIndex and pageSize must be greater than zero."));
            }
            var products = await _productRepo.GetProducts(search,sort
               , order, ProductBrandId, CategoryId, pageIndex, pageSize);
            var data = _mapper.Map<IEnumerable<ProductToReturnDto>>(products);
             var response = new PagedResposeDto<ProductToReturnDto>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount= await _productRepo.GetProductsCountAsync(search),
                 Data = data,
              
            };
            return Ok(response);
        }
        //this endpoint to get product by id 
        [ProducesResponseType(typeof(ProductToReturnDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseToNotFound_Badrequest_Unauthorized),StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>>GetProductById(int id)
        {
            if(id<=0)
                return NotFound( new ApiResponseToNotFound_Badrequest_Unauthorized(StatusCodes.Status400BadRequest, "Invalid product id."));
            var product = await _productRepo.GetAsync(id);
            if (product == null)
            {
                _logger.LogInformation("Product with id {ProductId} not found.", id);
                return NotFound(new ApiResponseToNotFound_Badrequest_Unauthorized(400));
            }
               
            return Ok(_mapper.Map<ProductToReturnDto>(product));
           

        }
    }
}
