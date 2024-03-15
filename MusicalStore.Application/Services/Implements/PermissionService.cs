using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MusicalStore.Application.Repositories.Interfaces;
using MusicalStore.Application.Services.Interfaces;
using MusicalStore.Common.ResponseBase;
using MusicalStore.Dtos.Galleries;
using MusicalStore.Dtos.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Application.Services.Implements
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;

        public PermissionService(IPermissionRepository permissionRepository, IMapper mapper)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
        }

        public async Task<List<PermissionDto>> GetAllPermission()
        {
            var permissions = await _permissionRepository.GetAll().ToListAsync();
            var permissionDto = _mapper.Map<List<PermissionDto>>(permissions);
            return permissionDto;
        }

        public async Task<ResponseMessage> UpdatePermission(UpdatePermission request)
        {
            ResponseMessage responseMessage = new();

            var permission = await _permissionRepository.FindById(request.PermissionId);
            if (permission == null)
            {
                responseMessage.StatusCode = 400;
                responseMessage.Message = "Not Found";
            }
            else
            {
                permission.CanRead = request.CanRead;
                permission.CanCreate = request.CanCreate;
                permission.CanUpdate = request.CanUpdate;
                permission.CanDelete = request.CanDelete;

                var update = await _permissionRepository.Update(permission);
                if (update > 0)
                {
                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Success";
                }
                else
                {
                    responseMessage.StatusCode = 400;
                    responseMessage.Message = "Fail";
                }
            }
            return responseMessage;
        }
    }
}
