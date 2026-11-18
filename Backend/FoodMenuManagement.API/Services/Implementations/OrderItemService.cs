
using FoodMenuManagement.API.DTOs.OrderItem;
using FoodMenuManagement.API.Models;
using FoodMenuManagement.API.Repositories.Interfaces;
using FoodMenuManagement.API.Services.Interfaces;

namespace FoodMenuManagement.API.Services.Implementations
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _repository;

        public OrderItemService(IOrderItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<OrderItemResponseDto>?> GetByOrderIdAsync(
            int orderId, string customerId)
        {
            var order = await _repository.GetOrderByIdForCustomerAsync(
                orderId, customerId);

            if (order == null)
                return null;

            return order.OrderItems
                .Select(MapToResponse)
                .ToList();
        }

        public async Task<OrderItemResponseDto> CreateAsync(
            CreateOrderItemDto dto, string customerId)
        {
            if (dto.Quantity <= 0)
                throw new ArgumentException(
                    "Quantity must be greater than zero.");

            var order = await _repository.GetOrderByIdForCustomerAsync(
                dto.OrderId, customerId);

            if (order == null)
                throw new KeyNotFoundException("Order not found.");

            if (order.Status != "Pending")
                throw new InvalidOperationException(
                    "Items can only be added to pending orders.");

            var foodItem = await _repository.GetFoodItemByIdAsync(
                dto.FoodItemId);

            if (foodItem == null || !foodItem.IsAvailable)
                throw new InvalidOperationException(
                    "Food item is unavailable.");

            var orderItem = new OrderItem
            {
                OrderId = order.Id,
                FoodItemId = foodItem.Id,
                Quantity = dto.Quantity,
                UnitPrice = foodItem.Price,
                SubTotal = foodItem.Price * dto.Quantity
            };

            order.OrderItems.Add(orderItem);

            order.TotalAmount = order.OrderItems.Sum(
                item => item.SubTotal);

            await _repository.AddAsync(orderItem);

            orderItem.FoodItem = foodItem;

            return MapToResponse(orderItem);
        }

        public async Task<OrderItemResponseDto?> UpdateAsync(
            int id, UpdateOrderItemDto dto, string customerId)
        {
            if (dto.Quantity <= 0)
                throw new ArgumentException(
                    "Quantity must be greater than zero.");

            var orderItem = await _repository.GetByIdAsync(id);

            if (orderItem == null || orderItem.Order == null)
                return null;

            if (orderItem.Order.CustomerId != customerId)
                return null;

            var order = await _repository.GetOrderByIdForCustomerAsync(
                orderItem.OrderId, customerId);

            if (order == null)
                return null;

            if (order.Status != "Pending")
                throw new InvalidOperationException(
                    "Items can only be updated in pending orders.");

            orderItem.Quantity = dto.Quantity;

            orderItem.SubTotal =
                orderItem.UnitPrice * dto.Quantity;

            order.TotalAmount = order.OrderItems.Sum(
                item => item.SubTotal);

            await _repository.UpdateAsync(orderItem);

            return MapToResponse(orderItem);
        }

        public async Task<bool> DeleteAsync(
            int id, string customerId)
        {
            var orderItem = await _repository.GetByIdAsync(id);

            if (orderItem == null || orderItem.Order == null)
                return false;

            if (orderItem.Order.CustomerId != customerId)
                return false;

            var order = await _repository.GetOrderByIdForCustomerAsync(
                orderItem.OrderId, customerId);

            if (order == null)
                return false;

            if (order.Status != "Pending")
                throw new InvalidOperationException(
                    "Items can only be removed from pending orders.");

            order.TotalAmount = order.OrderItems
                .Where(item => item.Id != id)
                .Sum(item => item.SubTotal);

            await _repository.DeleteAsync(orderItem);

            return true;
        }

        private static OrderItemResponseDto MapToResponse(
            OrderItem orderItem)
        {
            return new OrderItemResponseDto
            {
                Id = orderItem.Id,
                OrderId = orderItem.OrderId,
                FoodItemId = orderItem.FoodItemId,
                FoodItemName = orderItem.FoodItem?.Name ?? string.Empty,
                Quantity = orderItem.Quantity,
                UnitPrice = orderItem.UnitPrice,
                SubTotal = orderItem.SubTotal
            };
        }
    }
}
