using Microsoft.AspNetCore.Identity;
using MusicalStore.Common.ResponseBase;
using MusicalStore.Data.Entities;
using MusicalStore.Dtos.AppRole;

namespace MusicalStore.Application.Services
{
    public interface IRoleService
    {
        Task<List<IdentityRole>> GetAll();
        Task<ResponseMessage> CheckPermission(PermissionInput request);
    }
}
