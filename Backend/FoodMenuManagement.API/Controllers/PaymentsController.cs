using System.Security.Claims;
using FoodMenuManagement.API.DTOs.Payment;
using FoodMenuManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodMenuManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _service;

        public PaymentsController(IPaymentService service)
        {
            _service = service;
        }

        private string GetCustomerId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub")
                ?? string.Empty;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreatePaymentDto dto)
        {
            try
            {
                var result = await _service.CreateAsync(
                    dto, GetCustomerId());

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpGet("my-payments")]
        public async Task<IActionResult> GetMyPayments()
        {
            var result = await _service.GetMyPaymentsAsync(
                GetCustomerId());

            return Ok(result);
        }

        [HttpGet("order/{orderId:int}")]
        public async Task<IActionResult> GetByOrderId(int orderId)
        {
            var result = await _service.GetByOrderIdAsync(
                orderId, GetCustomerId());

            if (result == null)
                return NotFound(new { message = "Payment not found." });

            return Ok(result);
        }
    }
}