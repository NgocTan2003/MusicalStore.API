using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using MusicalStore.Application.Repositories.Implements;
using MusicalStore.Application.Repositories.Interfaces;
using MusicalStore.Application.Services.Interfaces;
using MusicalStore.Common.ResponseBase;
using MusicalStore.Data.Entities;
using MusicalStore.Dtos.CartItems;
using MusicalStore.Dtos.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Application.Services.Implements
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CartItemService(ICartItemRepository cartItemRepository, ICartRepository cartRepository, IProductRepository productRepository, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _productRepository = productRepository;
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<List<CartItemDto>> GetAllCartItem()
        {
            var cartItems = await _cartItemRepository.GetAll().ToListAsync();
            var cartItemsDto = _mapper.Map<List<CartItemDto>>(cartItems);
            return cartItemsDto;
        }

        public async Task<CartItemDto> GetCartItemById(Guid idCart, Guid idProduct)
        {
            var cartItem = await _cartItemRepository.GetCartItemByID(idCart, idProduct);
            var cartItemDto = _mapper.Map<CartItemDto>(cartItem);
            return cartItemDto;
        }

        public async Task<ResponseMessage> CreateCartItem(CreateCartItem request)
        {
            ResponseMessage responseMessage = new();
            var findCartItem = await _cartItemRepository.GetCartItemByID(request.CartID, request.ProductID);
            var findCart = await _cartRepository.FindById(request.CartID);
            var findProduct = await _productRepository.FindById(request.CartID);

            if (findCartItem != null)
            {
                responseMessage.Message = "Products already in the cart";
                responseMessage.StatusCode = 400;
            }
            else if (findCart == null || findProduct == null)
            {
                responseMessage.Message = "Cart or product does not exist";
                responseMessage.StatusCode = 400;
            }
            else
            {
                var cartItem = new CartItem();
                cartItem.CartID = request.CartID;
                cartItem.ProductID = request.ProductID;
                cartItem.Quantity = request.Quantity;
                cartItem.DateCreated = DateTime.Now;

                var create = await _cartItemRepository.Create(cartItem);
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
            }
            return responseMessage;
        }

        public async Task<ResponseMessage> UpdateCartItem(UpdateCartItem request)
        {
            var find = await _cartItemRepository.GetCartItemByID(request.CartID, request.ProductID);

            find.Quantity = request.Quantity;
            find.ModifiedDate = DateTime.Now;
            find.UpdateBy = request.UpdateBy;

            var update = await _cartItemRepository.Update(find);

            ResponseMessage responseMessage = new();
            if (update > 0)
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

        public async Task<ResponseMessage> DeleteCartItem(Guid idCart, Guid idProduct)
        {
            ResponseMessage responseMessage = new();

            var exists = await _cartItemRepository.GetCartItemByID(idCart, idProduct);

            if (exists != null)
            {
                responseMessage.Message = "Not Found";
                responseMessage.StatusCode = 400;

                return responseMessage;
            }
            else
            {
                var delete = await _cartItemRepository.DeleteCartItemByID(idCart, idProduct);

                if (delete > 0)
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
}
