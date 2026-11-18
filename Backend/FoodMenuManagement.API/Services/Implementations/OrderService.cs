
using FoodMenuManagement.API.DTOs.Order;
using FoodMenuManagement.API.Models;
using FoodMenuManagement.API.Repositories.Interfaces;
using FoodMenuManagement.API.Services.Interfaces;

namespace FoodMenuManagement.API.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;

        public OrderService(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<OrderResponseDto>> GetAllByCustomerAsync(
            string customerId)
        {
            var orders = await _repository.GetAllByCustomerAsync(customerId);

            return orders.Select(MapToResponse).ToList();
        }

        public async Task<OrderResponseDto?> GetByIdAsync(
            int id, string customerId)
        {
            var order = await _repository.GetByIdAsync(id, customerId);

            return order == null ? null : MapToResponse(order);
        }

        public async Task<OrderResponseDto> CreateAsync(string customerId)
        {
            var order = new Order
            {
                CustomerId = customerId,
                OrderDate = DateTime.UtcNow,
                Status = "Pending",
                TotalAmount = 0
            };

            await _repository.AddAsync(order);

            return MapToResponse(order);
        }

        public async Task<bool> CancelAsync(int id, string customerId)
        {
            var order = await _repository.GetByIdAsync(id, customerId);

            if (order == null)
                return false;

            if (order.Status == "Delivered" ||
                order.Status == "Cancelled")
            {
                throw new InvalidOperationException(
                    "This order cannot be cancelled.");
            }

            order.Status = "Cancelled";

            await _repository.UpdateAsync(order);

            return true;
        }

        private static OrderResponseDto MapToResponse(Order order)
        {
            return new OrderResponseDto
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount
            };
        }
    }
}
