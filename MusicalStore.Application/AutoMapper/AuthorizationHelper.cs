using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MusicalStore.Application.Services;
using MusicalStore.Common.ResponseBase;
using MusicalStore.Dtos.AppRole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Application.AutoMapper
{
    public static class AuthorizationHelper
    {
        public static async Task<ResponseMessage> CheckAccess(HttpContext httpContext, string functionName, string action)
        {
            var result = new ResponseMessage();
            var roleClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            if (roleClaim != null)
            {
                PermissionInput permissionInput = new()
                {
                    FunctionName = functionName,
                    Action = action,
                    Role = roleClaim.Value
                };

                var roleService = httpContext.RequestServices.GetRequiredService<IRoleService>();
                result = await roleService.CheckPermission(permissionInput);
            }
            return result;
        }
    }

}
