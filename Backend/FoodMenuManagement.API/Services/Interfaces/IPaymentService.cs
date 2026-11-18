using FoodMenuManagement.API.DTOs.Payment;

namespace FoodMenuManagement.API.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResponseDto> CreateAsync(
            CreatePaymentDto dto, string customerId);

        Task<List<PaymentResponseDto>> GetMyPaymentsAsync(
            string customerId);

        Task<PaymentResponseDto?> GetByOrderIdAsync(
            int orderId, string customerId);

        Task<bool> UpdateStatusAsync(
            int paymentId, UpdatePaymentStatusDto dto);
    }
}