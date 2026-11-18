
using FoodMenuManagement.API.Models;

namespace FoodMenuManagement.API.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllByCustomerAsync(string customerId);

        Task<Order?> GetByIdAsync(int id, string customerId);

        Task AddAsync(Order order);

        Task UpdateAsync(Order order);
    }
}
