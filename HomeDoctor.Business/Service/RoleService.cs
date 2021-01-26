using HomeDoctor.Business.IService;
using HomeDoctor.Business.Repositories;
using HomeDoctor.Business.UnitOfWork;
using HomeDoctor.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeDoctor.Business.Service
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _uow;
        //private readonly IRepositoryBase<Role> _repo;

        public RoleService(IUnitOfWork uow)
        {
            _uow = uow;
            //_repo = _uow.GetRepository<Role>();
        }

        //public async Task<ICollection<Role>> GetAllRole()
        //{
        //    return await _repo.GetAll();
        //}
    }
}
