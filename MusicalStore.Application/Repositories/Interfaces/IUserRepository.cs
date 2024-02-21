using MusicalStore.Application.Repositories.RepositoryBase;
using MusicalStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MusicalStore.Application.Repositories.RepositoryBase.IRepositoryBase;

namespace MusicalStore.Application.Repositories.Interfaces
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<User?> GetUserByUsername(string username);
        Task<User?> GetUserByEmail(string email);

        //Task<bool> UserExists(Guid id);
        //Task<List<User>> GetAllUser();
        //Task<User> GetUserById(Guid id);
        //Task<Guid> CreateUser(User user);
        //Task<bool> UpdateUser(User user);
        //Task<bool> DeleteUser(User user);
    }
}
