using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {

        private readonly IImageRepository imageRepository;

        public ImageController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        

        [HttpPost]
        [Route("Upload")]
        public async Task<ActionResult> Upload([FromForm] ImageUploadRequestDTO request)
        {
            ValidateFile(request);
            if (ModelState.IsValid)
            {
                var imageDomainModel = new Image
                {
                    File = request.File,
                    FileDescription = request.FileDescription,
                    FileName = request.Filename,
                    FileSizeInBytes = request.File.Length,
                    FileExtension = Path.GetExtension(request.File.FileName)
                }; 

                //Upload Image
                await imageRepository.Upload(imageDomainModel);

                return Ok(imageDomainModel);


            }
            return BadRequest(ModelState);
           
        }

        private void ValidateFile(ImageUploadRequestDTO request)
        {
            var allowedExtension = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtension.Contains(Path.GetExtension(request.File.FileName)))
            {
                ModelState.AddModelError("file", "Unexpected File Extension");
            }
            if (request.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File Size is more than MB");
            }
        }

    }
}
