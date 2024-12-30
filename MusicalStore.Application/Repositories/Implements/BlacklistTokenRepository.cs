using Microsoft.EntityFrameworkCore;
using MusicalStore.Application.Repositories.Interfaces;
using MusicalStore.Application.Repositories.RepositoryBase;
using MusicalStore.Data.EF;
using MusicalStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Application.Repositories.Implements
{
    public class BlacklistTokenRepository : RepositoryBase<BlackListToken>, IBlacklistTokenRepository
    {
        private readonly DataContext _dataContext;

        public BlacklistTokenRepository(DataContext context) : base(context)
        {
            _dataContext = context;
        }

        public async Task<bool> IsTokenBlacklisted(string accessToken)
        {
            var exist = await _dataContext.BlackLists.AnyAsync(bt => bt.Token == accessToken);
            return exist;
        }

        public async Task CleanUpExpiredTokens()
        {
            var expiredTokens = _dataContext.BlackLists.Where(bt => bt.ExpirationTime < DateTime.UtcNow);
            _dataContext.BlackLists.RemoveRange(expiredTokens);
            await _dataContext.SaveChangesAsync();
        }
    }
}
