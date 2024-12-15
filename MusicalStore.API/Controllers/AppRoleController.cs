using Microsoft.AspNetCore.Mvc;
using MusicalStore.Application.Services;

namespace MusicalStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppRoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public AppRoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRole()
        {
            try
            {
                var getall = await _roleService.GetAll();
                return Ok(getall);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
