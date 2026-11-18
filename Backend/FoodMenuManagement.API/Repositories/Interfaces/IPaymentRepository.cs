using FoodMenuManagement.API.Models;

namespace FoodMenuManagement.API.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Order?> GetOrderByIdForCustomerAsync(
            int orderId, string customerId);

        Task<Payment?> GetByOrderIdAsync(int orderId);

        Task<List<Payment>> GetAllByCustomerAsync(string customerId);

        Task<Payment?> GetByIdForCustomerAsync(
            int paymentId, string customerId);

        Task AddAsync(Payment payment);

        Task UpdateAsync(Payment payment);
    }
}