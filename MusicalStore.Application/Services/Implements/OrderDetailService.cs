using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MusicalStore.Application.Repositories.Implements;
using MusicalStore.Application.Repositories.Interfaces;
using MusicalStore.Application.Services.Interfaces;
using MusicalStore.Common.ResponseBase;
using MusicalStore.Data.Entities;
using MusicalStore.Dtos.OrderDetails;
using MusicalStore.Dtos.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Application.Services.Implements
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public OrderDetailService(IOrderDetailRepository orderDetailRepository, IOrderRepository orderRepository, IProductRepository productRepository, IMapper mapper)
        {
            _orderDetailRepository = orderDetailRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<List<OrderDetailDto>> GetAllOrderDetail()
        {
            var orderDetails = await _orderDetailRepository.GetAll().ToListAsync();
            var orderDetailDto = _mapper.Map<List<OrderDetailDto>>(orderDetails);
            return orderDetailDto;
        }

        public async Task<List<OrderDetailDto>> GetOrderDetailByOrder(Guid id)
        {
            var orderDetail = await _orderDetailRepository.GetOrderDetailByOrder(id);
            var orderDetailDto = _mapper.Map<List<OrderDetailDto>>(orderDetail);
            return orderDetailDto;
        }

        public async Task<OrderDetailDto> GetOrderDetailByID(Guid orderID, Guid productID)
        {
            var orderDetail = await _orderDetailRepository.GetOrderDetailById(orderID, productID);
            var orderDetailDto = _mapper.Map<OrderDetailDto>(orderDetail);
            return orderDetailDto;
        }

        public async Task<ResponseMessage> CreateOrderDetail(CreateOrderDetail request)
        {
            ResponseMessage responseMessage = new();

            var order = await _orderRepository.FindById(request.OrderID);
            var product = await _productRepository.FindById(request.ProductID);

            if (order == null || product == null)
            {
                responseMessage.StatusCode = 400;
                responseMessage.Message = "No order or product are found";
            }
            else
            {
                OrderDetail orderDetail = new()
                {
                    OrderID = request.OrderID,
                    ProductID = request.ProductID,
                    UnitPrice = request.UnitPrice,
                    Quantity = request.Quantity,
                    TotalDetails = request.TotalDetails,
                    DateCreated = DateTime.Now,
                    CreateBy = request.CreateBy
                };

                var create = await _orderDetailRepository.Create(orderDetail);

                if (create > 0)
                {
                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Success";
                }
                else
                {
                    responseMessage.StatusCode = 400;
                    responseMessage.Message = "Fail";
                }
            }
            return responseMessage;
        }

        public async Task<ResponseMessage> UpdateOrderDetail(UpdateOrderDetail request)
        {
            ResponseMessage responseMessage = new();

            var orderDetail = await _orderDetailRepository.GetOrderDetailById(request.OrderID, request.ProductID);

            if (orderDetail == null)
            {
                responseMessage.StatusCode = 400;
                responseMessage.Message = "Not found OrderDetail";
            }
            else
            {
                orderDetail.Quantity = request.Quantity;
                orderDetail.UnitPrice = request.UnitPrice;
                orderDetail.TotalDetails = request.TotalDetails;
                orderDetail.ModifiedDate = DateTime.Now;
                orderDetail.UpdateBy = request.UpdateBy;

                var update = await _orderDetailRepository.Update(orderDetail);

                if (update > 0)
                {
                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Success";
                }
                else
                {
                    responseMessage.StatusCode = 400;
                    responseMessage.Message = "Fail";
                }
            }

            return responseMessage;
        }

        public async Task<ResponseMessage> DeleteOrderDetailById(Guid OrderID, Guid ProductID)
        {
            ResponseMessage responseMessage = new();

            var orderDetail = await _orderDetailRepository.GetOrderDetailById(OrderID, ProductID);
            if (orderDetail == null)
            {
                responseMessage.StatusCode = 400;
                responseMessage.Message = "Not Found";
            }
            else
            {
                var delete = await _orderDetailRepository.DeleteById(OrderID, ProductID);
                if (delete > 0)
                {
                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Success";
                }
                else
                {
                    responseMessage.StatusCode = 400;
                    responseMessage.Message = "Fail";
                }
            }

            return responseMessage;
        }

        public async Task<ResponseMessage> DeleteOrderDetailByOrder(Guid OrderID)
        {
            ResponseMessage responseMessage = new();

            var orderDetail = await _orderDetailRepository.GetOrderDetailByOrder(OrderID);
            if (orderDetail == null)
            {
                responseMessage.StatusCode = 400;
                responseMessage.Message = "Not Found";
            }
            else
            {
                var delete = await _orderDetailRepository.DeleteByOrderID(OrderID);
                if (delete > 0)
                {
                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Success";
                }
                else
                {
                    responseMessage.StatusCode = 400;
                    responseMessage.Message = "Fail";
                }
            }

            return responseMessage;
        }

        public async Task<List<OrderDetailDto>> GetPaginationOrderDetail(int page)
        {
            var orderDetails = await _orderDetailRepository.Pagination(page);
            var orderDetailsDto = _mapper.Map<List<OrderDetailDto>>(orderDetails);
            return orderDetailsDto;
        }
    }

}
