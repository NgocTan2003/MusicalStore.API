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

namespace MusicalStore.Application.Services.Implements
{

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenSerVice;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _config;
        private readonly DataContext _context;

        public UserService(IUserRepository userRepository, ICartService cartService, SignInManager<AppUser> signInManager,
            IMapper mapper, IWebHostEnvironment webHostEnvironment, ITokenService tokenSerVice, RoleManager<IdentityRole> roleManager
            , IEmailService emailService, UserManager<AppUser> userManager, IConfiguration config, DataContext context)
        {
            _userRepository = userRepository;
            _cartService = cartService;
            _signInManager = signInManager;
            _tokenSerVice = tokenSerVice;
            _roleManager = roleManager;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
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
                result.StatusCode = 400;
                result.Message = "No user found";
                return result;
            }
            else if (user.LockoutEnd != null)
            {
                result.StatusCode = 400;
                result.Message = $"The account is locked {user.LockoutEnd}";
                return result;
            }
            else if (user.TwoFactorEnabled == true)
            {
                result.StatusCode = 400;
                result.Message = "You must login with OTP";
                return result;
            }

            int accessFailedCount = await _userManager.GetAccessFailedCountAsync(user);
            var SignIn = await _signInManager.PasswordSignInAsync(user, request.Password, true, true);
            if (SignIn.Succeeded == false)
            {
                if (accessFailedCount >= 2)
                {
                    user.TwoFactorEnabled = true;
                }
                result.StatusCode = 400;
                result.Message = "Đăng nhập không thành công";
                return result;
            }
            else
            {
                var roles = await _userRepository.GetAllRoleByName(user.UserName);
                var claimUser = new ClaimUserLogin()
                {
                    Id = user.Id,
                    UserName = request.UserName,
                    Email = request.UserName,
                    Roles = roles
                };
                var token = _tokenSerVice.GenerateAccessToken(claimUser);
                var refeshToken = _tokenSerVice.GenerateRefreshToken();
                user.TokenExpirationTime = DateTime.Now.AddMinutes(2);
                user.RefeshToken = refeshToken;
                user.RefreshTokenExpirationTime = DateTime.Now.AddMinutes(4);
                await _context.SaveChangesAsync();
                await _userManager.UpdateAsync(user);

                result.Id = user.Id;
                result.UserName = user.UserName;
                result.AccessToken = token;
                result.RefeshToken = refeshToken;
                result.Message = "Đăng nhập thành công";
                result.TokenExpiration = DateTime.Now.AddMinutes(2);
                result.RefreshTokenExpiration = DateTime.Now.AddMinutes(4);
                return result;
            }
        }

        public async Task<ResponseMessage> SendEmailOTP(string username, string password)
        {
            var user = await _userRepository.GetUserByUsername(username);
            var result = new ResponseMessage();
            if (user.TwoFactorEnabled)
            {
                await _signInManager.SignOutAsync();
                await _signInManager.PasswordSignInAsync(user, password, false, true);
                var tokenOTP = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
                var message = _emailService.ChangeToMessageEmail(user.Email!, "OTP Confrimation", tokenOTP);
                result = await _emailService.SendEmail(message);
            }
            return result;
        }

        public async Task<TokenResponse> AuthenticationOTP(string code, string username)
        {
            var tokenResponse = new TokenResponse();
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

                    tokenResponse.Id = user.Id;
                    tokenResponse.UserName = user.UserName;
                    tokenResponse.AccessToken = token;
                    tokenResponse.Message = "Đăng nhập băng OTP thành công";
                }
            }
            else
            {
                tokenResponse.StatusCode = 400;
                tokenResponse.Message = "Đăng nhập không thành công";
            }
            return tokenResponse;
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

        public async Task<UserDto> GetUserByEmail(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        public async Task<UserDto> GetUserByUsername(string username)
        {
            var user = await _userRepository.GetUserByUsername(username);
            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        public async Task<IList<string>> GetAllRoleByName(string UserName)
        {
            return await _userRepository.GetAllRoleByName(UserName);
        }

        public async Task<bool> CheckCreateRole(RegisterRequest request)
        {
            bool checkRole = false;
            if (AppRole.Customer == request.Role || AppRole.Admin == request.Role || AppRole.Manager == request.Role)
            {
                checkRole = true;
                if (!await _roleManager.RoleExistsAsync(request.Role) && AppRole.Customer == request.Role)
                {
                    await _roleManager.CreateAsync(new IdentityRole(AppRole.Customer));
                }

                if (!await _roleManager.RoleExistsAsync(request.Role) && AppRole.Admin == request.Role)
                {
                    await _roleManager.CreateAsync(new IdentityRole(AppRole.Admin));
                }

                if (!await _roleManager.RoleExistsAsync(request.Role) && AppRole.Manager == request.Role)
                {
                    await _roleManager.CreateAsync(new IdentityRole(AppRole.Manager));
                }
            }

            return checkRole;
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

            bool checkRole = await CheckCreateRole(request);
            if (!checkRole)
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
            if (request.UploadFile != null)
            {
                user.Avatar = await UploadImage(request.UploadFile);
            }
            user.DateCreated = DateTime.Now;
            user.CreateBy = request.CreateBy;
            //user.TwoFactorEnabled = true;

            await _userRepository.CreateUser(user, request.PassWord);
            if (request.Role == AppRole.Customer)
            {
                roles.Add(AppRole.Customer);
                await _userRepository.AddRole(user, roles);
                await _cartService.CreateCart(user.Id);
            }
            else if (request.Role == AppRole.Admin)
            {
                roles.Add(AppRole.Admin);
                await _userRepository.AddRole(user, roles);
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            result.StatusCode = 200;
            result.Message = "CreateUser Success";
            result.token = token;
            result.Email = user.Email;
            return result;
        }

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

        public async Task<ResponseMessage> UpdateUser(UpdateUser request)
        {
            ResponseMessage responseMessage = new();

            var find = await _userRepository.GetUserByUsername(request.UserName);
            if (find.Email == request.Email)
            {
                responseMessage.Message = "Email already exist";
                responseMessage.StatusCode = 400;
            }
            else
            {
                find.PasswordHash = request.PassWord;
                find.Email = request.Email;
                find.FullName = request.FullName;
                find.Gender = request.Gender;
                find.PhoneNumber = request.PhoneNumber;
                find.UpdateBy = request.UpdateBy;
                find.ModifiedDate = DateTime.Now;
            }
            if (request.UploadFile != null)
            {
                find.Avatar = await UploadImage(request.UploadFile);
            }

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
            return responseMessage;
        }

        public async Task<string> UploadImage(UploadFile request)
        {

            var imageFolder = $@"\uploaded\Avatar\images\{DateTime.Now.ToString("yyyyMMdd")}";

            string folder = _webHostEnvironment.WebRootPath + imageFolder;
            var nameImage = $"{request.FileName}.jpg";

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string filePath = Path.Combine(folder, nameImage);

            using (FileStream fs = System.IO.File.Create(filePath))
            {
                fs.Write(request.Bytes, 0, request.Bytes.Length);
                fs.Flush();
            }
            var pathimage = Path.Combine(imageFolder, nameImage).Replace(@"\", @"/");
            var responseimage = "https://localhost:7099" + "/" + pathimage;
            return responseimage;
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
