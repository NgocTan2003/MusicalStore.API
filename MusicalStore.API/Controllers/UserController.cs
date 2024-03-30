using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicalStore.Application.AutoConfiguration;
using MusicalStore.Application.Services;
using MusicalStore.Application.Services.Implements;
using MusicalStore.Application.Services.Interfaces;
using MusicalStore.Common.ResponseBase;
using MusicalStore.Data.EF;
using MusicalStore.Data.Entities;
using MusicalStore.Data.Enums;
using MusicalStore.Dtos.AppRole;
using MusicalStore.Dtos.Users;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Security.Claims;

namespace MusicalStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly DataContext _context;

        public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor, IEmailService emailService, 
            UserManager<AppUser> userManager, ITokenService tokenService, DataContext context)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _userManager = userManager;
            _tokenService = tokenService;
            _context = context;
        }

        [HttpPost("Authentication")]
        public async Task<IActionResult> Authen([FromBody] AuthenticationRequest request)
        {
            try
            {
                var auth = await _userService.Authentication(request);
                return Ok(auth);
            }
            catch (Exception ex)
            {
                return BadRequest(new TokenResponse() { AccessToken = "lỗi rồi", Message = ex.Message, UserName = null });
            }
        }

        [HttpPost("SendEmailOTP")]
        public async Task<IActionResult> SendEmailOTP(string username, string password)
        {
            try
            {
                var auth = await _userService.SendEmailOTP(username, password);
                return Ok(auth);
            }
            catch (Exception ex)
            {
                return BadRequest(new TokenResponse() { AccessToken = "lỗi rồi", Message = ex.Message, UserName = null });
            }
        }

        [HttpPost("Authentication-2FA")]
        public async Task<IActionResult> Authen2FA(string code, string username)
        {
            try
            {
                var auth = await _userService.AuthenticationOTP(code, username);
                return Ok(auth);
            }
            catch (Exception ex)
            {
                return BadRequest(new TokenResponse() { AccessToken = "lỗi rồi", Message = ex.Message, UserName = null });
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterRequest request)
        {
            try
            {
                var result = await _userService.CreateUser(request);
                if (result.token == null)
                {
                    return Ok(result);
                }
                var confirmationLink = Url.Action(nameof(ConfirmEmail), "User", new { result.token, email = result.Email }, Request.Scheme);
                var message = _emailService.ChangeToMessageEmail(result.Email, "Confirmation email Link", confirmationLink);
                var response = await _emailService.SendEmail(message);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { StatusCode = 500, Message = ex.Message });
            }
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status200OK,
                        new ResponseMessage { StatusCode = 200, Message = "Email Verified Successfully" });
                }
            }
            return StatusCode(StatusCodes.Status500InternalServerError,
                                   new ResponseMessage { StatusCode = 500, Message = "This User Doenot exist" });
        }

        [HttpPost("SendEmailForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string EmailForgotPassword)
        {
            try
            {
                var response = await _userService.ForgotPassword(EmailForgotPassword);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { StatusCode = 500, Message = ex.Message });
            }
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPassword ResetPassword)
        {
            try
            {
                var response = await _userService.ResetPassword(ResetPassword);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { StatusCode = 500, Message = ex.Message });
            }
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePassword ChangePassword)
        {
            try
            {
                var response = await _userService.ChangePassword(ChangePassword);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { StatusCode = 500, Message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _userService.GetAllUser();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("id")]
        [Authorize]
        public async Task<IActionResult> FindUserIdentity(string id)
        {
            try
            {
                var hasAccess = await AuthorizationHelper.CheckAccess(_httpContextAccessor.HttpContext, "User", "Read");

                if (hasAccess.StatusCode == 200)
                {
                    var user = await _userService.GetUserById(id);
                    return Ok(user);
                }
                else
                {
                    return Ok(hasAccess);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("Update")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUser request)
        {
            try
            {
                var hasAccess = await AuthorizationHelper.CheckAccess(_httpContextAccessor.HttpContext, "User", "Update");

                if (hasAccess.StatusCode == 200)
                {
                    var update = await _userService.UpdateUser(request);
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
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var hasAccess = await AuthorizationHelper.CheckAccess(_httpContextAccessor.HttpContext, "User", "Delete");

                if (hasAccess.StatusCode == 200)
                {
                    var delete = await _userService.DeleteUser(id);
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
