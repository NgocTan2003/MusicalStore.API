using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicalStore.Application.AutoConfiguration;
using MusicalStore.Application.Services.Interfaces;
using MusicalStore.Dtos.Galleries;
using MusicalStore.Dtos.Orders;

namespace MusicalStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderController(IOrderService orderService, IHttpContextAccessor httpContextAccessor)
        {
            _orderService = orderService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var orders = await _orderService.GetAllOrder();
                return Ok(orders);
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
                var order = await _orderService.GetOrderById(id);
                if (order != null)
                {
                    return Ok(order);
                }
                else
                {
                    return BadRequest(new { Message = "Not Found" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("Create")]
        [Authorize]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrder request)
        {
            try
            {
                var hasAccess = await AuthorizationHelper.CheckAccess(_httpContextAccessor.HttpContext, "Order", "Create");

                if (hasAccess.StatusCode == 200)
                {
                    var create = await _orderService.CreateOrder(request);
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
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrder request)
        {
            try
            {
                var hasAccess = await AuthorizationHelper.CheckAccess(_httpContextAccessor.HttpContext, "Order", "Update");

                if (hasAccess.StatusCode == 200)
                {
                    var update = await _orderService.UpdateOrder(request);
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
                var hasAccess = await AuthorizationHelper.CheckAccess(_httpContextAccessor.HttpContext, "Order", "Delete");

                if (hasAccess.StatusCode == 200)
                {
                    var delete = await _orderService.DeleteOrder(id);
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
