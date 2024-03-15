using MusicalStore.Application.Repositories.RepositoryBase;
using MusicalStore.Data.Entities;
using MusicalStore.Dtos.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MusicalStore.Application.Repositories.RepositoryBase.IRepositoryBase;

namespace MusicalStore.Application.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<List<AppUser>> GetAllUser();
        Task<AppUser> GetUserById(string id);
        Task<AppUser?> GetUserByUsername(string username);
        Task<AppUser?> GetUserByEmail(string email);
        Task<bool> AddRole(AppUser user, IList<string> Roles);
        Task<IList<string>> GetAllRoleByName(string UserName);
        Task<bool> CreateUser(AppUser user, string password);
        Task<bool> UpdateUser(AppUser user);
        Task<bool> DeleteUser(AppUser user);
    }
}
