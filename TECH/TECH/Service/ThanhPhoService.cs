
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Data.DatabaseEntity;
using TECH.General;
using TECH.Reponsitory;
using TECH.Utilities;

namespace TECH.Service
{
    public interface IThanhPhoService
    {
        List<ThanhPhoModelView> GetAll();
        ThanhPhoModelView GetById(int id);
        ThanhPhoModelView GetByName(string name);
    }

    public class ThanhPhoService : IThanhPhoService
    {
        private readonly IThanhPhoRepository _thanhPhoRepository;
        private IUnitOfWork _unitOfWork;
        public ThanhPhoService(IThanhPhoRepository thanhPhoRepository,
            IUnitOfWork unitOfWork)
        {
            _thanhPhoRepository = thanhPhoRepository;
            _unitOfWork = unitOfWork;
        }       
        public ThanhPhoModelView GetById(int id)
        {
            var data = _thanhPhoRepository.FindAll(p => p.Id == id).FirstOrDefault();
            if (data != null)
            {
                var model = new ThanhPhoModelView()
                {
                    Id = data.Id,
                    Ten = data.Ten
                };
                return model;
            }
            return null;
        }
        public List<ThanhPhoModelView> GetAll()
        {
            var data = _thanhPhoRepository.FindAll().Select(c => new ThanhPhoModelView()
            {
                Id = c.Id,
                Ten = c.Ten
            }).ToList();
            return data;
        }       
        public ThanhPhoModelView GetByName(string name)
        {
            var data = _thanhPhoRepository.FindAll(p => p.Ten.ToLower().Trim().Contains(name.ToLower().Trim())).FirstOrDefault();
            if (data != null)
            {
                var model = new ThanhPhoModelView()
                {
                    Id = data.Id,
                    Ten = data.Ten
                };
                return model;
            }
            return null;
        }
    }
}
