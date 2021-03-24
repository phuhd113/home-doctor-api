using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeDoctor.Business.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeDoctor.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IImageService _imageService;

        public RolesController(IRoleService roleService, IImageService imageService)
        {
            _roleService = roleService;
            _imageService = imageService;
        }


        //[HttpGet]
        //public async Task<IActionResult> GetRoles()
        //{
        //    var roles = await _roleService.GetAllRole();
        //    return Ok(roles);
        //}
        [HttpPost]
        public async Task<IActionResult> TestUploadImage(int id, IFormFile image)
        {
            var tmp = await _imageService.Upload("0354888765", 2, id, image);
            if (tmp) return Ok("Upload Success !");
            else 
            return BadRequest();
        }
    }
}
