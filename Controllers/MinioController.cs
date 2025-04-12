using Microsoft.AspNetCore.Mvc;
using MinioExample.Services;
using System.IO;
using System.Threading.Tasks;

namespace MinioExample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MinioController : ControllerBase
    {
        private readonly MinioService _minioService;

        public MinioController()
        {   
            _minioService = new MinioService();
        }

        [HttpPost("create-bucket")]
        public async Task<IActionResult> CreateBucket(string bucketName)
        {
            await _minioService.CreateBucketAsync(bucketName);
            return Ok($"Bucket {bucketName} created.");
        }

        [HttpPost("upload-file")]
        public async Task<IActionResult> UploadFile(string bucketName, string objectName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), objectName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound($"File {objectName} not found.");
            }
            await _minioService.UploadFileAsync(bucketName, objectName, filePath);
            return Ok($"File {objectName} uploaded to bucket {bucketName}.");
        }

        [HttpGet("get-file")]
        public async Task<IActionResult> GetFile(string bucketName, string objectName)
        {
            var fileUrl = await _minioService.GetFileAsync(bucketName, objectName);
            return Ok(new { Url = fileUrl });
        }
    }
}
