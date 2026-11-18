namespace FoodMenuManagement.API.DTOs.OrderItem
{
    public class OrderItemResponseDto
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int FoodItemId { get; set; }

        public string FoodItemName { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal SubTotal { get; set; }
    }
}