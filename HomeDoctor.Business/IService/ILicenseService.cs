using HomeDoctor.Business.ViewModel.RequestModel;
using HomeDoctor.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.IService
{
    public interface ILicenseService
    {
        public Task<int> CreateLicense(LicenseCreate license);
        public Task<bool> UpdateLicense(License? license);
        public Task<bool> DeleteLicense(int licenseId);
        public Task<ICollection<License>> GetLicensesByStatus(string? status);
        public Task<License> GetLicenseById(int licenseId);
        public Task<License> GetLicenseByDays(int days);
    }
}
