using FoodMenuManagement.API.DTOs.FoodItem;
using FoodMenuManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodMenuManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodItemsController : ControllerBase
    {
        private readonly IFoodItemService _service;

        public FoodItemsController(IFoodItemService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var foodItem = await _service.GetByIdAsync(id);

            if (foodItem == null)
                return NotFound(new { message = "Food item not found." });

            return Ok(foodItem);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateFoodItemDto request)
        {
            try
            {
                var foodItem = await _service.CreateAsync(request);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = foodItem.Id },
                    foodItem);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id, UpdateFoodItemDto request)
        {
            try
            {
                var foodItem = await _service.UpdateAsync(id, request);

                if (foodItem == null)
                    return NotFound(new { message = "Food item not found." });

                return Ok(foodItem);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool deleted = await _service.DeleteAsync(id);

            if (!deleted)
                return NotFound(new { message = "Food item not found." });

            return Ok(new
            {
                message = "Food item deactivated successfully."
            });
        }
    }
}