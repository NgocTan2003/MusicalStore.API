using Microsoft.AspNetCore.Mvc;
using MusicalStore.Common.ResponseBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Application.Services.Interfaces
{
    public interface IBucketService
    {
        Task<ResponseMessage> CreateBucketAsync(string bucketName);
        Task<Array> GetAllBucketAsync();
        Task<ResponseMessage> DeleteBucketAsync(string bucketName);
    }
}
