
namespace FoodMenuManagement.API.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string CustomerId { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public string Status { get; set; } = "Pending";

        public decimal TotalAmount { get; set; } = 0;

        // Navigation property
        public ApplicationUser? Customer { get; set; }
    }
}
