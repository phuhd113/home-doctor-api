using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.IService
{
    public interface IImageService
    {
        public Task<bool> Upload(string phoneNumber, int healthRecordId, int medicalInstructionTypeId, IFormFile fileImage);

        public Task<string> ReadImage(IFormFile fileImage);
        //public Task TestUpdateCheckImage(IFormFile formFile);
        public Task<bool> CheckImageExist(int healthRecordId, int medicalInstructionTypeId, IFormFile formFile);

    }
}
