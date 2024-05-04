using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MusicalStore.Application.Repositories.Interfaces;
using MusicalStore.Application.Services.Interfaces;
using MusicalStore.Common.Enums;
using MusicalStore.Common.ResponseBase;
using MusicalStore.Data.EF;
using MusicalStore.Data.Entities;
using MusicalStore.Data.Enums;
using MusicalStore.Dtos.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Collections;
using System.Data;
using MusicalStore.Dtos.Carts;
using System.Diagnostics.Eventing.Reader;
using MusicalStore.Application.AutoConfiguration;
using Org.BouncyCastle.Asn1.Ocsp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Amazon.Runtime.Internal;

namespace MusicalStore.Application.Services.Implements
{

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenSerVice;
        private readonly IEmailService _emailService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _config;
        private readonly DataContext _context;
        private readonly IAwsS3Service _awsS3Service;

        public UserService(IUserRepository userRepository, ICartService cartService, SignInManager<AppUser> signInManager,
            IMapper mapper, ITokenService tokenSerVice, IAwsS3Service awsS3Service, IEmailService emailService,
            UserManager<AppUser> userManager, IConfiguration config, DataContext context)
        {
            _userRepository = userRepository;
            _cartService = cartService;
            _signInManager = signInManager;
            _tokenSerVice = tokenSerVice;
            _awsS3Service = awsS3Service;
            _mapper = mapper;
            _emailService = emailService;
            _userManager = userManager;
            _config = config;
            _context = context;
        }
        public async Task<TokenResponse> Authentication(AuthenticationRequest request)
        {
            var result = new TokenResponse();
            var user = await _userRepository.GetUserByUsername(request.UserName);

            if (user == null)
            {
                result.StatusCode = 404;
                result.Message = "No user found";
                return result;
            }
            else if (await _userManager.IsLockedOutAsync(user))
            {
                result.StatusCode = 400;
                result.Message = $"The account is locked {user.LockoutEnd}";
                return result;
            }

            if (user.TwoFactorEnabled)
            {
                if (await _userManager.CheckPasswordAsync(user, request.Password))
                {
                    await _signInManager.SignOutAsync();
                    await _signInManager.PasswordSignInAsync(user, request.Password, false, true);
                    var tokenOTP = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
                    var message = _emailService.ChangeToMessageEmail(user.Email!, "OTP Confrimation", tokenOTP);
                    var send = await _emailService.SendEmail(message);
                    result.StatusCode = send.StatusCode;
                    result.Message = send.Message;
                    return result;
                }
                else
                {
                    await _userManager.AccessFailedAsync(user);
                    result.StatusCode = 401;
                    result.Message = "Invalid password";
                    return result;
                }
            }

            var SignIn = await _signInManager.PasswordSignInAsync(user, request.Password, true, true);
            if (SignIn.Succeeded)
            {
                var roles = await _userRepository.GetAllRoleByName(user.UserName);
                var claimUser = new ClaimUserLogin()
                {
                    Id = user.Id,
                    UserName = request.UserName,
                    Email = user.Email,
                    Roles = roles
                };
                var token = _tokenSerVice.GenerateAccessToken(claimUser);
                var refeshToken = _tokenSerVice.GenerateRefreshToken();
                var timeToken = int.TryParse(_config["JWT:TokenExpirationTime"], out int tokenExpirationTime);
                var timeRefeshToken = int.TryParse(_config["JWT:RefreshTokenExpirationTime"], out int refeshtokenExpirationTime);

                user.TokenExpirationTime = DateTime.Now.AddMinutes(tokenExpirationTime);
                user.RefreshToken = refeshToken;
                user.RefreshTokenExpirationTime = DateTime.Now.AddMinutes(refeshtokenExpirationTime);
                await _context.SaveChangesAsync();
                await _userManager.UpdateAsync(user);

                var response = new TokenResponse(user.Id, user.UserName, token, refeshToken, 200, "Login success", DateTime.Now.AddMinutes(tokenExpirationTime), DateTime.Now.AddMinutes(refeshtokenExpirationTime));
                return response;
            }
            else
            {
                int accessFailedCount = await _userManager.GetAccessFailedCountAsync(user);
                if (accessFailedCount >= 2)
                {
                    user.TwoFactorEnabled = true;
                }
                await _context.SaveChangesAsync();
                await _userManager.UpdateAsync(user);
                result.StatusCode = 401;
                result.Message = "Invalid password";
                return result;
            }
        }

