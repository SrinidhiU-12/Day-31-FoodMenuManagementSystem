using FoodMenuManagement.API.Data;
using FoodMenuManagement.API.Models;
using FoodMenuManagement.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodMenuManagement.API.Repositories.Implementations
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> ExistsByNameAsync(
            string name, int? excludeId = null)
        {
            return await _context.Categories.AnyAsync(c =>
                c.Name.ToLower() == name.ToLower() &&
                (!excludeId.HasValue || c.Id != excludeId.Value));
        }

        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeactivateAsync(Category category)
        {
            category.IsActive = false;
            await _context.SaveChangesAsync();
        }
    }
}