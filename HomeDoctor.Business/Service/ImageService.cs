using HomeDoctor.Business.IService;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace HomeDoctor.Business.Service
{
    public class ImageService : IImageService
    {
        public async Task TestReadImage(IFormFile fileImage)
        {
            try
            {
                var pathImage = Path.Combine(Directory.GetCurrentDirectory(), fileImage.FileName);
                var pixImage = Pix.LoadFromFile(pathImage);
                var pathVieData = Path.Combine(Directory.GetDirectoryRoot(Directory.GetCurrentDirectory()),"root");
                TesseractEngine engine = new TesseractEngine(pathVieData, "vie", EngineMode.Default);
                Page page = engine.Process(pixImage, PageSegMode.Auto);
                string result = page.GetText();
                Console.WriteLine(result);
                Console.ReadKey();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
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
    }
}
