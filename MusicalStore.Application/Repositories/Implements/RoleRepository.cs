﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MusicalStore.Application.Repositories.Interfaces;
using MusicalStore.Data.EF;
using MusicalStore.Dtos.AppRole;
using System.Data;

namespace MusicalStore.Application.Repositories.Implements
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DataContext _context;

        public RoleRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<IdentityRole>> GetAll()
        {
            var list = await _context.Roles.ToListAsync();
            return list;
        }

        public async Task<bool> CheckPermission(PermissionInput permissionInput)
        {
            CheckPermission request = await ChangePermission(permissionInput);

            var functions = await _context.Functions.ToListAsync();
            var permissions = await _context.Permissions.ToListAsync();
            var listroles = await this.GetAll();
            var query = from f in functions
                        join p in permissions on f.FunctionId equals p.FunctionId
                        join r in listroles on p.RoleName equals r.Name
                        where request.Role.Contains(r.Name) && f.FunctionId == request.FunctionId
                        && ((p.CanCreate && request.Action == "Create")
                        || (p.CanUpdate && request.Action == "Update")
                        || (p.CanDelete && request.Action == "Delete")
                        || (p.CanRead && request.Action == "Read"))
                        select p;
            var test = query.Any();
            return query.Any();
        }

        public async Task<CheckPermission> ChangePermission(PermissionInput request)
        {
            CheckPermission checkPermission = new CheckPermission();

            var function = await _context.Functions.FirstOrDefaultAsync(f => f.FunctionName == request.FunctionName);

            if (function != null)
            {
                checkPermission.FunctionId = function.FunctionId;
            }

            checkPermission.Action = request.Action;
            checkPermission.Role = request.Role;

            return checkPermission;
        }


    }
}
