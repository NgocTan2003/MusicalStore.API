using Amazon.Runtime.Internal;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicalStore.Application.Services.Implements;
using MusicalStore.Application.Services.Interfaces;
using MusicalStore.Common.Enums;
using MusicalStore.Common.ResponseBase;
using MusicalStore.Dtos.Users;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Utilities.Zlib;

namespace MusicalStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BucketsController : ControllerBase
    {
        private readonly IBucketService _bucketService;

        public BucketsController(IBucketService bucketService)
        {
            _bucketService = bucketService;
        }

        [HttpPost("bucketName")]
        public async Task<IActionResult> CreateBucketAsync(string bucketName)
        {
            try
            {
                var result = await _bucketService.CreateBucketAsync(bucketName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { StatusCode = 500, Message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBucketAsync()
        {
            try
            {
                var array = await _bucketService.GetAllBucketAsync();
                return Ok(array);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { StatusCode = 500, Message = ex.Message });
            }
        }

        [HttpDelete("bucketName")]
        public async Task<IActionResult> DeleteBucketAsync(string bucketName)
        {
            try
            {
                var result = await _bucketService.DeleteBucketAsync(bucketName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { StatusCode = 500, Message = ex.Message });
            }
        }
    }
}

