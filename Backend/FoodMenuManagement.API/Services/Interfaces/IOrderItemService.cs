
using FoodMenuManagement.API.DTOs.OrderItem;

namespace FoodMenuManagement.API.Services.Interfaces
{
    public interface IOrderItemService
    {
        Task<List<OrderItemResponseDto>?> GetByOrderIdAsync(
            int orderId, string customerId);

        Task<OrderItemResponseDto> CreateAsync(
            CreateOrderItemDto dto, string customerId);

        Task<OrderItemResponseDto?> UpdateAsync(
            int id, UpdateOrderItemDto dto, string customerId);

        Task<bool> DeleteAsync(int id, string customerId);
    }
}
