using MusicalStore.Common.ResponseBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Application.Services.Interfaces
{
    public interface IBlacklistTokenService
    {
        Task<ResponseMessage> AddTokenToBlacklist(string accessToken, DateTime expirationTime);
        Task<bool> IsTokenBlacklisted(string accessToken);
        Task CleanUpExpiredTokens();
    }
}
