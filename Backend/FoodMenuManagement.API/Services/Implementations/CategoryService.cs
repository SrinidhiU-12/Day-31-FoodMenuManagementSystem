using FoodMenuManagement.API.DTOs.Category;
using FoodMenuManagement.API.Models;
using FoodMenuManagement.API.Repositories.Interfaces;
using FoodMenuManagement.API.Services.Interfaces;

namespace FoodMenuManagement.API.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CategoryResponseDto>> GetAllAsync()
        {
            var categories = await _repository.GetAllAsync();

            return categories.Select(MapToResponse).ToList();
        }

        public async Task<CategoryResponseDto?> GetByIdAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);

            return category == null ? null : MapToResponse(category);
        }

        public async Task<CategoryResponseDto> CreateAsync(
            CreateCategoryDto request)
        {
            string name = request.Name.Trim();

            if (await _repository.ExistsByNameAsync(name))
                throw new InvalidOperationException(
                    "Category already exists.");

            var category = new Category
            {
                Name = name,
                Description = request.Description.Trim()
            };

            await _repository.AddAsync(category);

            return MapToResponse(category);
        }

        public async Task<CategoryResponseDto?> UpdateAsync(
            int id, UpdateCategoryDto request)
        {
            var category = await _repository.GetByIdAsync(id);

            if (category == null)
                return null;

            string name = request.Name.Trim();

            if (await _repository.ExistsByNameAsync(name, id))
                throw new InvalidOperationException(
                    "Another category already uses this name.");

            category.Name = name;
            category.Description = request.Description.Trim();
            category.IsActive = request.IsActive;

            await _repository.UpdateAsync(category);

            return MapToResponse(category);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);

            if (category == null)
                return false;

            await _repository.DeactivateAsync(category);

            return true;
        }

        private static CategoryResponseDto MapToResponse(Category category)
        {
            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive,
                CreatedAt = category.CreatedAt
            };
        }
    }
}