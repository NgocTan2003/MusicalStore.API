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

        public UserService(IUserRepository userRepository, ICartService cartService, SignInManager<AppUser> signInManager, IMapper mapper
            , IWebHostEnvironment webHostEnvironment, ITokenService tokenSerVice, RoleManager<IdentityRole> roleManager)
        {
            _userRepository = userRepository;
            _cartService = cartService;
            _signInManager = signInManager;
            _tokenSerVice = tokenSerVice;
            _roleManager = roleManager;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<TokenResponse> Authentication(AuthenticationRequest request)
        {
            var user = await _userRepository.GetUserByUsername(request.UserName);

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, true, true);
            if (result.Succeeded == false)
            {
                return new TokenResponse()
                {
                    StatusCode = 400,
                    Message = "Đăng nhập không thành công"
                };
            }

            var roles = await _userRepository.GetAllRoleByName(user.UserName);
            var claimUser = new ClaimUserLogin()
            {
                Id = user.Id,
                UserName = request.UserName,
                Email = request.UserName,
                Roles = roles
            };

            var token = _tokenSerVice.GenerateAccessToken(claimUser);

            return new TokenResponse()
            {
                Id = user.Id,
                UserName = user.UserName,
                AccessToken = token,
                Message = "Đăng nhập thành công"
            };
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
            if (AppRole.Customer == request.Role || AppRole.Admin == request.Role)
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
            }

            return checkRole;
        }

        public async Task<ResponseMessage> CreateUser(RegisterRequest request)
        {
            ResponseMessage responseMessage = new();
            List<string> roles = new List<string>();

            var findUserName = await _userRepository.GetUserByUsername(request.UserName);
            var findEmail = await _userRepository.GetUserByEmail(request.Email);

            if (findUserName != null || findEmail != null)
            {
                responseMessage.Message = "Username or email already exist";
                responseMessage.StatusCode = 400;

                return responseMessage;
            }

            var checkRole = CheckCreateRole(request);

            if (checkRole == null)
            {
                responseMessage.Message = "Can not find the role";
                responseMessage.StatusCode = 400;
            }
            else
            {
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

                var createUser = await _userRepository.CreateUser(user, request.PassWord);

                if (createUser)
                {
                    if (request.Role == AppRole.Customer)
                    {
                        roles.Add(AppRole.Customer);
                        var createRole = await _userRepository.AddRole(user, roles);

                        var createCart = await _cartService.CreateCart(user.Id);
                        if (createCart.StatusCode == 200 && createRole)
                        {
                            responseMessage.Message = "Success";
                            responseMessage.StatusCode = 200;
                        }
                    }
                    if (request.Role == AppRole.Admin)
                    {
                        roles.Add(AppRole.Admin);
                        var createRole = await _userRepository.AddRole(user, roles);
                        if (createRole)
                        {
                            responseMessage.Message = "Success";
                            responseMessage.StatusCode = 200;
                        }
                    }
                }
                else
                {
                    responseMessage.Message = "Fail";
                    responseMessage.StatusCode = 500;
                }
            }

            return responseMessage;
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
