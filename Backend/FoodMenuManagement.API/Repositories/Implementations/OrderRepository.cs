
using FoodMenuManagement.API.Data;
using FoodMenuManagement.API.Models;
using FoodMenuManagement.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodMenuManagement.API.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetAllByCustomerAsync(
            string customerId)
        {
            return await _context.Orders
                .AsNoTracking()
                .Where(o => o.CustomerId == customerId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(
            int id, string customerId)
        {
            return await _context.Orders
                .FirstOrDefaultAsync(o =>
                    o.Id == id && o.CustomerId == customerId);
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            await _context.SaveChangesAsync();
        }
    }
}
