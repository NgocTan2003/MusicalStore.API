using MusicalStore.Common.ResponseBase;
using MusicalStore.Dtos.Galleries;
using MusicalStore.Dtos.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Application.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetAllOrder();
        Task<OrderDto> GetOrderById(Guid id);
        Task<ResponseMessage> CreateOrder(CreateOrder request);
        Task<ResponseMessage> UpdateOrder(UpdateOrder request);
        Task<ResponseMessage> DeleteOrder(Guid id);
    }
}
