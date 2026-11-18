namespace FoodMenuManagement.API.DTOs.OrderItem
{
    public class CreateOrderItemDto
    {
        public int OrderId { get; set; }

        public int FoodItemId { get; set; }

        public int Quantity { get; set; }
    }
}