        public async Task<TokenResponse> AuthenticationOTP(string code, string username)
        {
            var result = new TokenResponse();
            var user = await _userRepository.GetUserByUsername(username);
            var singIn = await _signInManager.TwoFactorSignInAsync("Email", code, false, false);

            if (singIn.Succeeded)
            {
                var userRole = await _userManager.GetRolesAsync(user);
                user.TwoFactorEnabled = false;
                if (user != null)
                {
                    var claimUser = new ClaimUserLogin()
                    {
                        Id = user.Id,
                        UserName = username,
                        Email = user.Email,
                        Roles = userRole
                    };

                    var token = _tokenSerVice.GenerateAccessToken(claimUser);
                    var refeshToken = _tokenSerVice.GenerateRefreshToken();
                    var timeToken = int.TryParse(_config["JWT:TokenExpirationTime"], out int tokenExpirationTime);
                    var timeRefeshToken = int.TryParse(_config["JWT:RefreshTokenExpirationTime"], out int refeshtokenExpirationTime);

                    user.TokenExpirationTime = DateTime.Now.AddMinutes(tokenExpirationTime);
                    user.RefreshToken = refeshToken;
                    user.RefreshTokenExpirationTime = DateTime.Now.AddMinutes(refeshtokenExpirationTime);
                    await _context.SaveChangesAsync();
                    await _userManager.UpdateAsync(user);

                    var response = new TokenResponse(user.Id, user.UserName, token, refeshToken, 200, "Login success", DateTime.Now.AddMinutes(tokenExpirationTime), DateTime.Now.AddMinutes(refeshtokenExpirationTime));
                    return response;
                }
            }
            else
            {
                result.StatusCode = 401;
                result.Message = "Invalid OTP";
            }
            return result;
        }

        public async Task<List<UserDto>> GetAllUser()
        {
            var users = await _userRepository.GetAllUser();
            var usersDto = _mapper.Map<List<UserDto>>(users);
            return usersDto;
        }

        public async Task<UserDto> GetUserById(string id)
        {
            var user = await _userRepository.GetUserById(id);
            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        public async Task<ResponseCreateUser> CreateUser(RegisterRequest request)
        {
            var result = new ResponseCreateUser();
            List<string> roles = new List<string>();

            var findUserName = await _userRepository.GetUserByUsername(request.UserName);
            var findEmail = await _userRepository.GetUserByEmail(request.Email);
            if (findUserName != null || findEmail != null)
            {
                result.StatusCode = 400;
                result.Message = "UserName or Email has been used";
                return result;
            }

            var roleInput = request.Role;
            if (roleInput.Equals("Admin") || roleInput.Equals("Manager") || roleInput.Equals("Customer"))
            {
                result.StatusCode = 400;
                result.Message = "Can not find the role";
                return result;
            }

            var user = new AppUser();
            user.Id = Guid.NewGuid().ToString();
            user.UserName = request.UserName;
            user.PasswordHash = request.PassWord;
            user.Email = request.Email;
            user.FullName = request.FullName;
            user.Gender = request.Gender;
            user.PhoneNumber = request.PhoneNumber;
            user.DateCreated = DateTime.Now;
            user.CreateBy = request.CreateBy;

            await _userRepository.CreateUser(user, request.PassWord);
            if (request.Role == "Customer")
            {
                roles.Add(roleInput);
                await _userRepository.AddRole(user, roles);
                await _cartService.CreateCart(user.Id);
            }
            else if (request.Role == "Admin")
            {
                roles.Add(roleInput);
                await _userRepository.AddRole(user, roles);
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            result.StatusCode = 200;
            result.Message = "CreateUser Success";
            result.token = token;
            result.Email = user.Email;
            return result;
        }

        // khi user ForgotPassword sẽ gửi 1 đường link đến email để reset password
        public async Task<ResponseMessage> ForgotPassword(string EmailForgotPassword)
        {
            var result = new ResponseMessage();
            var user = await _userRepository.GetUserByEmail(EmailForgotPassword);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var Address = "https://localhost:7099/swagger/index.html";
                var forgotPasswordLink = $"{Address}?token={token}&email={EmailForgotPassword}";
                var message = _emailService.ChangeToMessageEmail(user.Email, "Forgot Password Link", forgotPasswordLink!);
                result = await _emailService.SendEmail(message);
                return result;
            }
            else
            {
                result.StatusCode = 400;
                result.Message = "No email found";
            }
            return result;
        }

        public async Task<ResponseMessage> ResetPassword(ResetPassword resetPassword)
        {
            var result = new ResponseMessage();
            var user = await _userRepository.GetUserByEmail(resetPassword.Email);
            if (user != null)
            {
                var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.ConfirmPassword);
                if (resetPassResult.Succeeded)
                {
                    result.StatusCode = 200;
                    result.Message = "Password has been changed";
                }
                else
                {
                    result.StatusCode = 400;
                    result.Message = "Change password fail";
                }
            }
            return result;
        }

