using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MusicalStore.Application.Services.Interfaces;
using MusicalStore.Data.EF;
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
    private readonly DataContext _context;
    public TokenService(DataContext context, IConfiguration config)
    {
        _config = config;
        _context = context;
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
                expires: DateTime.Now.AddMinutes(10),
                claims: claims,
                signingCredentials: creds
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        throw new NotImplementedException();
    }
}
