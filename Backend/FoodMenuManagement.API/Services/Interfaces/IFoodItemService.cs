using FoodMenuManagement.API.DTOs.FoodItem;

namespace FoodMenuManagement.API.Services.Interfaces
{
    public interface IFoodItemService
    {
        Task<List<FoodItemResponseDto>> GetAllAsync();

        Task<FoodItemResponseDto?> GetByIdAsync(int id);

        Task<FoodItemResponseDto> CreateAsync(CreateFoodItemDto request);

        Task<FoodItemResponseDto?> UpdateAsync(
            int id, UpdateFoodItemDto request);

        Task<bool> DeleteAsync(int id);
    }
}