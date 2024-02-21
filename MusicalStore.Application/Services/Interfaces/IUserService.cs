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
        Task<List<UserDto>> GetAllUser();
        Task<UserDto> GetUserById(Guid id);
        Task<bool> UserExists(Guid id);
        Task<User?> GetUserByUsername(string username);
        Task<User?> GetUserByEmail(string email);
        Task<ResponseMessage> CreateUser(RegisterRequest request);
        Task<ResponseMessage> UpdateUser(UpdateUser request);
        Task<ResponseMessage> DeleteUser(Guid id);
    }
}
