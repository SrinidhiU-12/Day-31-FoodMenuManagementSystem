using FoodMenuManagement.API.DTOs.Payment;
using FoodMenuManagement.API.Models;
using FoodMenuManagement.API.Repositories.Interfaces;
using FoodMenuManagement.API.Services.Interfaces;

namespace FoodMenuManagement.API.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _repository;

        public PaymentService(IPaymentRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaymentResponseDto> CreateAsync(
            CreatePaymentDto dto, string customerId)
        {
            var order = await _repository.GetOrderByIdForCustomerAsync(
                dto.OrderId, customerId);

            if (order == null)
                throw new KeyNotFoundException("Order not found.");

            if (order.Status != "Pending")
                throw new InvalidOperationException(
                    "Payment can only be created for pending orders.");

            if (order.TotalAmount <= 0)
                throw new InvalidOperationException(
                    "Cannot create payment for an empty order.");

            var existingPayment =
                await _repository.GetByOrderIdAsync(dto.OrderId);

            if (existingPayment != null)
                throw new InvalidOperationException(
                    "Payment already exists for this order.");

            var method = dto.PaymentMethod.Trim();

            if (method != "CashOnDelivery" && method != "Online")
                throw new ArgumentException(
                    "Payment method must be CashOnDelivery or Online.");

            var payment = new Payment
            {
                OrderId = order.Id,
                Amount = order.TotalAmount,
                PaymentMethod = method,
                PaymentStatus = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(payment);

            return MapToResponse(payment);
        }

        public async Task<List<PaymentResponseDto>> GetMyPaymentsAsync(
            string customerId)
        {
            var payments =
                await _repository.GetAllByCustomerAsync(customerId);

            return payments.Select(MapToResponse).ToList();
        }

        public async Task<PaymentResponseDto?> GetByOrderIdAsync(
            int orderId, string customerId)
        {
            var order = await _repository.GetOrderByIdForCustomerAsync(
                orderId, customerId);

            if (order == null)
                return null;

            var payment = await _repository.GetByOrderIdAsync(orderId);

            return payment == null ? null : MapToResponse(payment);
        }

        public async Task<bool> UpdateStatusAsync(
            int paymentId, UpdatePaymentStatusDto dto)
        {
            var payment = await _repository.GetByIdForCustomerAsync(
                paymentId, string.Empty);

            // Status changes are performed through the Admin endpoint.
            // Fetch the payment directly through the customer query is
            // not suitable for an admin, so this method is updated below
            // if an admin repository lookup is added.
            return false;
        }

        private static PaymentResponseDto MapToResponse(Payment payment)
        {
            return new PaymentResponseDto
            {
                Id = payment.Id,
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod,
                PaymentStatus = payment.PaymentStatus,
                TransactionId = payment.TransactionId,
                PaymentDate = payment.PaymentDate,
                CreatedAt = payment.CreatedAt
            };
        }
    }
}