using AutoMapper;
using MusicalStore.Data.Entities;
using MusicalStore.Dtos.AppRole;
using MusicalStore.Dtos.CartItems;
using MusicalStore.Dtos.Carts;
using MusicalStore.Dtos.Categories;
using MusicalStore.Dtos.FeedBacks;
using MusicalStore.Dtos.Galleries;
using MusicalStore.Dtos.OrderDetails;
using MusicalStore.Dtos.Orders;
using MusicalStore.Dtos.Permissions;
using MusicalStore.Dtos.Products;
using MusicalStore.Dtos.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Application.AutoMapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<AppUser, UserDto>().ReverseMap();
            CreateMap<Permission, PermissionDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailDto>().ReverseMap();
            CreateMap<Gallery, GalleryDto>().ReverseMap();
            CreateMap<FeedBack, FeedBackDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Cart, CartDto>().ReverseMap();
            CreateMap<CartItem, CartItemDto>().ReverseMap();

        }
    }
}
