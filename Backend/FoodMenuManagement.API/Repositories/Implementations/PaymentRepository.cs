using FoodMenuManagement.API.Data;
using FoodMenuManagement.API.Models;
using FoodMenuManagement.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodMenuManagement.API.Repositories.Implementations
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order?> GetOrderByIdForCustomerAsync(
            int orderId, string customerId)
        {
            return await _context.Orders
                .FirstOrDefaultAsync(o =>
                    o.Id == orderId &&
                    o.CustomerId == customerId);
        }

        public async Task<Payment?> GetByOrderIdAsync(int orderId)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.OrderId == orderId);
        }

        public async Task<List<Payment>> GetAllByCustomerAsync(
            string customerId)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .Where(p => p.Order != null &&
                            p.Order.CustomerId == customerId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<Payment?> GetByIdForCustomerAsync(
            int paymentId, string customerId)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p =>
                    p.Id == paymentId &&
                    p.Order != null &&
                    p.Order.CustomerId == customerId);
        }

        public async Task AddAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Payment payment)
        {
            await _context.SaveChangesAsync();
        }
    }
}