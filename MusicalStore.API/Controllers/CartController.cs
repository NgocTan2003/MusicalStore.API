using Microsoft.AspNetCore.Mvc;
using MusicalStore.Application.Services.Interfaces;

namespace MusicalStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {

        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var carts = await _cartService.GetAllCart();
                return Ok(carts);
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
                var cart = await _cartService.GetCartById(id);
                if (cart != null)
                {
                    return Ok(cart);
                }
                else
                {
                    return BadRequest(new { Message = "Không tìm thấy giỏ hàng." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
