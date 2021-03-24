using HomeDoctor.Business.IService;
using HomeDoctor.Business.Repositories;
using HomeDoctor.Business.UnitOfWork;
using HomeDoctor.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.Service
{
    public class LicenseService : ILicenseService
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepositoryBase<License> _repo;

        public LicenseService(IUnitOfWork uow)
        {
            _uow = uow;
            _repo = _uow.GetRepository<License>();
        }

        public async Task<bool> CreateLicense(License license)
        {
            if (license != null)
            {
                var check = await _repo.Insert(license);
                if (check)
                {
                    await _uow.CommitAsync();
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> DeleteLicense(int licenseId)
        {
            if (licenseId != 0)
            {
                var license = await _repo.GetById(licenseId);
                license.Status = "CANCEL";
                if (license != null)
                {
                    var check = await _repo.Update(license);
                    if (check)
                    {
                        await _uow.CommitAsync();
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<License> GetLicenseByDays(int days)
        {
            if(days != 0)
            {
                var license = _repo.GetDbSet().Where(x => x.Days >= days).OrderBy(x => x.Days).FirstOrDefault();
                if(license != null)
                {
                    return license;
                }
            }
            return null;
        }

        public async Task<License> GetLicenseById(int licenseId)
        {
            if(licenseId != 0)
            {
                var license = await _repo.GetById(licenseId);
                if(license != null)
                {
                     return license;
                }
                
            }
            return null;
        }

        public async Task<ICollection<License>> GetLicensesByStatus(string? status)
        {
            var licenses = _repo.GetDbSet().Where(x => string.IsNullOrEmpty(status) ? true : x.Status.Equals(status.ToUpper()));
            if (licenses.Count() != 0)
            {
                return licenses.ToList();
            }
            return null;
        }

        public async Task<bool> UpdateLicense(License? license)
        {
            if (license.LicenseId != 0)
            {
                var tmp = await _repo.GetById(license.LicenseId);
                if (tmp != null)
                {
                    if (!string.IsNullOrEmpty(license.Name)) tmp.Name = license.Name;
                    if (!string.IsNullOrEmpty(license.Status)) tmp.Status = license.Status;
                    if (!string.IsNullOrEmpty(license.Description)) tmp.Description = license.Description;
                    if (license.Days != 0) tmp.Days = license.Days;
                    if (license.Price != 0) tmp.Price = license.Price;

                    var check = await _repo.Update(tmp);
                    if (check)
                    {
                        await _uow.CommitAsync();
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