        public async Task<ResponseMessage> ChangePassword(ChangePassword changePassword)
        {
            var result = new ResponseMessage();
            var user = await _userRepository.GetUserByUsername(changePassword.UserName);
            if (user == null)
            {
                result.StatusCode = 404;
                result.Message = "User not found.";
                return result;
            }
            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, changePassword.PasswordOld);
            if (!isPasswordCorrect)
            {
                result.StatusCode = 400;
                result.Message = "Incorrect old password.";
                return result;
            }
            var changeResult = await _userManager.ChangePasswordAsync(user, changePassword.PasswordOld, changePassword.PasswordNew);
            if (!changeResult.Succeeded)
            {
                result.StatusCode = 400;
                result.Message = "Failed to change password.";
                return result;
            }
            result.StatusCode = 200;
            result.Message = "Password changed successfully.";
            return result;
        }

        public async Task<ResponseMessage> UpdateUser(UpdateUser request)
        {
            ResponseMessage responseMessage = new();
            try
            {
                var find = await _userRepository.GetUserByUsername(request.UserName);
                find.FullName = request.FullName;
                find.Gender = request.Gender;
                find.PhoneNumber = request.PhoneNumber;
                find.UpdateBy = request.UpdateBy;
                find.ModifiedDate = DateTime.Now;

                var update = await _userRepository.UpdateUser(find);
                if (update)
                {
                    responseMessage.Message = "Success";
                    responseMessage.StatusCode = 200;
                }
                else
                {
                    responseMessage.Message = "Fail";
                    responseMessage.StatusCode = 500;
                }
            }
            catch (Exception ex)
            {
                responseMessage.Message = ex.Message;
                responseMessage.StatusCode = 500;
            }
            return responseMessage;
        }

        public async Task<ResponseMessage> UpdateAvatar(string username, IFormFile file, string bucketName, string? prefix, string? namefile)
        {
            var responseMessage = new ResponseMessage();
            try
            {
                var user = await _userRepository.GetUserByUsername(username);
                var response = await _awsS3Service.UploadFile(file, bucketName, prefix, namefile);
                if (response.StatusCode == 200)
                {
                    user.Avatar = response.PresignedUrl;
                    var update = await _userRepository.UpdateUser(user);
                    if (update)
                    {
                        responseMessage.Message = "Update Avatar Success";
                        responseMessage.StatusCode = 200;
                    }
                    else
                    {
                        responseMessage.Message = "Update Avatar Fail";
                        responseMessage.StatusCode = 500;
                    }
                }
                else
                {
                    responseMessage.StatusCode = response.StatusCode;
                    response.Message = response.Message;
                }
            }
            catch (Exception ex)
            {
                responseMessage.Message = ex.Message;
                responseMessage.StatusCode = 500;
            }
            return responseMessage;
        }

        public async Task<ResponseMessage> DeleteUser(string id)
        {
            ResponseMessage responseMessage = new();

            var user = await _userRepository.GetUserById(id);

            if (user == null)
            {
                responseMessage.Message = "Not Found";
                responseMessage.StatusCode = 400;

                return responseMessage;
            }
            else
            {
                var delete = await _userRepository.DeleteUser(user);
                if (delete)
                {
                    responseMessage.Message = "Success";
                    responseMessage.StatusCode = 200;
                }
                else
                {
                    responseMessage.Message = "Fail";
                    responseMessage.StatusCode = 500;
                }
                return responseMessage;
            }
        }


    }
}
