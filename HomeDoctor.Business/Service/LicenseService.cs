﻿using HomeDoctor.Business.IService;
using HomeDoctor.Business.Repositories;
using HomeDoctor.Business.UnitOfWork;
using HomeDoctor.Business.ViewModel.RequestModel;
using HomeDoctor.Data.Models;
using Microsoft.EntityFrameworkCore;
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

        public async Task<int> CreateLicense(LicenseCreate license)
        {
            if (license != null)
            {
                var licenseCreate = new License()
                {
                    DateActive = DateTime.Now,
                    Days = license.Days,
                    Description = license.Description,
                    Name = license.Name,
                    Price = license.Price,
                    Status = "ACTIVE",                   
                };
                if (license.FromBy != null)
                {
                    var tmp = await _repo.GetById(license.FromBy);
                    if(tmp!= null)
                    {
                        // update dateCancel
                        tmp.DateCancel = DateTime.Now;
                        tmp.Status = "CANCEL";
                        await _repo.Update(tmp);
                        // Create new license
                        licenseCreate.FromBy = license.FromBy;                       
                    }
                };  
                if (await _repo.Insert(licenseCreate))
                {
                    await _uow.CommitAsync();
                    return licenseCreate.LicenseId;
                }
            }
            return 0;
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
                var license = await _repo.GetDbSet().Where(x => x.Days >= days).OrderBy(x => x.Days).FirstOrDefaultAsync();
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
            var licenses = await _repo.GetDbSet().Where(x => string.IsNullOrEmpty(status) ? true : x.Status.Equals(status.ToUpper())).ToListAsync();
            if (licenses.Any())
            {
                return licenses;
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
