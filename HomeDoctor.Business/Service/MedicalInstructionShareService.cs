using HomeDoctor.Business.IService;
using HomeDoctor.Business.Repositories;
using HomeDoctor.Business.UnitOfWork;
using HomeDoctor.Business.ViewModel.ResponeModel;
using HomeDoctor.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.Service
{
    public class MedicalInstructionShareService : IMedicalInstructionShareService
    {
        private readonly IRepositoryBase<MedicalInstructionShare> _repo;
        private readonly IRepositoryBase<Contract> _repoContract;
        private readonly IUnitOfWork _uow;
        private readonly IMedicalInstructionTypeService _serMIT;


        public MedicalInstructionShareService(IUnitOfWork uow,IMedicalInstructionTypeService serMIT)
        {
            _uow = uow;
            _repo = _uow.GetRepository<MedicalInstructionShare>();
            _repoContract = _uow.GetRepository<Contract>();
            _serMIT = serMIT;
        }

        public async Task<ICollection<MedicalInstructionShareRespone>> GetMedicalInstructionShare(int contractId)
        {
            if(contractId != 0)
            {
                // Get type and image of medicalInstructionShared with doctorId and patientId
                var miShares = await _repo.GetDbSet().Where(x => x.ContractId == contractId).Include(x => x.MedicalInstruction).ThenInclude(x => x.Prescription).ThenInclude(x => x.MedicationSchedules).Select(x => new
                {
                    x.MedicalInstruction.MedicalInstructionTypeId,
                    x.MedicalInstruction.MedicalInstructionType.Name,
                    x.MedicalInstructionId,
                    x.MedicalInstruction.Image,
                    x.MedicalInstruction.Diagnose,
                    x.MedicalInstruction.Description,
                    x.Status,
                    x.MedicalInstruction.Prescription
                }).ToListAsync();
                if (miShares.Any())
                {
                    var respone = miShares.GroupBy(x => x.Name).Select(x => new MedicalInstructionShareRespone()
                    {
                        MedicalInstructionType = x.Key,
                        MedicalInstructions = x.Select(y => new MedicalInstructionShareRespone.MedicalInstructionInformation() {
                            Status = y.Status,
                            MedicalInstructionId = y.MedicalInstructionId,
                            Image = y.Image,
                            Description = y.Description,
                            Diagnose = y.Diagnose,
                            PrescriptionRespone = y.Prescription != null ? new PrescriptionRespone()
                            {
                                DateStarted = y.Prescription.DateStarted,
                                DateFinished = y.Prescription.DateFinished,
                                ReasonCancel = y.Prescription.ReasonCancel,
                                DateCanceled = y.Prescription.DateCanceled,
                                Status = y.Prescription.Status,
                                MedicationSchedules = y.Prescription.MedicationSchedules.Select(y =>
                                new MedicationScheduleRespone()
                                {
                                    MedicationName = y.MedicationName,
                                    Content = y.Content,
                                    Unit = y.Unit,
                                    UseTime = y.UseTime,
                                    Morning = y.Morning,
                                    Noon = y.Noon,
                                    Night = y.Night,
                                    AfterNoon = y.AfterNoon
                                }).ToList()
                            } : null
                        }).ToList()
                    }).ToList();                   
                    return respone;
                }
            }
            return null;
        }

        public async Task<bool> ShareMedicalInstructionById(int contractId, ICollection<int> medicalInstructions)
        {
            if(contractId != 0)
            {
                if(await _repoContract.GetById(contractId) != null)
                {
                    _repo.GetDbSet().AddRange(
                        medicalInstructions.Select(id => new MedicalInstructionShare(){
                            ContractId = contractId,
                            MedicalInstructionId = id,
                            Status = "SHARE",                           
                        }
                    ));
                    await _uow.CommitAsync();
                    return true;
                }
            }
            return false;
        }
    }
}
