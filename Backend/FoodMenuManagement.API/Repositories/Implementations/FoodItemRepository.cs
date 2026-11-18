using FoodMenuManagement.API.Data;
using FoodMenuManagement.API.Models;
using FoodMenuManagement.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodMenuManagement.API.Repositories.Implementations
{
    public class FoodItemRepository : IFoodItemRepository
    {
        private readonly ApplicationDbContext _context;

        public FoodItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<FoodItem>> GetAllAsync()
        {
            return await _context.FoodItems
                .Include(f => f.Category)
                .AsNoTracking()
                .OrderBy(f => f.Name)
                .ToListAsync();
        }

        public async Task<FoodItem?> GetByIdAsync(int id)
        {
            return await _context.FoodItems
                .Include(f => f.Category)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<bool> IsActiveCategoryAsync(int categoryId)
        {
            return await _context.Categories.AnyAsync(c =>
                c.Id == categoryId && c.IsActive);
        }

        public async Task AddAsync(FoodItem foodItem)
        {
            await _context.FoodItems.AddAsync(foodItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(FoodItem foodItem)
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeactivateAsync(FoodItem foodItem)
        {
            foodItem.IsAvailable = false;
            await _context.SaveChangesAsync();
        }
    }
}