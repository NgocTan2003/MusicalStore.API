using MusicalStore.Common.ResponseBase;
using MusicalStore.Dtos.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Application.Services.Interfaces
{
    public interface IPermissionService
    {
        Task<List<PermissionDto>> GetAllPermission();
        Task<ResponseMessage> UpdatePermission(UpdatePermission request);
    }
}
