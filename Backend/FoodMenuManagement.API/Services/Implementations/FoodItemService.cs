using FoodMenuManagement.API.DTOs.FoodItem;
using FoodMenuManagement.API.Models;
using FoodMenuManagement.API.Repositories.Interfaces;
using FoodMenuManagement.API.Services.Interfaces;

namespace FoodMenuManagement.API.Services.Implementations
{
    public class FoodItemService : IFoodItemService
    {
        private readonly IFoodItemRepository _repository;

        public FoodItemService(IFoodItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<FoodItemResponseDto>> GetAllAsync()
        {
            var foodItems = await _repository.GetAllAsync();

            return foodItems.Select(MapToResponse).ToList();
        }

        public async Task<FoodItemResponseDto?> GetByIdAsync(int id)
        {
            var foodItem = await _repository.GetByIdAsync(id);

            return foodItem == null ? null : MapToResponse(foodItem);
        }

        public async Task<FoodItemResponseDto> CreateAsync(
            CreateFoodItemDto request)
        {
            if (!await _repository.IsActiveCategoryAsync(request.CategoryId))
                throw new InvalidOperationException(
                    "Selected category does not exist or is inactive.");

            var foodItem = new FoodItem
            {
                Name = request.Name.Trim(),
                Description = request.Description.Trim(),
                Price = request.Price,
                ImageUrl = request.ImageUrl.Trim(),
                CategoryId = request.CategoryId
            };

            await _repository.AddAsync(foodItem);

            var savedItem = await _repository.GetByIdAsync(foodItem.Id);

            return MapToResponse(savedItem!);
        }

        public async Task<FoodItemResponseDto?> UpdateAsync(
            int id, UpdateFoodItemDto request)
        {
            var foodItem = await _repository.GetByIdAsync(id);

            if (foodItem == null)
                return null;

            if (!await _repository.IsActiveCategoryAsync(request.CategoryId))
                throw new InvalidOperationException(
                    "Selected category does not exist or is inactive.");

            foodItem.Name = request.Name.Trim();
            foodItem.Description = request.Description.Trim();
            foodItem.Price = request.Price;
            foodItem.ImageUrl = request.ImageUrl.Trim();
            foodItem.IsAvailable = request.IsAvailable;
            foodItem.CategoryId = request.CategoryId;

            await _repository.UpdateAsync(foodItem);

            var updatedItem = await _repository.GetByIdAsync(id);

            return MapToResponse(updatedItem!);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var foodItem = await _repository.GetByIdAsync(id);

            if (foodItem == null)
                return false;

            await _repository.DeactivateAsync(foodItem);

            return true;
        }

        private static FoodItemResponseDto MapToResponse(FoodItem foodItem)
        {
            return new FoodItemResponseDto
            {
                Id = foodItem.Id,
                Name = foodItem.Name,
                Description = foodItem.Description,
                Price = foodItem.Price,
                ImageUrl = foodItem.ImageUrl,
                IsAvailable = foodItem.IsAvailable,
                CreatedAt = foodItem.CreatedAt,
                CategoryId = foodItem.CategoryId,
                CategoryName = foodItem.Category?.Name ?? string.Empty
            };
        }
    }
}