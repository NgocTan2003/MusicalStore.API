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

        public async Task<List<User>> GetAll()
        {
            var result = new List<User>();
            try
            {
                result = await _dataContext.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗii");
            }
            return result;

        }

        public async Task<User?> GetById(Guid id)
        {
            return await _dataContext.Users.FindAsync(id);
        }

        public async Task<User?> GetByUsername(string username)
        {
            return _dataContext.Users.Where(e => e.UserName == username).FirstOrDefault();
        }

        public async Task<User?> GetByEmail(string email)
        {
            return _dataContext.Users.Where(e => e.Email == email).FirstOrDefault();
        }

        public async Task<Guid> Create(User user)
        {
            _dataContext.Users.Add(user);
            await _dataContext.SaveChangesAsync();
            return user.UserID;
        }

        public Task<int> Update(User user)
        {
            throw new NotImplementedException();
        }

        public Task<int> Delete(User user)
        {
            throw new NotImplementedException();
        }
    }
}
