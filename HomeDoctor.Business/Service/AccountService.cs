using HomeDoctor.Business.IService;
using HomeDoctor.Business.Repositories;
using HomeDoctor.Business.UnitOfWork;
using HomeDoctor.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.Service
{
    public class AccountService : IAccountService
    {
        private readonly IRepositoryBase<Account> _repo;
        private readonly IUnitOfWork _uow;

        public AccountService(IUnitOfWork uow)
        {           
            _uow = uow;
            _repo = _uow.GetRepository<Account>();
        }      
    }
}
