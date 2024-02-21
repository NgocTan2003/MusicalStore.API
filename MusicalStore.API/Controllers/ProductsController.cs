using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicalStore.Application.Services.Implements;
using MusicalStore.Application.Services.Interfaces;
using MusicalStore.Common.ResponseBase;
using MusicalStore.Dtos.Categories;
using MusicalStore.Dtos.Products;

namespace MusicalStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await _productService.GetAllProduct();
                return Ok(products);
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
                var product = await _productService.GetProductById(id);
                if (product != null)
                {
                    return Ok(product);
                }
                else
                {
                    return BadRequest(new { Message = "Không tìm thấy sản phẩm." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateProduct request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResponseMessage("Fail"));
                }
                var create = await _productService.CreateProduct(request);
                return Ok(create);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateProduct request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResponseMessage("Fail"));
                }
                var update = await _productService.UpdateProduct(request);
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
                var delete = await _productService.DeleteProduct(id);
                return Ok(delete);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}
