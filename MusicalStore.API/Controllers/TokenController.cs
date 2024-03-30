using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicalStore.Application.Services.Interfaces;
using MusicalStore.Data.EF;
using MusicalStore.Data.Entities;
using MusicalStore.Dtos.Users;

namespace MusicalStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly DataContext _context;

        public TokenController(ITokenService tokenService, UserManager<AppUser> userManager, DataContext context)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _context = context;
        }

        [HttpPost("RefreshToken")]
        public async Task<TokenResponse> Refresh([FromBody] TokenApiModel tokenApiModel)
        {
            var result = new TokenResponse();

            try
            {
                string accessToken = tokenApiModel.AccessToken;
                string refreshToken = tokenApiModel.RefreshToken;
                var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
                var username = principal.Identity.Name;

                var user = await _userManager.FindByNameAsync(username);
                var roles = await _userManager.GetRolesAsync(user);
                if (user == null)
                {
                    result.Message = "No user found";
                    return result;
                }
                else if (user.RefeshToken != refreshToken)
                {
                    result.Message = "The Refeshtoken code is not correct";
                    return result;
                }
                else if (user.RefreshTokenExpirationTime <= DateTime.Now)
                {
                    result.Message = "Refreshtoken has expired";
                    return result;
                }
                else if (user.TokenExpirationTime >= DateTime.Now)
                {
                    result.Message = "Token has not expired";
                    result.UserName = user.UserName;
                    result.AccessToken = tokenApiModel.AccessToken;
                    result.RefeshToken = tokenApiModel.RefreshToken;
                    return result;
                }

                //var utcExpireDate = long.Parse(principal.Claims.FirstOrDefault(x => x.Type == "exp").Value);
                //var expireDate = _tokenService.ConvertUnixTimeToDateTime(utcExpireDate);
                //var dateNow = DateTime.Now;
                //// nếu token chưa hết hạn thì return về token cũ 
                //if (expireDate > dateNow)
                //{
                //    result.UserName = user.UserName;
                //    result.AccessToken = tokenApiModel.AccessToken;
                //    result.RefeshToken = tokenApiModel.RefreshToken;
                //    return result;
                //}

                // nếu token đã hết hạn
                var request = new ClaimUserLogin()
                {
                    UserName = user.UserName,
                    Email = user.UserName,
                    Roles = roles
                };
                var newAccessToken = _tokenService.GenerateAccessToken(request);
                var newRefreshToken = _tokenService.GenerateRefreshToken();
                user.RefeshToken = newRefreshToken;
                user.TokenExpirationTime = DateTime.Now.AddMinutes(2);
                user.RefreshTokenExpirationTime = DateTime.Now.AddMinutes(4);
                _context.SaveChanges();
                await _userManager.UpdateAsync(user);

                result.UserName = username;
                result.AccessToken = newAccessToken;
                result.RefeshToken = newRefreshToken;
                result.TokenExpiration = DateTime.Now.AddMinutes(2);
                result.RefreshTokenExpiration = DateTime.Now.AddMinutes(4);
                result.StatusCode = 200;
                //};
                return result;
            }
            catch (Exception ex)
            {
                result.AccessToken = ex.Message;
                result.StatusCode = 400;
                //return BadRequest(new TokenResponse() { AccessToken = "Error", Message = ex.Message, UserName = null });
            }
            return result;
        }
    }
}
