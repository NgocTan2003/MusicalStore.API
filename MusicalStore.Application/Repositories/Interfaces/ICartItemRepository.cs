using MusicalStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MusicalStore.Application.Repositories.RepositoryBase.IRepositoryBase;

namespace MusicalStore.Application.Repositories.Interfaces
{
    public interface ICartItemRepository : IRepositoryBase<CartItem>
    {
        Task<CartItem?> GetCartItemByID(Guid idCart, Guid idProduct);
        Task<int> DeleteCartItemByID(Guid idCart, Guid idProduct);
    }
}
