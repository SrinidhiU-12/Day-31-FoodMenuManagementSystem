
namespace FoodMenuManagement.API.DTOs.Order
{
    public class OrderResponseDto
    {
        public int Id { get; set; }

        public string CustomerId { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; }

        public string Status { get; set; } = string.Empty;

        public decimal TotalAmount { get; set; }
    }
}
