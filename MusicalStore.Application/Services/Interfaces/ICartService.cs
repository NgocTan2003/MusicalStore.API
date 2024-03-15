using MusicalStore.Common.ResponseBase;
using MusicalStore.Dtos.Carts;
using MusicalStore.Dtos.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Application.Services.Interfaces
{
    public interface ICartService
    {
        Task<List<CartDto>> GetAllCart();
        Task<CartDto> GetCartById(Guid id);
        Task<ResponseMessage> CreateCart(string userID);

    }
}
