using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MusicalStore.Application.Services.Interfaces;
using MusicalStore.Common.Enums;
using MusicalStore.Common.ResponseBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Application.Services.Implements
{
    public class AwsS3Service : IAwsS3Service
    {
        private readonly IAmazonS3 _s3Client;

        public AwsS3Service(IAmazonS3 amazonS3)
        {
            _s3Client = amazonS3;
        }

        public async Task<List<S3ObjectDto>> GetAllFilesAsync(string bucketName, string? prefix)
        {
            var request = new ListObjectsV2Request()
            {
                BucketName = bucketName,
                Prefix = prefix
            };
            var result = await _s3Client.ListObjectsV2Async(request);

            var s3Objects = result.S3Objects.Select(s =>
            {
                var urlRequest = new GetPreSignedUrlRequest()
                {
                    BucketName = bucketName,
                    Key = s.Key,
                    Expires = DateTime.UtcNow.AddMinutes(2)
                };
                var presignedUrl = _s3Client.GetPreSignedURL(urlRequest);
                return new S3ObjectDto()
                {
                    Name = s.Key,
                    PresignedUrl = presignedUrl
                };
            }).ToList();
            return s3Objects;
        }

        public async Task<S3Response> UploadFile(IFormFile file, string bucketName, string? prefix, string namefile)
        {
            var result = new S3Response();
            try
            {
                var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName);
                if (!bucketExists)
                {
                    result.StatusCode = 404;
                    result.Message = $"Bucket {bucketName} does not exist.";
                    return result;
                }
                else
                {
                    string key = string.IsNullOrEmpty(namefile) ? file.FileName : $"{prefix}/{namefile}";
                    var request = new PutObjectRequest()
                    {
                        BucketName = bucketName,
                        Key = key,
                        InputStream = file.OpenReadStream()
                    };
                    request.Metadata.Add("Content-Type", file.ContentType);
                    await _s3Client.PutObjectAsync(request);

                    result.StatusCode = 200;
                    result.Message = $"File {key} uploaded to S3 successfully!";
                    result.PresignedUrl = $"https://{bucketName}.s3.amazonaws.com/{key}";
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ResponseMessage> DeleteFileAsync(string bucketName, string? key)
        {
            var result = new ResponseMessage();
            try
            {
                var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName);
                if (!bucketExists)
                {
                    result.StatusCode = 404;
                    result.Message = $"Bucket {bucketName} does not exist";
                    return result;
                }
                await _s3Client.DeleteObjectAsync(bucketName, key);
                result.StatusCode = 200;
                result.Message = "Delete success";
            }
            catch (Exception ex)
            {
                result.StatusCode = 400;
                result.Message = ex.Message;
            }
            return result;
        }
    }
}
