using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using MusicalStore.Application.Repositories.Implements;
using MusicalStore.Application.Repositories.Interfaces;
using MusicalStore.Application.Services.Interfaces;
using MusicalStore.Common.Enums;
using MusicalStore.Common.ResponseBase;
using MusicalStore.Data.Entities;
using MusicalStore.Dtos.Products;
using MusicalStore.Dtos.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Application.Services.Implements
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<List<ProductDto>> GetAllProduct()
        {
            var products = await _productRepository.GetAll().ToListAsync();
            var productsDto = _mapper.Map<List<ProductDto>>(products);
            return productsDto;
        }

        public async Task<bool> ProductExists(Guid id)
        {
            return await _productRepository.Exists(id);
        }

        public async Task<ProductDto> GetProductById(Guid id)
        {
            var products = await _productRepository.FindById(id);
            var productsDto = _mapper.Map<ProductDto>(products);
            return productsDto;
        }
        public async Task<ResponseMessage> CreateProduct(CreateProduct request)
        {
            ResponseMessage responseMessage = new();
            var findCategory = await _categoryRepository.FindById(request.CategoryID);

            if (findCategory == null)
            {
                responseMessage.Message = "Not Found Category";
                responseMessage.StatusCode = 400;
            }
            else
            {
                var product = new Product();
                product.ProductID = Guid.NewGuid();
                product.CategoryID = request.CategoryID;
                product.ProductName = request.ProductName;
                product.PriceOld = request.PriceOld;
                product.PriceNew = request.PriceNew;
                if (request.uploadFile != null)
                {
                    product.Thumbnail = await UploadImage(request.uploadFile);
                }
                product.Description = request.Description;
                product.Quantity = request.Quantity;
                product.DateCreated = DateTime.Now;
                product.CreateBy = "Admin";

                var create = await _productRepository.Create(product);
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

        public async Task<ResponseMessage> UpdateProduct(UpdateProduct request)
        {
            ResponseMessage responseMessage = new();

            var find = await _productRepository.FindById(request.ProductID);
            var findCategory = await _categoryRepository.FindById(request.CategoryID);
            if (findCategory != null)
            {
                find.ProductName = request.ProductName;
                find.PriceOld = request.PriceOld;
                find.PriceNew = request.PriceNew;
                if (request.uploadFile != null)
                {
                    find.Thumbnail = await UploadImage(request.uploadFile);
                }
                find.Description = request.Description;
                find.Quantity = request.Quantity;
                find.ModifiedDate = DateTime.Now;
                find.CreateBy = request.UpdateBy;

                var update = await _productRepository.Update(find);

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
            else
            {
                responseMessage.Message = "Not Found Category";
                responseMessage.StatusCode = 500;
                return responseMessage;
            }
        }

        public async Task<ResponseMessage> DeleteProduct(Guid id)
        {
            ResponseMessage responseMessage = new();

            var exists = await _productRepository.Exists(id);

            if (!exists)
            {
                responseMessage.Message = "Not Found";
                responseMessage.StatusCode = 400;

                return responseMessage;
            }
            else
            {
                var delete = await _productRepository.Delete(id);

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

        public async Task<string> UploadImage(UploadFile request)
        {

            var imageFolder = $@"\uploaded\Product\images\{DateTime.Now.ToString("yyyyMMdd")}";

            string folder = _webHostEnvironment.WebRootPath + imageFolder;

            var nameImage = $"{request.FileName}.jpg";

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string filePath = Path.Combine(folder, nameImage);

            using (FileStream fs = System.IO.File.Create(filePath))
            {
                fs.Write(request.Bytes, 0, request.Bytes.Length);
                fs.Flush();
            }
            var pathimage = Path.Combine(imageFolder, nameImage).Replace(@"\", @"/");
            var responseimage = "https://localhost:7099" + "/" + pathimage;
            return responseimage;
        }
    }
}
