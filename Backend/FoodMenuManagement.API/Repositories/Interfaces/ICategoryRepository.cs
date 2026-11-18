using FoodMenuManagement.API.Models;

namespace FoodMenuManagement.API.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();

        Task<Category?> GetByIdAsync(int id);

        Task<bool> ExistsByNameAsync(string name, int? excludeId = null);

        Task AddAsync(Category category);

        Task UpdateAsync(Category category);

        Task DeactivateAsync(Category category);
    }
}