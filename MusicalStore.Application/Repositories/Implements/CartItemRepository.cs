using MusicalStore.Application.Repositories.Interfaces;
using MusicalStore.Application.Repositories.RepositoryBase;
using MusicalStore.Data.EF;
using MusicalStore.Data.Entities;

namespace MusicalStore.Application.Repositories.Implements
{
    public class CartItemRepository : RepositoryBase<CartItem>, ICartItemRepository
    {
        private readonly DataContext _dataContext;

        public CartItemRepository(DataContext context) : base(context)
        {
            _dataContext = context;
        }

        public async Task<CartItem?> GetCartItemByID(Guid idCart, Guid idProduct)
        {
            return _dataContext.CartItems.Where(e => e.CartID == idCart && e.ProductID == idProduct).FirstOrDefault();
        }

        public async Task<int> DeleteCartItem(Guid idCart, Guid idProduct)
        {
            var cartItemToDelete = _dataContext.CartItems.Where(e => e.CartID == idCart && e.ProductID == idProduct).FirstOrDefault();
            if (cartItemToDelete != null)
            {
                _dataContext.CartItems.Remove(cartItemToDelete);
                return await _dataContext.SaveChangesAsync();
            }
            return 0;
        }

        public Task<int> DeleteCartItemByID(Guid idCart, Guid idProduct)
        {
            throw new NotImplementedException();
        }
    }
}
