using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicalStore.Application.Services.Interfaces;
using MusicalStore.Common.ResponseBase;
using MusicalStore.Dtos.CartItems;
using MusicalStore.Dtos.Categories;

namespace MusicalStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemService _cartItemService;

        public CartItemController(ICartItemService categoryService)
        {
            _cartItemService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var cartItem = await _cartItemService.GetAllCartItem();
                return Ok(cartItem);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetByID(Guid idCart, Guid idProduct)
        {
            try
            {
                var cartItem = await _cartItemService.GetCartItemById(idCart, idProduct);
                if (cartItem != null)
                {
                    return Ok(cartItem);
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
        public async Task<IActionResult> CreateCartItem([FromBody] CreateCartItem request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResponseMessage("Fail"));
                }
                var create = await _cartItemService.CreateCartItem(request);
                return Ok(create);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateCartItem([FromBody] UpdateCartItem request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResponseMessage("Fail"));
                }
                var update = await _cartItemService.UpdateCartItem(request);
                return Ok(update);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });

            }
        }

        [HttpDelete("idCart, idProduct")]
        public async Task<IActionResult> Delete(Guid idCart, Guid idProduct)
        {
            try
            {
                var delete = await _cartItemService.DeleteCartItem(idCart, idProduct);
                return Ok(delete);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}

