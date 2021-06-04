using HomeDoctor.Business.IService;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Linq;
using HomeDoctor.Business.Repositories;
using HomeDoctor.Data.Models;
using HomeDoctor.Business.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace HomeDoctor.Business.Service
{
    public class ImageService : IImageService
    {
        private readonly IRepositoryBase<MedicalInstruction> _repoMI;
        private readonly IUnitOfWork _uow;

        public ImageService(IUnitOfWork uow)
        {
            _uow = uow;
            _repoMI = _uow.GetRepository<MedicalInstruction>();
        }

        public async Task<string> ReadImage(IFormFile fileImage)
        {            
            try
            {
                /*
                //IronOcr.Installation.LicenseKey = "496B34E7-1043-4B07-A556-313FF383FC5E";
                bool result = License.IsValidLicense("3C2C3B9A-DC89-41DD-985E-33999472D889");
                var Ocr = new IronTesseract();               
                Ocr.Language = OcrLanguage.Vietnamese;
                using (var input = new OcrInput())
                {
                    input.AddImage(fileImage.OpenReadStream());
                    // Add image filters if needed
                    input.Deskew();
                    var Result =await Ocr.ReadAsync(input);
                    string TestResult = Result.Text;
                    if (!string.IsNullOrEmpty(TestResult))
                    {
                        return TestResult;
                    }               
                }
                */

                /*
                AsposeOcr ocr = new AsposeOcr();
                MemoryStream ms = new MemoryStream();
                fileImage.OpenReadStream().CopyTo(ms);
                string text = ocr.RecognizeImage(ms);
                if (!string.IsNullOrEmpty(text))
                {
                    return text;
                }
                */
                return null;
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
            
        }

        public async Task<bool> Upload(string phoneNumber,int healthRecordId,int medicalInstructionTypeId, IFormFile fileImage)
        {          
            if(fileImage.Length > 0)
            {
                var pathRoot = Directory.GetDirectoryRoot(Directory.GetCurrentDirectory());
                Console.WriteLine("Root : " + pathRoot);
                Console.WriteLine("File Image : " + fileImage);
                
                // create path of folder
                var srcFile = Path.Combine(pathRoot,"data",phoneNumber, "HR" + healthRecordId, "MIT" + medicalInstructionTypeId);

                // check if folder is not exist then create new
                if (!Directory.Exists(srcFile))
                    Directory.CreateDirectory(srcFile);
                Console.WriteLine("SrcFile : " + srcFile);
                // create path of image
                var filePath = Path.Combine(srcFile, fileImage.FileName);
                Console.WriteLine("filePath : " + filePath);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    // create new file with folder
                    await fileImage.CopyToAsync(stream);
                }
                return true;
            }           
            return false;
        }
        public async Task<bool> CheckImageExist(int healthRecordId, int medicalInstructionTypeId,IFormFile formFile)
        {
            if(healthRecordId != 0 && medicalInstructionTypeId != 0 && formFile != null)
            {
                // Parse formFile to Has 
                Stream st = formFile.OpenReadStream();
                MemoryStream mst = new MemoryStream();
                await st.CopyToAsync(mst);
                string hashImageChooose = ToMD5Hash(mst.ToArray());
                // Get Image from MItype 
                var mis = await _repoMI.GetDbSet().Where(x => x.HealthRecordId == healthRecordId && x.MedicalInstructionTypeId == medicalInstructionTypeId && x.MedicalInstructionImages.Any()).Select(
                    x => new
                    {
                        x.MedicalInstructionImages
                    }).ToListAsync();
                if (mis.Any())
                {
                    foreach (var images in mis)
                    {
                        var imageMd5s = images.MedicalInstructionImages.Select(x =>
                            ToMD5Hash(System.IO.File.ReadAllBytes(Path.Combine(Directory.GetDirectoryRoot(Directory.GetCurrentDirectory()), "data",x.Image)))).ToList();
                        if(imageMd5s.Any(x => x.Equals(hashImageChooose)))
                        {
                            return true;
                        }
                    }
                }
            }           
            return false;
        }
        private string ToMD5Hash(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                return null;

            using (var md5 = MD5.Create())
            {
                return string.Join("", md5.ComputeHash(bytes).Select(x => x.ToString("X2")));
            }
        }      
      
    }
}
