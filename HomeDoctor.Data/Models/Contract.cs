using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HomeDoctor.Data.Models
{
    public class Contract
    {
        public int ContractId { get; set; }
        //Doctor
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string FullNameDoctor { get; set; }
        [Required]
        [Column(TypeName = "varchar(15)")]
        public string PhoneNumberDoctor { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string WorkLocationDoctor { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string AddressDoctor { get; set; }
        [Required]
        [Column(TypeName = "date")]
        public DateTime DOBDoctor { get; set; }
        //Patient
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string FullNamePatient { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(15)")]
        public string PhoneNumberPatient { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string AddressPatient { get; set; }
        [Required]
        [Column(TypeName = "date")]
        public DateTime DOBPatient { get; set; }
        // Contract Detail
        [Required]
        [Column(TypeName = "varchar(15)")]
        public string ContractCode { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string Note { get; set; }
        [Required]
        [Column(TypeName = "varchar(15)")]
        public string Status { get; set; }
        public ICollection<Disease> Diseases { get; set; }
        //DateTime
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime DateCreated { get; set; }
        [Required]
        [Column(TypeName = "date")]
        public DateTime DateStarted { get; set; }
        [Required]
        [Column(TypeName = "date")]
        public DateTime DateFinished { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateLocked { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateApproved { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateSigned { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateCancel { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string? ReasonLocked { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string? ReasonCancel { get; set; }
        //License
        [Column(TypeName = "nvarchar(50)")]
        public string NameLicense { get; set; }
        [Column(TypeName = "Money")]
        public decimal PriceLicense { get; set; }
        public int DaysOfTracking { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string TransactionNo { get; set; }
        //Relationship
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public int? LicenseId { get; set; }
        public License License { get; set; }
        public HealthRecord HealthRecord { get; set; }
        public ICollection<ContractMedicalInstruction> ContractMedicalInstructions { get; set; }
       
    }
}
