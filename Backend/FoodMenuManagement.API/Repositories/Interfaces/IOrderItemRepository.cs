
using FoodMenuManagement.API.Models;

namespace FoodMenuManagement.API.Repositories.Interfaces
{
    public interface IOrderItemRepository
    {
        Task<Order?> GetOrderByIdForCustomerAsync(
            int orderId, string customerId);

        Task<FoodItem?> GetFoodItemByIdAsync(int foodItemId);

        Task<OrderItem?> GetByIdAsync(int id);

        Task AddAsync(OrderItem orderItem);

        Task UpdateAsync(OrderItem orderItem);

        Task DeleteAsync(OrderItem orderItem);
    }
}
