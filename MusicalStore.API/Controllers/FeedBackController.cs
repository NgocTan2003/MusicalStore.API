using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicalStore.Application.AutoConfiguration;
using MusicalStore.Application.Services.Implements;
using MusicalStore.Application.Services.Interfaces;
using MusicalStore.Dtos.Categories;
using MusicalStore.Dtos.FeedBacks;

namespace MusicalStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedBackController : ControllerBase
    {
        private readonly IFeedBackService _feedBackService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FeedBackController(IFeedBackService feedBackService, IHttpContextAccessor httpContextAccessor)
        {
            _feedBackService = feedBackService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var feedBacks = await _feedBackService.GetAllFeedBack();
                return Ok(feedBacks);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("Pagination")]
        public async Task<IActionResult> GetPaginationFeedBack(int page)
        {
            try
            {
                var feedbacks = await _feedBackService.GetPaginationFeedBack(page);
                return Ok(feedbacks);
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
                var feedback = await _feedBackService.GetFeedBackById(id);
                if (feedback != null)
                {
                    return Ok(feedback);
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
        public async Task<IActionResult> CreateFeedBack([FromBody] CreateFeedback request)
        {
            try
            {
                var hasAccess = await AuthorizationHelper.CheckAccess(_httpContextAccessor.HttpContext, "Feedback", "Create");

                if (hasAccess.StatusCode == 200)
                {
                    var create = await _feedBackService.CreateFeedback(request);
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
        public async Task<IActionResult> UpdateFeedback([FromBody] UpdateFeedback request)
        {
            try
            {
                var hasAccess = await AuthorizationHelper.CheckAccess(_httpContextAccessor.HttpContext, "Feedback", "Update");

                if (hasAccess.StatusCode == 200)
                {
                    var update = await _feedBackService.UpdateFeedBack(request);
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
                var hasAccess = await AuthorizationHelper.CheckAccess(_httpContextAccessor.HttpContext, "Feedback", "Delete");

                if (hasAccess.StatusCode == 200)
                {
                    var delete = await _feedBackService.DeleteFeedBack(id);
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
