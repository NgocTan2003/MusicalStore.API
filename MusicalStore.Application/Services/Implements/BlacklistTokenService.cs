using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MusicalStore.Application.Repositories.Interfaces;
using MusicalStore.Application.Services.Interfaces;
using MusicalStore.Common.ResponseBase;
using MusicalStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Application.Services.Implements
{
    public class BlacklistTokenService : IBlacklistTokenService
    {
        public IBlacklistTokenRepository _blacklistTokenRepository;
        private readonly IConfiguration _config;

        public BlacklistTokenService(IBlacklistTokenRepository blacklistTokenRepository, IConfiguration config)
        {
            _blacklistTokenRepository = blacklistTokenRepository;
            _config = config;
        }

        public async Task<ResponseMessage> AddTokenToBlacklist(string accessToken, DateTime expirationTime)
        {
            ResponseMessage responseMessage = new();
           // var (principal, expirationTime) = GetPrincipalFromExpiredToken(accessToken);

            var blacklistToken = new BlackListToken
            {
                Token = accessToken,
                ExpirationTime = expirationTime
            };

            var create = await _blacklistTokenRepository.Create(blacklistToken);
            if (create > 0)
            {
                responseMessage.Message = "Success";
                responseMessage.StatusCode = StatusCodes.Status200OK;
            }
            else
            {
                responseMessage.Message = "Fail";
                responseMessage.StatusCode = StatusCodes.Status500InternalServerError;
            }
            return responseMessage;
        }

        public async Task<bool> IsTokenBlacklisted(string token)
        {
            return await _blacklistTokenRepository.IsTokenBlacklisted(token);
        }

        public async Task CleanUpExpiredTokens()
        {
            await _blacklistTokenRepository.CleanUpExpiredTokens();
        }
    }
}
