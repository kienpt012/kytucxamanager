
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
    public interface IQuanHuyenService
    {
        List<QuanHuyenModelView> GetAll();
        QuanHuyenModelView GetById(int id);
        List<QuanHuyenModelView> GetDistrictForCityId(int cityId);
        QuanHuyenModelView GetByName(string name);
        QuanHuyenModelView GetByid(int id);
    }

    public class QuanHuyenService : IQuanHuyenService
    {
        private readonly IQuanHuyenRepository _quanHuyenRepository;
        private IUnitOfWork _unitOfWork;
        public QuanHuyenService(IQuanHuyenRepository quanHuyenRepository,
            IUnitOfWork unitOfWork)
        {
            _quanHuyenRepository = quanHuyenRepository;
            _unitOfWork = unitOfWork;
        }       
        public QuanHuyenModelView GetByid(int id)
        {
            var data = _quanHuyenRepository.FindAll(p => p.Id == id).FirstOrDefault();
            if (data != null)
            {
                var model = new QuanHuyenModelView()
                {
                    Id = data.Id,
                    Ten = data.Ten,
                    MaTP = data.MaTP
                };
                return model;
            }
            return null;
        }
        public List<QuanHuyenModelView> GetAll()
        {
            var data = _quanHuyenRepository.FindAll().Select(c => new QuanHuyenModelView()
            {
                Id = c.Id,
                Ten = c.Ten
            }).ToList();

            return data;
        }

        public List<QuanHuyenModelView> GetDistrictForCityId(int cityId)
        {
            var data = _quanHuyenRepository.FindAll().Where(c => c.MaTP == cityId).Select(c => new QuanHuyenModelView()
            {
                Id = c.Id,
                Ten = c.Ten
            }).ToList();

            return data;
        }

        public QuanHuyenModelView GetById(int id)
        {
            var data = _quanHuyenRepository.FindAll(p => p.Id == id).FirstOrDefault();
            if (data != null)
            {
                var model = new QuanHuyenModelView()
                {
                    Id = data.Id,
                    Ten = data.Ten,
                };
                return model;
            }
            return null;
        }
        public QuanHuyenModelView GetByName(string name)
        {
            var data = _quanHuyenRepository.FindAll(p => p.Ten.ToLower().Contains(name.ToLower())).FirstOrDefault();
            if (data != null)
            {
                var model = new QuanHuyenModelView()
                {
                    Id = data.Id,
                    Ten = data.Ten,
                };
                return model;
            }
            return null;
        }
    }
}
