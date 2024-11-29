
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
    public interface IPhuongXaService
    {
        List<PhuongXaModelView> GetAll();
        List<PhuongXaModelView> GetWardsForDistrictId(int districtId);
        PhuongXaModelView GetById(int id);
        PhuongXaModelView GetByName(string name);
    }
    public class PhuongXaService : IPhuongXaService
    {
        private readonly IPhuongXaRepository _phuongXaRepository;
        private IUnitOfWork _unitOfWork;
        public PhuongXaService(IPhuongXaRepository phuongXaRepository,
            IUnitOfWork unitOfWork)
        {
            _phuongXaRepository = phuongXaRepository;
            _unitOfWork = unitOfWork;
        }              
        public List<PhuongXaModelView> GetAll()
        {
            var data = _phuongXaRepository.FindAll().Select(c => new PhuongXaModelView()
            {
                Id = c.Id,
                Ten = c.Ten
            }).ToList();

            return data;
        }
        public PhuongXaModelView GetById(int id)
        {
            var data = _phuongXaRepository.FindAll(p => p.Id == id).FirstOrDefault();
            if (data != null)
            {
                var model = new PhuongXaModelView()
                {
                    Id = data.Id,
                    Ten = data.Ten,
                };
                return model;
            }
            return null;
        }
        public List<PhuongXaModelView> GetWardsForDistrictId(int districtId)
        {
            var data = _phuongXaRepository.FindAll().Where(d => d.MaQH == districtId).Select(c => new PhuongXaModelView()
            {
                Id = c.Id,
                Ten = c.Ten
            }).ToList();

            return data;
        }
        public PhuongXaModelView GetByName(string name)
        {
            var data = _phuongXaRepository.FindAll(p => p.Ten.ToLower().Trim().Contains(name.ToLower().Trim())).FirstOrDefault();
            if (data != null)
            {
                var model = new PhuongXaModelView()
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
