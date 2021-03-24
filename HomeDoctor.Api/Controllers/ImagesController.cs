using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HomeDoctor.Business.IService;
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

        [HttpPost]
        public async Task<IActionResult> TestReadImage(IFormFile image)
        {
            await _serImage.TestReadImage(image);
            return BadRequest();
        }
        [HttpGet("TestTime")]
        public async Task<IActionResult> GetTime()
        {
            string tmp1 = "Datetime.now : " +DateTime.Now.ToUniversalTime();
            string tmp2 = "DateTime.UtcNow : "+DateTime.Now.ToUniversalTime().AddHours(7);           
            string tmp3 = "Tmp3 : " + TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.Local);

            string tmp4 = TimeZoneInfo.Local.Id.Equals("SE Asia Standard Time") ? DateTime.Now.ToString() : DateTime.Now.AddDays(7).ToString();
            return Ok(tmp1 + " \n " + tmp2 + "\n" + tmp3 + "\n" + "\n" + tmp4 + "\n" + TimeZoneInfo.Local.ToString());
        }
    }
}
