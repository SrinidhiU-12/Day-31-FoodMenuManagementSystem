namespace FoodMenuManagement.API.DTOs.Payment
{
    public class UpdatePaymentStatusDto
    {
        public string PaymentStatus { get; set; } = string.Empty;
        public string? TransactionId { get; set; }
    }
}