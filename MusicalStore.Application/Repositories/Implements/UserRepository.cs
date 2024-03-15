using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MusicalStore.Application.Repositories.Interfaces;
using MusicalStore.Application.Repositories.RepositoryBase;
using MusicalStore.Data.EF;
using MusicalStore.Data.Entities;
using MusicalStore.Dtos.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MusicalStore.Application.Repositories.RepositoryBase.IRepositoryBase;

namespace MusicalStore.Application.Repositories.Implements
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<AppUser> _userManager;

        public UserRepository(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AppUser?> GetUserByUsername(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<AppUser?> GetUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<List<AppUser>> GetAllUser()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<AppUser> GetUserById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<IList<string>> GetAllRoleByName(string UserName)
        {
            var user = await _userManager.FindByNameAsync(UserName);
            var listrole = await _userManager.GetRolesAsync(user);
            return listrole;
        }

        public async Task<bool> AddRole(AppUser user, IList<string> Roles)
        {
            var createrole = await _userManager.AddToRolesAsync(user, Roles);
            if (createrole.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CreateUser(AppUser user, string password)
        {
            var create = await _userManager.CreateAsync(user, password);
            if (create.Succeeded)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateUser(AppUser user)
        {
            var update = await _userManager.UpdateAsync(user);
            if (update.Succeeded)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteUser(AppUser user)
        {
            var delete = await _userManager.DeleteAsync(user);
            if (delete.Succeeded)
            {
                return true;
            }
            return false;
        }

       
    }
}
