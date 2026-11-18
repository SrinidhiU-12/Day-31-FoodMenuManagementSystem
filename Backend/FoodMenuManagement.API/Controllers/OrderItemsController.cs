
using System.Security.Claims;
using FoodMenuManagement.API.DTOs.OrderItem;
using FoodMenuManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodMenuManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemService _service;

        public OrderItemsController(IOrderItemService service)
        {
            _service = service;
        }

        private string? GetCustomerId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub");
        }

        [HttpGet("order/{orderId:int}")]
        public async Task<IActionResult> GetByOrderId(int orderId)
        {
            var customerId = GetCustomerId();

            if (string.IsNullOrEmpty(customerId))
                return Unauthorized();

            var items = await _service.GetByOrderIdAsync(
                orderId, customerId);

            if (items == null)
                return NotFound(new { message = "Order not found." });

            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateOrderItemDto dto)
        {
            var customerId = GetCustomerId();

            if (string.IsNullOrEmpty(customerId))
                return Unauthorized();

            try
            {
                var item = await _service.CreateAsync(dto, customerId);

                return Ok(item);
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

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id, UpdateOrderItemDto dto)
        {
            var customerId = GetCustomerId();

            if (string.IsNullOrEmpty(customerId))
                return Unauthorized();

            try
            {
                var item = await _service.UpdateAsync(
                    id, dto, customerId);

                if (item == null)
                    return NotFound(new { message = "Order item not found." });

                return Ok(item);
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

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var customerId = GetCustomerId();

            if (string.IsNullOrEmpty(customerId))
                return Unauthorized();

            try
            {
                var deleted = await _service.DeleteAsync(id, customerId);

                if (!deleted)
                    return NotFound(new { message = "Order item not found." });

                return Ok(new { message = "Order item removed successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}
