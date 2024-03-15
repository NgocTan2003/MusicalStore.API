using MusicalStore.Common.ResponseBase;
using MusicalStore.Data.Entities;
using MusicalStore.Dtos.AppRole;

namespace MusicalStore.Application.Services
{
    public interface IRoleService
    {
        Task<List<string>> GetAll();
        Task<ResponseMessage> CheckPermission(PermissionInput request);
    }
}
