using Microsoft.IdentityModel.Tokens;
using MusicalStore.Dtos.Users;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
        Task<TokenResponse> RefreshToken(TokenApiModel tokenApiModel);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        DateTime ConvertUnixTimeToDateTime(long utcExpireDate);
    }
}
