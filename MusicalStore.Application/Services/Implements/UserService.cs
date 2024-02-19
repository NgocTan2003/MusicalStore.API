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

namespace MusicalStore.Application.Services.Implements
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserService(IUserRepository userRepository, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<List<UserDto>> GetAllUser()
        {
            var users = await _userRepository.GetAllUser();
            var usersDto = _mapper.Map<List<UserDto>>(users);
            return usersDto;
        }

        public async Task<UserDto> GetUserById(Guid id)
        {
            var users = await _userRepository.GetUserById(id);
            var usersDto = _mapper.Map<UserDto>(users);
            return usersDto;
        }

        public async Task<bool> UserExists(Guid id)
        {
            return await _userRepository.UserExists(id);
        }

        public Task<User> GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseMessage> CreateUser(RegisterRequest request)
        {

            ResponseMessage responseMessage = new();
            var findUserName = await _userRepository.GetUserByUsername(request.UserName);
            var findEmail = await _userRepository.GetUserByEmail(request.Email);

            if (findUserName != null || findEmail != null)
            {
                responseMessage.Message = "Tài khoản đã tồn tại";
                responseMessage.StatusCode = 400;
            }
            else
            {
                var user = new User()
                {
                    UserID = Guid.NewGuid(),
                    UserName = request.UserName,
                    PassWord = request.PassWord,
                    Email = request.Email,
                    FullName = request.FullName,
                    Gender = request.Gender,
                    PhoneNumber = request.PhoneNumber,
                    Avatar = await UploadImage(request.UploadFile),
                    DateCreated = DateTime.Now
                };

                var create = await _userRepository.CreateUser(user);
                if (create == user.UserID)
                {
                    responseMessage.Message = "Đăng kí thành công";
                    responseMessage.StatusCode = 200;
                }
                else
                {
                    responseMessage.Message = "Đăng kí không thành công";
                    responseMessage.StatusCode = 500;
                }
            }
            return responseMessage;

        }

        public async Task<string> UploadImage(UploadFile request)
        {

            var imageFolder = $@"\uploaded\Avatar\images\{DateTime.Now.ToString("yyyyMMdd")}";

            string folder = _webHostEnvironment.WebRootPath + imageFolder;
            var randomfile = System.IO.Path.GetRandomFileName();
            var nameImage = $"{randomfile}.jpg";

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
            var responseimage = "https://localhost:7184" + "/" + pathimage;
            return responseimage;
        }

        public async Task<ResponseMessage> UpdateUser(UpdateUser request)
        {
            var find = await _userRepository.GetUserById(request.UserID);

            find.PassWord = request.PassWord;
            find.Email = request.Email;
            find.FullName = request.FullName;
            find.Gender = request.Gender;
            find.PhoneNumber = request.PhoneNumber;
            find.UpdateBy = request.UserName;
            find.ModifiedDate = DateTime.Now;

            if (request.UploadFile != null)
            {
                find.Avatar = await UploadImage(request.UploadFile);
            }

            var update = await _userRepository.UpdateUser(find);

            ResponseMessage responseMessage = new();
            if (update)
            {
                responseMessage.Message = "Cập nhật thông tin thành công";
                responseMessage.StatusCode = 200;
            }
            else
            {
                responseMessage.Message = "Cập nhật thông tin thất bại";
                responseMessage.StatusCode = 500;
            }
            return responseMessage;
        }

        public async Task<ResponseMessage> DeleteUser(Guid id)
        {
            ResponseMessage responseMessage = new();

            var exists = await _userRepository.UserExists(id);

            if (!exists)
            {
                responseMessage.Message = "Người dùng không tồn tại";
                responseMessage.StatusCode = 400;

                return responseMessage;
            }
            else
            {
                var userDto = await _userRepository.GetUserById(id);
                var user = _mapper.Map<User>(userDto);
                var delete = await _userRepository.DeleteUser(user);

                if (delete)
                {
                    responseMessage.Message = "Xóa người dùng thành công";
                    responseMessage.StatusCode = 200;
                }
                else
                {
                    responseMessage.Message = "Xóa người dùng thất bại";
                    responseMessage.StatusCode = 500;
                }
                return responseMessage;
            }
        }


    }
}
