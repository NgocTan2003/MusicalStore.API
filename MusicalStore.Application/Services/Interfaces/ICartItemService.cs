using MusicalStore.Common.ResponseBase;
using MusicalStore.Dtos.CartItems;
using MusicalStore.Dtos.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Application.Services.Interfaces
{
    public interface ICartItemService
    {
        Task<List<CartItemDto>> GetAllCartItem();
        Task<CartItemDto> GetCartItemById(Guid idCart, Guid idProduct);
        Task<ResponseMessage> CreateCartItem(CreateCartItem request);
        Task<ResponseMessage> UpdateCartItem(UpdateCartItem request);
        Task<ResponseMessage> DeleteCartItem(Guid idCart, Guid idProduct);
    }
}
