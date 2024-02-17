using MusicalStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Application.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAll();
        Task<User> GetById(Guid id);
        Task<User> GetByUsername(string username);
        Task<User> GetByEmail(string email);
        Task<Guid> Create(User user);
        Task<int> Update(User user);
        Task<int> Delete(User user);
    }
}
