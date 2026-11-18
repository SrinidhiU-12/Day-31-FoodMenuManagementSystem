using System.ComponentModel.DataAnnotations;

namespace FoodMenuManagement.API.DTOs.FoodItem
{
    public class UpdateFoodItemDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Range(typeof(decimal), "0.01", "999999")]
        public decimal Price { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public bool IsAvailable { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }
    }
}