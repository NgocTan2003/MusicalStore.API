using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MusicalStore.Application.Repositories.Interfaces;
using MusicalStore.Application.Services.Interfaces;
using MusicalStore.Data.EF;
using MusicalStore.Data.Entities;
using MusicalStore.Dtos.Users;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Application.Services.Implements;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly UserManager<AppUser> _userManager;
    private readonly DataContext _context;
    private readonly IUserRepository _userRepository;
    public TokenService(DataContext context, IConfiguration config, UserManager<AppUser> userManager, IUserRepository userRepository)
    {
        _context = context;
        _config = config;
        _userManager = userManager;
        _userRepository = userRepository;
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

        var token = new JwtSecurityToken(
                issuer: _config["JWT:ValidIssuer"],
                audience: _config["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(2),
                claims: claims,
                signingCredentials: creds
            );

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
            ValidAudience = _config["JWT:ValidAudience"],
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
            //var claimUserLogin = GetClaimUserLoginFromPrincipal(principal);
            //var username = claimUserLogin.UserName;
            //result.UserName = principal.Identity.Name;
            //if (principal == null)
            //{
            //    return result;
            //}
            //var user = await _userRepository.GetUserByUsername(username);
            //var roles = await _userRepository.GetAllRoleByName(username);
            //if (user == null)
            //{
            //    result.Message = "No user found";
            //    return result;
            //}
            //else if (user.RefeshToken != refreshToken)
            //{
            //    result.Message = "The Refeshtoken code is not correct";
            //    return result;
            //}
            //else if (user.ExpiryTime <= DateTime.Now)
            //{
            //    result.Message = "Token has not expired";
            //    return result;
            //}

            //var utcExpireDate = long.Parse(principal.Claims.FirstOrDefault(x => x.Type == "exp").Value);
            //var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);
            //var dateNow = DateTime.Now;
            //// nếu token chưa hết hạn thì return về token cũ 
            //if (expireDate > dateNow)
            //{
            //    result.UserName = user.UserName;
            //    result.AccessToken = tokenApiModel.AccessToken;
            //    result.RefeshToken = tokenApiModel.RefreshToken;
            //    return result;
            //}

            //// nếu token đã hết hạn
            //var request = new ClaimUserLogin()
            //{
            //    UserName = user.UserName,
            //    Email = user.UserName,
            //    Roles = roles
            //};
            //var newAccessToken = GenerateAccessToken(request);
            //var newRefreshToken = GenerateRefreshToken();
            //user.RefeshToken = newRefreshToken;
            //_context.SaveChanges();

            //result.UserName = username;
            //result.AccessToken = newAccessToken;
            //result.RefeshToken = newRefreshToken;
            //result.StatusCode = 200;
            return result;
        }
        catch(Exception ex)
        {
            result.Message = ex.Message;
            return result;
        }
    }

    public DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(utcExpireDate).ToLocalTime();
        return dateTime;
    }
}
