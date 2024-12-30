using Microsoft.AspNetCore.Mvc;
using MusicalStore.Application.Services.Interfaces;
using MusicalStore.Common.ResponseBase;
using MusicalStore.Dtos.Users;

namespace MusicalStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("RefreshToken")]
        public async Task<TokenResponse> Refresh([FromBody] TokenApiModel tokenApiModel)
        {
            var result = new TokenResponse();
            try
            {
                result = await _tokenService.RefreshToken(tokenApiModel);
            }
            catch (Exception ex)
            {
                result.AccessToken = ex.Message;
                result.StatusCode = 400;
            }
            return result;
        }
    }
}
