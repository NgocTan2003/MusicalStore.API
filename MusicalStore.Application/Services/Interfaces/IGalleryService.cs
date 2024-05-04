using Microsoft.AspNetCore.Http;
using MusicalStore.Common.ResponseBase;
using MusicalStore.Dtos.FeedBacks;
using MusicalStore.Dtos.Galleries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Application.Services.Interfaces
{
    public interface IGalleryService
    {
        Task<List<GalleryDto>> GetAllGallery();
        Task<GalleryDto> GetGalleryById(Guid id);
        Task<List<GalleryDto>> GetGalleryByProduct(Guid ProductId);
        Task<ResponseMessage> CreateGallery(CreateGallery request);
        Task<ResponseMessage> UpdateGallery(UpdateGallery request);
        Task<ResponseMessage> UpdateThumbnailGallery(Guid Id, IFormFile file, string bucketName, string namefile);
        Task<ResponseMessage> DeleteGallery(Guid id);
    }
}
