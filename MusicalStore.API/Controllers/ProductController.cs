using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicalStore.Application.AutoConfiguration;
using MusicalStore.Application.Services.Implements;
using MusicalStore.Application.Services.Interfaces;
using MusicalStore.Common.ResponseBase;
using MusicalStore.Dtos.Categories;
using MusicalStore.Dtos.Products;

namespace MusicalStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductController(IProductService productService, IHttpContextAccessor httpContextAccessor)
        {
            _productService = productService;
            _httpContextAccessor = httpContextAccessor;
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
        [Authorize]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProduct request)
        {
            try
            {
                var hasAccess = await AuthorizationHelper.CheckAccess(_httpContextAccessor.HttpContext, "Product", "Create");

                if (hasAccess.StatusCode == 200)
                {
                    var create = await _productService.CreateProduct(request);
                    return Ok(create);
                }
                else
                {
                    return Ok(hasAccess);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPut("Update")]
        [Authorize]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProduct request)
        {
            try
            {
                var hasAccess = await AuthorizationHelper.CheckAccess(_httpContextAccessor.HttpContext, "Product", "Update");

                if (hasAccess.StatusCode == 200)
                {
                    var update = await _productService.UpdateProduct(request);
                    return Ok(update);
                }
                else
                {
                    return Ok(hasAccess);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });

            }
        }

        [HttpDelete("id")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var hasAccess = await AuthorizationHelper.CheckAccess(_httpContextAccessor.HttpContext, "Product", "Delete");

                if (hasAccess.StatusCode == 200)
                {
                    var delete = await _productService.DeleteProduct(id);
                    return Ok(delete);
                }
                else
                {
                    return Ok(hasAccess);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}
