using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MusicalStore.Application.Repositories.Interfaces;
using MusicalStore.Application.Repositories.RepositoryBase;
using MusicalStore.Data.EF;
using MusicalStore.Data.Entities;
using MusicalStore.Dtos.OrderDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Application.Repositories.Implements
{
    public class OrderDetailRepository : RepositoryBase<OrderDetail>, IOrderDetailRepository
    {
        private readonly DataContext _dataContext;
        public OrderDetailRepository(DataContext context) : base(context)
        {
            _dataContext = context;
        }

        public async Task<OrderDetail> GetOrderDetailById(Guid OrderID, Guid ProducID)
        {
            var result = await _dataContext.OrderDetails.Where(o => o.OrderID == OrderID && o.ProductID == ProducID).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<OrderDetail>> GetOrderDetailByOrder(Guid id)
        {
            var result = await _dataContext.OrderDetails.Where(o => o.OrderID == id).ToListAsync();
            return result;
        }

        public async Task<int> DeleteById(Guid OrderID, Guid ProducID)
        {
            var find = await GetOrderDetailById(OrderID, ProducID);
            if (find is not null)
            {
                _dataContext.OrderDetails.Remove(find);
                return await _dataContext.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<int> DeleteByOrderID(Guid OrderID)
        {
            var find = await GetOrderDetailByOrder(OrderID);
            if (find is not null)
            {
                _dataContext.OrderDetails.RemoveRange(find);
                return await _dataContext.SaveChangesAsync();
            }
            return 0;
        }
    }
}
