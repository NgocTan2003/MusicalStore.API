using Microsoft.EntityFrameworkCore;
using MusicalStore.Application.Repositories.Interfaces;
using MusicalStore.Application.Repositories.RepositoryBase;
using MusicalStore.Data.EF;
using MusicalStore.Data.Entities;

namespace MusicalStore.Application.Repositories.Implements
{
    public class GalleryRepository : RepositoryBase<Gallery>, IGalleryRepository
    {
        private readonly DataContext _dataContext;

        public GalleryRepository(DataContext context) : base(context)
        {
            _dataContext = context;
        }

        public async Task<List<Gallery>> GetGalleryByProduct(Guid ProductId)
        {
            return await _dataContext.Galleries.Where(g => g.ProductID == ProductId).ToListAsync();
        }
    }
}
