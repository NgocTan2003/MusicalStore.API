using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MusicalStore.Application.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Application.Services.Implements
{
    public class CleanupExpiredTokensService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public CleanupExpiredTokensService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var blacklistService = scope.ServiceProvider.GetRequiredService<IBlacklistTokenRepository>();
                    await blacklistService.CleanUpExpiredTokens();
                }

                // Chạy định kỳ mỗi 1 giờ
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }

}
