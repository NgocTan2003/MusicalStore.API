using Microsoft.EntityFrameworkCore;
using MusicalStore.Application.Repositories.Interfaces;
using MusicalStore.Data.EF;
using MusicalStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Application.Repositories.Implements
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;

        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<User>> GetAllUser()
        {
            var result = await _dataContext.Users.ToListAsync();
            return result;
        }

        public async Task<User?> GetUserById(Guid id)
        {
            return await _dataContext.Users.FindAsync(id);
        }

        public async Task<bool> UserExists(Guid id)
        {
            var result = await _dataContext.Users.FirstOrDefaultAsync(u => u.UserID == id);
            return result != null;
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            return _dataContext.Users.Where(e => e.UserName == username).FirstOrDefault();
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return _dataContext.Users.Where(e => e.Email == email).FirstOrDefault();
        }

        public async Task<Guid> CreateUser(User user)
        {
            _dataContext.Users.Add(user);
            await _dataContext.SaveChangesAsync();
            return user.UserID;
        }

        public async Task<bool> UpdateUser(User user)
        {
            bool isUpdate = false;

            _dataContext.Users.Update(user);
            int affectedRows = await _dataContext.SaveChangesAsync();

            if (affectedRows > 0)
            {
                isUpdate = true;
            }

            return isUpdate;
        }

        public async Task<bool> DeleteUser(User user)
        {
            bool isDeleted = false;

            _dataContext.Users.Remove(user);
            int affectedRows = await _dataContext.SaveChangesAsync();

            if (affectedRows > 0)
            {
                isDeleted = true;
            }

            return isDeleted;
        }


    }
}
