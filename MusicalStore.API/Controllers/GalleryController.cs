using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicalStore.Application.AutoMapper;
using MusicalStore.Application.Services.Interfaces;
using MusicalStore.Dtos.FeedBacks;
using MusicalStore.Dtos.Galleries;

namespace MusicalStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GalleryController : ControllerBase
    {
        private readonly IGalleryService _galleryService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GalleryController(IGalleryService galleryService, IHttpContextAccessor httpContextAccessor)
        {
            _galleryService = galleryService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var galleries = await _galleryService.GetAllGallery();
                return Ok(galleries);
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
                var gallery = await _galleryService.GetGalleryById(id);
                if (gallery != null)
                {
                    return Ok(gallery);
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
        public async Task<IActionResult> CreateGallery([FromBody] CreateGallery request)
        {
            try
            {
                var hasAccess = await AuthorizationHelper.CheckAccess(_httpContextAccessor.HttpContext, "Gallery", "Create");

                if (hasAccess.StatusCode == 200)
                {
                    var create = await _galleryService.CreateGallery(request);
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
        public async Task<IActionResult> UpdateGallery([FromBody] UpdateGallery request)
        {
            try
            {
                var hasAccess = await AuthorizationHelper.CheckAccess(_httpContextAccessor.HttpContext, "Gallery", "Update");

                if (hasAccess.StatusCode == 200)
                {
                    var update = await _galleryService.UpdateGallery(request);
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
                var hasAccess = await AuthorizationHelper.CheckAccess(_httpContextAccessor.HttpContext, "Gallery", "Delete");

                if (hasAccess.StatusCode == 200)
                {
                    var delete = await _galleryService.DeleteGallery(id);
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
