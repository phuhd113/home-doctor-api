using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HomeDoctor.Business.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeDoctor.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService _serImage;

        public ImagesController(IImageService serImage)
        {
            _serImage = serImage;
        }

        /// <summary>
        /// Load Image by Path Image
        /// </summary>    
        [HttpGet]
        public async Task<IActionResult> GetImageByPath(string pathImage)
        {
            if (!String.IsNullOrEmpty(pathImage))
            {             

                pathImage = Path.Combine(Directory.GetDirectoryRoot(Directory.GetCurrentDirectory()),"data", pathImage);
                Console.WriteLine("Path Image :" + pathImage);
                byte[] stream = await System.IO.File.ReadAllBytesAsync(pathImage);
                if (pathImage.Contains(".jpg"))
                {
                    return File(stream, "image/jpeg");
                }
                if (pathImage.Contains(".png"))
                {
                    return File(stream, "image/png");
                }
            }
            return NotFound();

        }
       
        
        [HttpPost("CheckExistImage")]
        public async Task<IActionResult> CheckExistImage(int healthRecordId,int medicalInstructionTypeId,IFormFile image)
        {          
            if(image != null)
            {
                var respone = await _serImage.CheckImageExist(healthRecordId,medicalInstructionTypeId,image);
                if (respone)
                {
                    return NoContent();
                }
                else
                {
                    return Ok();
                }
                                      
            }
            return BadRequest();
        }
    }
}
