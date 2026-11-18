using FoodMenuManagement.API.DTOs.Category;

namespace FoodMenuManagement.API.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryResponseDto>> GetAllAsync();

        Task<CategoryResponseDto?> GetByIdAsync(int id);

        Task<CategoryResponseDto> CreateAsync(CreateCategoryDto request);

        Task<CategoryResponseDto?> UpdateAsync(
            int id, UpdateCategoryDto request);

        Task<bool> DeleteAsync(int id);
    }
}