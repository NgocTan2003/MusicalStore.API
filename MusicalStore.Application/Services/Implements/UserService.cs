using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using MusicalStore.Application.AutoConfiguration;
using MusicalStore.Application.Repositories.Interfaces;
using MusicalStore.Application.Services.Interfaces;
using MusicalStore.Common.ResponseBase;
using MusicalStore.Data.EF;
using MusicalStore.Data.Entities;
using MusicalStore.Dtos.Users;

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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IActionContextAccessor _actionContextAccessor;

        public UserService(IUserRepository userRepository, ICartService cartService, SignInManager<AppUser> signInManager, IActionContextAccessor actionContextAccessor,
            IMapper mapper, ITokenService tokenSerVice, IAwsS3Service awsS3Service, IEmailService emailService, IUrlHelperFactory urlHelperFactory,
            UserManager<AppUser> userManager, IConfiguration config, DataContext context, RoleManager<IdentityRole> roleManager)
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
            _roleManager = roleManager;
            _urlHelperFactory = urlHelperFactory;
            _actionContextAccessor = actionContextAccessor;
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
            else if (!user.EmailConfirmed)
            {
                result.StatusCode = 400;
                result.Message = "The account with emails have not been authenticated ";
                return result;
            }

            // account bắt login bằng 2 bước xác thực
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

        public async Task<bool> CheckCreateRole(string nameRole)
        {
            bool checkRole = false;
            if (Role.Customer == nameRole || Role.Admin == nameRole || Role.Manager == nameRole)
            {
                checkRole = true;
                if (!await _roleManager.RoleExistsAsync(nameRole) && Role.Customer == nameRole)
                {
                    await _roleManager.CreateAsync(new IdentityRole(Role.Customer));
                }
                if (!await _roleManager.RoleExistsAsync(nameRole) && Role.Manager == nameRole)
                {
                    await _roleManager.CreateAsync(new IdentityRole(Role.Manager));
                }
                if (!await _roleManager.RoleExistsAsync(nameRole) && Role.Admin == nameRole)
                {
                    await _roleManager.CreateAsync(new IdentityRole(Role.Admin));
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
            if (findUserName != null)
            {
                result.StatusCode = 400;
                result.Message = "UserName has been used";
                return result;
            }
            else if (findEmail != null)
            {
                result.StatusCode = 400;
                result.Message = "Email has been used";
                return result;
            }

            var checkRole = await CheckCreateRole(request.Role);
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
            user.DateCreated = DateTime.Now;
            user.CreateBy = request.CreateBy;

            var createUser = await _userRepository.CreateUser(user, request.PassWord);
            if (!createUser)
            {
                result.Message = "Create User Fail.";
                return result;
            }
            if (request.Role == "Customer")
            {
                roles.Add("Customer");
                await _userRepository.AddRole(user, roles);
                await _cartService.CreateCart(user.Id);
            }
            else if (request.Role == "Manager")
            {
                roles.Add("Manager");
                await _userRepository.AddRole(user, roles);
            }
            else if (request.Role == "Administrator")
            {
                roles.Add("Administrator");
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
                    result.Message = "Change password success";
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

        public async Task<ResponseMessage> SendEmailConfirm(string email)
        {
            ResponseMessage responseMessage = new();
            if (email != "")
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var result = await _userManager.ConfirmEmailAsync(user, token);

                    var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);
                    var confirmationLink = urlHelper.Action(
                         "ConfirmEmail",
                         "User",
                         new { token, email },
                         _actionContextAccessor.ActionContext.HttpContext.Request.Scheme
                     );

                    var message = _emailService.ChangeToMessageEmail(
                        email, "Email Confirmation Link",
                        $"Please confirm your email by clicking the link: <a href='{confirmationLink}'>Confirm Email</a>"
                    );
                    var response = await _emailService.SendEmail(message);

                    if (response.StatusCode == 200)
                    {
                        responseMessage.Message = "Send Email Success";
                        responseMessage.StatusCode = 200;
                    }
                    else
                    {
                        responseMessage.Message = "Send Email Fail";
                        responseMessage.StatusCode = 400;
                    }
                }
                else
                {
                    responseMessage.Message = "No email found";
                    responseMessage.StatusCode = 400;
                }
            }
            else
            {
                responseMessage.Message = "Email is null";
                responseMessage.StatusCode = 400;
            }
            return responseMessage;
        }
    }
}
