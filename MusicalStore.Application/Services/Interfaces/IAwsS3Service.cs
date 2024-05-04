using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicalStore.Common.Enums;
using MusicalStore.Common.ResponseBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Application.Services.Interfaces
{
    public interface IAwsS3Service
    {
        Task<S3Response> UploadFile(IFormFile file, string bucketName, string? prefix, string namefile);
        Task<List<S3ObjectDto>> GetAllFilesAsync(string bucketName, string? prefix);
        Task<ResponseMessage> DeleteFileAsync(string bucketName, string key);
    }
}
