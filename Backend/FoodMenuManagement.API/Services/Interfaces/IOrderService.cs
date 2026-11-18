
using FoodMenuManagement.API.DTOs.Order;

namespace FoodMenuManagement.API.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderResponseDto>> GetAllByCustomerAsync(
            string customerId);

        Task<OrderResponseDto?> GetByIdAsync(
            int id, string customerId);

        Task<OrderResponseDto> CreateAsync(string customerId);

        Task<bool> CancelAsync(int id, string customerId);
    }
}
