using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicalStore.Application.AutoConfiguration;
using MusicalStore.Application.Services.Interfaces;
using MusicalStore.Dtos.OrderDetails;

namespace MusicalStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderDetailController(IOrderDetailService orderDetailService, IHttpContextAccessor httpContextAccessor)
        {
            _orderDetailService = orderDetailService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var orders = await _orderDetailService.GetAllOrderDetail();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("Pagination")]
        public async Task<IActionResult> GetPaginationOrderDetail(int page)
        {
            try
            {
                var orderDetails = await _orderDetailService.GetPaginationOrderDetail(page);
                return Ok(orderDetails);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("OrderID")]
        public async Task<IActionResult> GetByOrderID(Guid OrderID)
        {
            try
            {
                var orderDetail = await _orderDetailService.GetOrderDetailByOrder(OrderID);
                if (orderDetail != null)
                {
                    return Ok(orderDetail);
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

        [HttpGet("OrderID, ProductID")]
        public async Task<IActionResult> GetByOrderIDandProductID(Guid OrderID, Guid ProductID)
        {
            try
            {
                var orderDetail = await _orderDetailService.GetOrderDetailByID(OrderID, ProductID);
                if (orderDetail != null)
                {
                    return Ok(orderDetail);
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
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDetail request)
        {
            try
            {
                var hasAccess = await AuthorizationHelper.CheckAccess(_httpContextAccessor.HttpContext, "OrderDetail", "Create");

                if (hasAccess.StatusCode == 200)
                {
                    var create = await _orderDetailService.CreateOrderDetail(request);
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
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderDetail request)
        {
            try
            {
                var hasAccess = await AuthorizationHelper.CheckAccess(_httpContextAccessor.HttpContext, "OrderDetail", "Update");

                if (hasAccess.StatusCode == 200)
                {
                    var update = await _orderDetailService.UpdateOrderDetail(request);
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

        [HttpDelete("orderID, productID")]
        [Authorize]
        public async Task<IActionResult> DeleteById(Guid orderID, Guid productID)
        {
            try
            {
                var hasAccess = await AuthorizationHelper.CheckAccess(_httpContextAccessor.HttpContext, "OrderDetail", "Delete");

                if (hasAccess.StatusCode == 200)
                {
                    var delete = await _orderDetailService.DeleteOrderDetailById(orderID, productID);
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

        [HttpDelete("orderID")]
        [Authorize]
        public async Task<IActionResult> DeleteByOrder(Guid orderID)
        {
            try
            {
                var hasAccess = await AuthorizationHelper.CheckAccess(_httpContextAccessor.HttpContext, "OrderDetail", "Delete");

                if (hasAccess.StatusCode == 200)
                {
                    var delete = await _orderDetailService.DeleteOrderDetailByOrder(orderID);
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
