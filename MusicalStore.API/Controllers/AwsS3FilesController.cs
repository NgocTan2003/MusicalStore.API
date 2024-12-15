using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using MusicalStore.Application.Services.Interfaces;
using MusicalStore.Common.ResponseBase;

namespace MusicalStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AwsS3FilesController : ControllerBase
    {
        private readonly IAmazonS3 _s3Client;
        private readonly IAwsS3Service _awsS3Service;

        public AwsS3FilesController(IAmazonS3 s3Client, IAwsS3Service awsS3Service)
        {
            _s3Client = s3Client;
            _awsS3Service = awsS3Service;
        }


        //[HttpPost]
        //public async Task<IActionResult> UploadFileAsync(string username, IFormFile file, string bucketName, string? prefix)
        //{
        //    try
        //    {
        //        var result = await _awsS3Service.UploadFileAsync(file, bucketName, prefix, username);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { StatusCode = 500, Message = ex.Message });
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file, string bucketName, string? prefix, string? namefile)
        {
            try
            {
                var result = await _awsS3Service.UploadFile(file, bucketName, prefix, namefile);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { StatusCode = 500, Message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFilesAsync(string bucketName, string? prefix)
        {
            try
            {
                var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName);
                if (!bucketExists)
                {
                    return NotFound($"Bucket {bucketName} does not exist.");
                }
                else
                {
                    var list = await _awsS3Service.GetAllFilesAsync(bucketName, prefix);
                    return Ok(list);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { StatusCode = 500, Message = ex.Message });
            }
        }

        [HttpGet("preview")]
        public async Task<IActionResult> GetFileByKeyAsync(string bucketName, string key)
        {
            var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName);
            if (!bucketExists) return NotFound($"Bucket {bucketName} does not exist.");
            var s3Object = await _s3Client.GetObjectAsync(bucketName, key);
            return File(s3Object.ResponseStream, s3Object.Headers.ContentType);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFileAsync(string bucketName, string key)
        {
            try
            {
                var result = await _awsS3Service.DeleteFileAsync(bucketName, key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { StatusCode = 500, Message = ex.Message });
            }
        }
    }
}
