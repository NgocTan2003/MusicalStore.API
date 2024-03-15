using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MusicalStore.Application.Repositories.Implements;
using MusicalStore.Application.Repositories.Interfaces;
using MusicalStore.Application.Services.Interfaces;
using MusicalStore.Common.ResponseBase;
using MusicalStore.Data.Entities;
using MusicalStore.Dtos.Galleries;
using MusicalStore.Dtos.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Application.Services.Implements
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IUserRepository userRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<OrderDto>> GetAllOrder()
        {
            var orders = await _orderRepository.GetAll().ToListAsync();
            var orderDto = _mapper.Map<List<OrderDto>>(orders);
            return orderDto;
        }

        public async Task<OrderDto> GetOrderById(Guid id)
        {
            var order = await _orderRepository.FindById(id);
            var orderDto = _mapper.Map<OrderDto>(order);
            return orderDto;
        }

        public async Task<ResponseMessage> CreateOrder(CreateOrder request)
        {
            ResponseMessage responseMessage = new();

            var exist = await _userRepository.GetUserById(request.Id);

            if (exist == null)
            {
                responseMessage.StatusCode = 400;
                responseMessage.Message = "No user found";
            }
            else
            {
                Order order = new();
                order.OrderID = Guid.NewGuid();
                order.Id = request.Id;
                order.Receiver = request.Receiver;
                order.PhoneNumber = request.PhoneNumber;
                order.DeliveryAddress = request.DeliveryAddress;
                order.OrderStatus = request.OrderStatus;
                order.TotalMoney = request.TotalMoney;
                order.DateCreated = request.DateCreated;
                order.CreateBy = request.CreateBy;

                var create = await _orderRepository.Create(order);

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

        public async Task<ResponseMessage> UpdateOrder(UpdateOrder request)
        {
            ResponseMessage responseMessage = new();

            var order = await _orderRepository.FindById(request.OrderID);

            if (order == null)
            {
                responseMessage.StatusCode = 400;
                responseMessage.Message = "Not found Order";
            }
            else
            {
                order.Receiver = request.Receiver;
                order.PhoneNumber = request.PhoneNumber;
                order.DeliveryAddress = request.DeliveryAddress;
                order.TotalMoney = request.TotalMoney;
                order.ModifiedDate = DateTime.Now;
                order.UpdateBy = request.UpdateBy;

                var update = await _orderRepository.Update(order);

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

        public async Task<ResponseMessage> DeleteOrder(Guid id)
        {
            ResponseMessage responseMessage = new();

            var order = await _orderRepository.FindById(id);
            if (order == null)
            {
                responseMessage.StatusCode = 400;
                responseMessage.Message = "Not Found";
            }
            else
            {
                var delete = await _orderRepository.Delete(id);
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
    }
}
