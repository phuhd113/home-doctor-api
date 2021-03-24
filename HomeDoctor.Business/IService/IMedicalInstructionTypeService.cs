using HomeDoctor.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.IService
{
    public interface IMedicalInstructionTypeService
    {
        public Task<ICollection<MedicalInstructionType>> GetMedicalInstructionTypeByStatus(string? status);  
    }
}
