using MusicalStore.Data.Entities;
using MusicalStore.Dtos.OrderDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MusicalStore.Application.Repositories.RepositoryBase.IRepositoryBase;

namespace MusicalStore.Application.Repositories.Interfaces
{
    public interface IOrderDetailRepository : IRepositoryBase<OrderDetail>
    {
        Task<List<OrderDetail>> GetOrderDetailByOrder(Guid id);
        Task<OrderDetail> GetOrderDetailById(Guid OrderID, Guid ProducID);
        Task<int> DeleteById(Guid OrderID, Guid ProducID);
        Task<int> DeleteByOrderID(Guid OrderID);
    }
}
