using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicalStore.Application.Services.Implements;
using MusicalStore.Application.Services.Interfaces;
using MusicalStore.Common.ResponseBase;
using MusicalStore.Dtos.Categories;
using MusicalStore.Dtos.Users;

namespace MusicalStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var categories = await _categoryService.GetAllCategory();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetByID(Guid id)
        {
            try
            {
                var category = await _categoryService.GetCategoryById(id);
                if (category != null)
                {
                    return Ok(category);
                }
                else
                {
                    return BadRequest(new { Message = "Không tìm thấy danh mục." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategory request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResponseMessage("Fail"));
                }
                var create = await _categoryService.CreateCategory(request);
                return Ok(create);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategory request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResponseMessage("Fail"));
                }
                var update = await _categoryService.UpdateCategory(request);
                return Ok(update);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });

            }
        }

        [HttpDelete("id")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var delete = await _categoryService.DeleteCategory(id);
                return Ok(delete);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}
