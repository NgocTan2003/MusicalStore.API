using MusicalStore.Application.Repositories.Interfaces;
using MusicalStore.Application.Repositories.RepositoryBase;
using MusicalStore.Data.EF;
using MusicalStore.Data.Entities;

namespace MusicalStore.Application.Repositories.Implements
{
    public class CartRepository : RepositoryBase<Cart>, ICartRepository
    {
        public CartRepository(DataContext context) : base(context)
        {

        }
    }
}
