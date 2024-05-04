using Amazon.S3;
using MusicalStore.Application.Services.Interfaces;
using MusicalStore.Common.ResponseBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Application.Services.Implements
{
    public class BucketService : IBucketService
    {
        private readonly IAmazonS3 _s3Client;

        public BucketService(IAmazonS3 amazonS3)
        {
            _s3Client = amazonS3;
        }

        public async Task<ResponseMessage> CreateBucketAsync(string bucketName)
        {
            var result = new ResponseMessage();
            try
            {
                var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName);
                if (bucketExists)
                {
                    result.StatusCode = 404;
                    result.Message = $"Bucket {bucketName} already exists";
                }
                else
                {
                    await _s3Client.PutBucketAsync(bucketName);
                    result.StatusCode = 200;
                    result.Message = $"Bucket {bucketName} create success";
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = 400;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ResponseMessage> DeleteBucketAsync(string bucketName)
        {
            var result = new ResponseMessage();
            try
            {
                var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName);
                if (!bucketExists)
                {
                    result.StatusCode = 404;
                    result.Message = $"Bucket {bucketName} does not exist";
                }
                else
                {
                    await _s3Client.DeleteBucketAsync(bucketName);
                    result.StatusCode = 200;
                    result.Message = "Delete success";
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = 400;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<Array> GetAllBucketAsync()
        {
            var data = await _s3Client.ListBucketsAsync();
            var bucketNames = data.Buckets.Select(b => b.BucketName).ToArray();
            return bucketNames;
        }
    }
}
