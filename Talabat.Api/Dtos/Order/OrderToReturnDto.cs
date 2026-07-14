using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Api.Dtos.Order
{
    public class OrderToReturnDto
    {
        public int Id { set; get; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } 
        public string Status { get; set; }
        public Address ShippingAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal DeliveryMethodCost { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
        public decimal Subtotal { get; set; }
       public decimal Total { get; set; }
        public string PaymentIntentId { get; set; } = string.Empty;

    }
}
