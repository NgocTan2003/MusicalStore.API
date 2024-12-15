using Microsoft.EntityFrameworkCore;
using MusicalStore.Application.Repositories.Interfaces;
using MusicalStore.Application.Repositories.RepositoryBase;
using MusicalStore.Data.EF;
using MusicalStore.Data.Entities;

namespace MusicalStore.Application.Repositories.Implements
{
    public class FeedBackRepository : RepositoryBase<FeedBack>, IFeedBackRepository
    {
        private readonly DataContext _dataContext;

        public FeedBackRepository(DataContext context) : base(context)
        {
            _dataContext = context;
        }

        public async Task<FeedBack?> GetFeedBackByProductAndUser(Guid ProductId, string UserId)
        {
            var feedback = await _dataContext.FeedBacks.Where(f => f.ProductID == ProductId && f.Id == UserId).FirstOrDefaultAsync();
            return feedback;
        }
    }
}
