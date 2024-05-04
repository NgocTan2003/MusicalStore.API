using Microsoft.AspNetCore.Http;
using MusicalStore.Common.ResponseBase;
using MusicalStore.Data.Entities;
using MusicalStore.Dtos.Products;
using MusicalStore.Dtos.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Application.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllProduct();
        Task<List<ProductDto>> GetPaginationProduct(int page);
        Task<ProductDto> GetProductById(Guid id);
        Task<bool> ProductExists(Guid id);
        Task<ResponseMessage> CreateProduct(CreateProduct request);
        Task<ResponseMessage> UpdateProduct(UpdateProduct request);
        Task<ResponseMessage> UpdateThumbnailProduct(Guid Id, IFormFile file, string bucketName, string namefile);
        Task<ResponseMessage> DeleteProduct(Guid id);
    }
}
