
using System.Security.Claims;
using FoodMenuManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodMenuManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrdersController(IOrderService service)
        {
            _service = service;
        }

        private string? GetCustomerId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var customerId = GetCustomerId();

            if (string.IsNullOrEmpty(customerId))
                return Unauthorized();

            var orders = await _service.GetAllByCustomerAsync(customerId);

            return Ok(orders);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var customerId = GetCustomerId();

            if (string.IsNullOrEmpty(customerId))
                return Unauthorized();

            var order = await _service.GetByIdAsync(id, customerId);

            if (order == null)
                return NotFound(new { message = "Order not found." });

            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            var customerId = GetCustomerId();

            if (string.IsNullOrEmpty(customerId))
                return Unauthorized();

            var order = await _service.CreateAsync(customerId);

            return CreatedAtAction(
                nameof(GetById),
                new { id = order.Id },
                order);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Cancel(int id)
        {
            var customerId = GetCustomerId();

            if (string.IsNullOrEmpty(customerId))
                return Unauthorized();

            try
            {
                var cancelled = await _service.CancelAsync(id, customerId);

                if (!cancelled)
                    return NotFound(new { message = "Order not found." });

                return Ok(new
                {
                    message = "Order cancelled successfully."
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}
