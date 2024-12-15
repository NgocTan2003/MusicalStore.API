using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MusicalStore.Application.Services.Interfaces;
using MusicalStore.Data.EF;
using MusicalStore.Data.Entities;
using MusicalStore.Dtos.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MusicalStore.Application.Services.Implements;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly UserManager<AppUser> _userManager;
    private readonly DataContext _context;
    public TokenService(DataContext context, IConfiguration config, UserManager<AppUser> userManager)
    {
        _context = context;
        _config = config;
        _userManager = userManager;
    }

    public string GenerateAccessToken(ClaimUserLogin request)
    {
        IList<string> roles;
        var claims = new[]
        {
            new Claim(ClaimTypes.Email,request.Email),
            new Claim(ClaimTypes.Role, string.Join(";",request.Roles)),
            new Claim(ClaimTypes.Name, request.UserName)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var time = int.TryParse(_config["JWT:TokenExpirationTime"], out int tokenExpirationTime);

        var token = new JwtSecurityToken(_config["JWT:ValidIssuer"],
           _config["JWT:ValidIssuer"],
           claims,
           expires: DateTime.Now.AddMinutes(tokenExpirationTime),
           signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
    {
        var token = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _config["JWT:ValidIssuer"],
            ValidAudience = _config["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"])),
            ValidateLifetime = true
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(accessToken, token, out SecurityToken securityToken);
        return principal;
    }

    public async Task<TokenResponse> RefreshToken(TokenApiModel tokenApiModel)
    {
        var result = new TokenResponse();
        try
        {
            string accessToken = tokenApiModel.AccessToken;
            string refreshToken = tokenApiModel.RefreshToken;
            var principal = GetPrincipalFromExpiredToken(accessToken);
            var username = principal.Identity.Name;

            var user = await _userManager.FindByNameAsync(username);
            var roles = await _userManager.GetRolesAsync(user);
            if (user == null)
            {
                result.Message = "No user found";
                return result;
            }
            else if (user.RefreshToken != refreshToken)
            {
                result.Message = "The Refeshtoken code is not correct";
                return result;
            }
            else if (user.RefreshTokenExpirationTime <= DateTime.Now)
            {
                result.Message = "Refreshtoken has expired";
                return result;
            }
            else if (user.TokenExpirationTime >= DateTime.Now)
            {
                result.Message = "Token has not expired";
                result.UserName = user.UserName;
                result.AccessToken = tokenApiModel.AccessToken;
                result.RefreshToken = tokenApiModel.RefreshToken;
                return result;
            }

            // nếu token đã hết hạn
            var request = new ClaimUserLogin()
            {
                UserName = user.UserName,
                Email = user.UserName,
                Roles = roles
            };
            var tokenTime = int.TryParse(_config["JWT:TokenExpirationTime"], out int tokenExpirationTime);
            var refreshtoken= int.TryParse(_config["JWT:RefreshTokenExpirationTime"], out int refreshtokenExpirationTime);
            var newAccessToken = GenerateAccessToken(request);
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.TokenExpirationTime = DateTime.Now.AddMinutes(tokenExpirationTime);
            user.RefreshTokenExpirationTime = DateTime.Now.AddMinutes(refreshtokenExpirationTime);
            _context.SaveChanges();
            await _userManager.UpdateAsync(user);

            result.UserName = username;
            result.AccessToken = newAccessToken;
            result.RefreshToken = newRefreshToken;
            result.TokenExpiration = DateTime.Now.AddMinutes(tokenExpirationTime);
            result.RefreshTokenExpiration = DateTime.Now.AddMinutes(refreshtokenExpirationTime);
            result.StatusCode = 200;
            return result;
        }
        catch (Exception ex)
        {
            result.Message = ex.Message;
            return result;
        }
    }
}
