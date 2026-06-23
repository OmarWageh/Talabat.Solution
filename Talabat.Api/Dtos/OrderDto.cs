using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Api.Dtos
{
    public class OrderDto
    {
        [Required]
        public string BuyerEmail { set; get; }
        [Required]
        public string BasketId { set; get; }
        [Required]
        public int DeliveryMethodId { get; set; }
        [Required]
        public AddressDto ShippingAddress { set; get; }


    }

    
}
