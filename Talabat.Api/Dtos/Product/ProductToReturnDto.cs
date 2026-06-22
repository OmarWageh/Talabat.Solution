using Talabat.Core.Entities;

namespace Talabat.Api.Dtos.Product
{
    public class ProductToReturnDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int ProductBrandId { get; set; }
        public string? productBrand { get; set; }
        public int CategoryId { get; set; }
        public string? category { get; set; }
    }
}
