using HomeDoctor.Business.ViewModel.ResponeModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.IService
{
    public interface IMedicalInstructionShareService
    {
        // Get list MedicalInstruction with image of patient for doctor
        public Task<ICollection<MedicalInstructionShareRespone>> GetMedicalInstructionShare(int contractId);

        public Task<bool> ShareMedicalInstructionById(int contractId, ICollection<int> medicalInstructions);
    }
}
