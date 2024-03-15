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
        Task<List<UserDto>> GetAllUser();
        Task<UserDto> GetUserById(string id);
        Task<UserDto?> GetUserByUsername(string username);
        Task<UserDto?> GetUserByEmail(string email);
        Task<IList<string>> GetAllRoleByName(string UserName);
        Task<ResponseMessage> CreateUser(RegisterRequest request);
        Task<ResponseMessage> UpdateUser(UpdateUser request);
        Task<ResponseMessage> DeleteUser(string id);


    }
}
