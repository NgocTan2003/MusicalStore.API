using MusicalStore.Dtos.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Application.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(ClaimUserLogin request);
        string GenerateRefreshToken();
    }
}
