using Microsoft.AspNetCore.Identity;
using MusicalStore.Data.Entities;
using MusicalStore.Dtos.AppRole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Application.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<List<IdentityRole>> GetAll();
        Task<bool> CheckPermission(PermissionInput permissionInput);
        Task<CheckPermission> ChangePermission(PermissionInput request);
    }
}
