using FoodMenuManagement.API.DTOs.Category;
using FoodMenuManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodMenuManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoriesController(ICategoryService service)
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
            var category = await _service.GetByIdAsync(id);

            if (category == null)
                return NotFound(new { message = "Category not found." });

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryDto request)
        {
            try
            {
                var category = await _service.CreateAsync(request);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = category.Id },
                    category);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id, UpdateCategoryDto request)
        {
            try
            {
                var category = await _service.UpdateAsync(id, request);

                if (category == null)
                    return NotFound(new { message = "Category not found." });

                return Ok(category);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool deleted = await _service.DeleteAsync(id);

            if (!deleted)
                return NotFound(new { message = "Category not found." });

            return Ok(new { message = "Category deactivated successfully." });
        }
    }
}