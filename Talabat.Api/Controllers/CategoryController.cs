using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Api.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;

namespace Talabat.Api.Controllers
{

    public class CategoriesController : BaseController
    {
        private readonly IGenericRepository<Category> _CategoryRepo;

        public CategoriesController(IGenericRepository<Category> CategoryRepo) {
            _CategoryRepo = CategoryRepo;
        }
        [ProducesResponseType(typeof(Category), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseToNotFound_Badrequest_Unauthorized), StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Category>>> GetCategories()
        {
            var Categories = await _CategoryRepo.GetAllAsync();
            return Ok(Categories);
        }
    }
}
