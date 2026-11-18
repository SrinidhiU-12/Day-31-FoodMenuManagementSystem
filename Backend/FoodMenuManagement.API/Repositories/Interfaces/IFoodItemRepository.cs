using FoodMenuManagement.API.Models;

namespace FoodMenuManagement.API.Repositories.Interfaces
{
    public interface IFoodItemRepository
    {
        Task<List<FoodItem>> GetAllAsync();

        Task<FoodItem?> GetByIdAsync(int id);

        Task<bool> IsActiveCategoryAsync(int categoryId);

        Task AddAsync(FoodItem foodItem);

        Task UpdateAsync(FoodItem foodItem);

        Task DeactivateAsync(FoodItem foodItem);
    }
}