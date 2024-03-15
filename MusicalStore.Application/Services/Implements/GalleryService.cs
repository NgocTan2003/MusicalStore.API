﻿using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using MusicalStore.Application.Repositories.Implements;
using MusicalStore.Application.Repositories.Interfaces;
using MusicalStore.Application.Services.Interfaces;
using MusicalStore.Common.Enums;
using MusicalStore.Common.ResponseBase;
using MusicalStore.Data.Entities;
using MusicalStore.Dtos.FeedBacks;
using MusicalStore.Dtos.Galleries;
using MusicalStore.Dtos.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Application.Services.Implements
{
    public class GalleryService : IGalleryService
    {
        private readonly IGalleryRepository _galleryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public GalleryService(IGalleryRepository galleryRepository, IProductRepository productRepository, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _galleryRepository = galleryRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<List<GalleryDto>> GetAllGallery()
        {
            var galleries = await _galleryRepository.GetAll().ToListAsync();
            var galleryDto = _mapper.Map<List<GalleryDto>>(galleries);
            return galleryDto;
        }

        public async Task<GalleryDto> GetGalleryById(Guid id)
        {
            var gallery = await _galleryRepository.FindById(id);
            var galleryDto = _mapper.Map<GalleryDto>(gallery);
            return galleryDto;
        }

        public async Task<List<GalleryDto>> GetGalleryByProduct(Guid ProductId)
        {
            var galleries = await _galleryRepository.GetGalleryByProduct(ProductId);
            var galleryDto = _mapper.Map<List<GalleryDto>>(galleries);
            return galleryDto;
        }

        public async Task<ResponseMessage> CreateGallery(CreateGallery request)
        {
            ResponseMessage responseMessage = new();

            var exist = await _productRepository.FindById(request.ProductID);

            if (exist == null)
            {
                responseMessage.StatusCode = 400;
                responseMessage.Message = "No product found";
            }
            else
            {
                Gallery gallery = new();
                gallery.ProductID = request.ProductID;
                if (request.UploadFile != null)
                {
                    gallery.Thumbnail = await UploadImage(request.UploadFile);
                }
                gallery.DateCreated = DateTime.Now;
                gallery.CreateBy = request.CreateBy;

                var create = await _galleryRepository.Create(gallery);

                if (create>0)
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


        public async Task<ResponseMessage> UpdateGallery(UpdateGallery request)
        {
            ResponseMessage responseMessage = new();
            var gallery = await _galleryRepository.FindById(request.GalleryID);
            var product = await _productRepository.FindById(request.ProductID);

            if (gallery == null || product == null)
            {
                responseMessage.StatusCode = 400;
                responseMessage.Message = "Not found Product or Gallery";
            }
            else
            {
                gallery.ProductID = request.ProductID;
                if (request.UploadFile != null)
                {
                    gallery.Thumbnail = await UploadImage(request.UploadFile);
                }
                gallery.ModifiedDate = DateTime.Now;
                gallery.UpdateBy = request.UpdateBy;

                var update = await _galleryRepository.Update(gallery);

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

        public async Task<ResponseMessage> DeleteGallery(Guid id)
        {
            ResponseMessage responseMessage = new();

            var gallery = await _galleryRepository.FindById(id);
            if (gallery == null)
            {
                responseMessage.StatusCode = 400;
                responseMessage.Message = "Not Found";
            }
            else
            {
                var delete = await _galleryRepository.Delete(id);
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

        public async Task<string> UploadImage(UploadFile request)
        {
            var imageFolder = $@"\uploaded\Gallery\images\{DateTime.Now.ToString("yyyyMMdd")}";

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
