using MusicalStore.Application.AutoConfiguration;
using MusicalStore.Common.ResponseBase;
using MusicalStore.Data.Entities;
using MusicalStore.Dtos.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<TokenResponse> Authentication(AuthenticationRequest request);
        Task<TokenResponse> AuthenticationOTP(string code, string username);
        Task<ResponseMessage> SendEmailOTP(string username, string password);
        Task<ResponseMessage> ForgotPassword(string EmailForgotPassword);
        Task<ResponseMessage> ResetPassword(ResetPassword resetPassword);
        Task<ResponseMessage> ChangePassword(ChangePassword changePassword);
        Task<List<UserDto>> GetAllUser();
        Task<UserDto> GetUserById(string id);
        Task<UserDto?> GetUserByUsername(string username);
        Task<UserDto?> GetUserByEmail(string email);
        Task<IList<string>> GetAllRoleByName(string UserName);
        Task<ResponseCreateUser> CreateUser(RegisterRequest request);
        Task<ResponseMessage> UpdateUser(UpdateUser request);
        Task<ResponseMessage> DeleteUser(string id);
    }
}
