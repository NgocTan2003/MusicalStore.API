using MusicalStore.Common.ResponseBase;
using MusicalStore.Dtos.OrderDetails;
using MusicalStore.Dtos.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Application.Services.Interfaces
{
    public interface IOrderDetailService
    {
        Task<List<OrderDetailDto>> GetAllOrderDetail();
        Task<List<OrderDetailDto>> GetOrderDetailByOrder(Guid id);
        Task<OrderDetailDto> GetOrderDetailByID(Guid orderID, Guid productID);
        Task<ResponseMessage> CreateOrderDetail(CreateOrderDetail request);
        Task<ResponseMessage> UpdateOrderDetail(UpdateOrderDetail request);
        Task<ResponseMessage> DeleteOrderDetailById(Guid OrderID, Guid ProductID);
        Task<ResponseMessage> DeleteOrderDetailByOrder(Guid OrderID);

    }
}
