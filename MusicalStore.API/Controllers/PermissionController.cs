using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicalStore.Application.AutoConfiguration;
using MusicalStore.Application.Services.Implements;
using MusicalStore.Application.Services.Interfaces;
using MusicalStore.Dtos.Galleries;
using MusicalStore.Dtos.Permissions;

namespace MusicalStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PermissionController(IPermissionService permissionService, IHttpContextAccessor httpContextAccessor)
        {
            _permissionService = permissionService;
            _httpContextAccessor = httpContextAccessor;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var permissions = await _permissionService.GetAllPermission();
                return Ok(permissions);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("Update")]
        [Authorize]
        public async Task<IActionResult> UpdatePermission([FromBody] UpdatePermission request)
        {
            try
            {
                var hasAccess = await AuthorizationHelper.CheckAccess(_httpContextAccessor.HttpContext, "Permission", "Update");

                if (hasAccess.StatusCode == 200)
                {
                    var update = await _permissionService.UpdatePermission(request);
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


    }
}
