using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MusicalStore.Application.Repositories.Interfaces;
using MusicalStore.Application.Services.Interfaces;
using MusicalStore.Common.ResponseBase;
using MusicalStore.Data.Entities;
using MusicalStore.Dtos.Carts;

namespace MusicalStore.Application.Services.Implements
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public CartService(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<List<CartDto>> GetAllCart()
        {
            var carts = await _cartRepository.GetAll().ToListAsync();
            var cartsDto = _mapper.Map<List<CartDto>>(carts);
            return cartsDto;
        }

        public async Task<CartDto> GetCartById(Guid id)
        {
            var cart = await _cartRepository.FindById(id);
            var cartDto = _mapper.Map<CartDto>(cart);
            return cartDto;
        }

        public async Task<ResponseMessage> CreateCart(string userID)
        {
            ResponseMessage responseMessage = new();

            var cart = new Cart();
            cart.CartID = Guid.NewGuid();
            cart.Id = userID;
            cart.DateCreated = DateTime.Now;

            var create = await _cartRepository.Create(cart);
            if (create > 0)
            {
                responseMessage.Message = "Success";
                responseMessage.StatusCode = 200;
            }
            else
            {
                responseMessage.Message = "Fail";
                responseMessage.StatusCode = 500;
            }
            return responseMessage;
        }

    }
}
