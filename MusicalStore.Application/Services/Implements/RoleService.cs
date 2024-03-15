using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MusicalStore.Application.Repositories.Interfaces;
using MusicalStore.Common.ResponseBase;
using MusicalStore.Data.EF;
using MusicalStore.Data.Entities;
using MusicalStore.Data.Enums;
using MusicalStore.Dtos.AppRole;
using System.Data;
using System.Reflection;


namespace MusicalStore.Application.Services.Implements
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<List<string>> GetAll()
        {
            var list = await _roleRepository.GetAll();
            return list;
        }

        public async Task<ResponseMessage> CheckPermission(PermissionInput request)
        {
            ResponseMessage responseMessage = new();
            bool result = await _roleRepository.CheckPermission(request);
            if (result)
            {
                responseMessage.StatusCode = 200;
            }
            else
            {
                responseMessage.StatusCode = 403;
                responseMessage.Message = "Bạn không có quyền truy cập.";
            }
            return responseMessage;
        }
    }
}
