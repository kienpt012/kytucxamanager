
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
    public interface IThanhVienPhongService
    {
        ThanhVienPhongModelView GetByid(int id);
        ThanhVienPhongModelView GetByThanhVienByMaPhongMaKH(int maKH,int maPhong);
        void Add(ThanhVienPhongModelView view);
        bool Update(ThanhVienPhongModelView view);
        bool Deleted(int id);
        void DeletedByMaPhong(int maPhong);
        void Save();
        List<ThanhVienPhongModelView> GetThanhVienByPhong(int maPhong);
    }

    public class ThanhVienPhongService : IThanhVienPhongService
    {
        private readonly IThanhVienPhongRepository _thanhVienPhongRepository;
        private IUnitOfWork _unitOfWork;
        public ThanhVienPhongService(IThanhVienPhongRepository thanhVienPhongRepository,
            IUnitOfWork unitOfWork)
        {
            _thanhVienPhongRepository = thanhVienPhongRepository;
            _unitOfWork = unitOfWork;
        }       
        public ThanhVienPhongModelView GetByThanhVienByMaPhongMaKH(int maKH, int maPhong)
        {
            if (maKH > 0 && maPhong > 0)
            {
                var thanhvien = _thanhVienPhongRepository.FindAll(p => p.MaKH == maKH && p.MaPhong == maPhong).Select(p => new ThanhVienPhongModelView()
                {
                    Id = p.Id,
                    MaKH = p.MaKH,
                    MaPhong = p.MaPhong,
                }).FirstOrDefault();

                if (thanhvien != null)                
                    return thanhvien;                
            }
            return null;
        }
        public ThanhVienPhongModelView GetByid(int id)
        {
            var data = _thanhVienPhongRepository.FindAll(p => p.Id == id).FirstOrDefault();
            if (data != null)
            {
                var model = new ThanhVienPhongModelView()
                {
                    Id = data.Id,
                    MaKH = data.MaKH,
                    MaPhong = data.MaPhong,
                };
                return model;
            }
            return null;
        }
     
        public void Add(ThanhVienPhongModelView view)
        {
            try
            {
                if (view != null)
                {
                    var dichVuPhong = new ThanhVienPhong
                    {
                        MaKH = view.MaKH,
                        MaPhong = view.MaPhong,   
                    };
                    _thanhVienPhongRepository.Add(dichVuPhong);
                }
            }
            catch (Exception ex)
            {
            }
        }
        public void Save()
        {
            _unitOfWork.Commit();
        }
        public bool Update(ThanhVienPhongModelView view)
        {
            try
            {
                var dataServer = _thanhVienPhongRepository.FindById(view.Id);
                if (dataServer != null)
                {
                    dataServer.MaKH = view.MaKH;
                    dataServer.MaPhong = view.MaPhong;
                    _thanhVienPhongRepository.Update(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }      
        public bool Deleted(int id)
        {
            try
            {
                var dataServer = _thanhVienPhongRepository.FindById(id);
                if (dataServer != null)
                {
                    _thanhVienPhongRepository.Remove(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return false;
        }
        public void DeletedByMaPhong(int maPhong)
        {
            if (maPhong > 0)
            {
                var dichVuPhongByMaPhong = _thanhVienPhongRepository.FindAll(d=>d.MaPhong == maPhong).ToList();
                if (dichVuPhongByMaPhong != null && dichVuPhongByMaPhong.Count > 0)
                {
                    foreach (var item in dichVuPhongByMaPhong)
                    {
                        Deleted(item.Id);
                    }
                    Save();
                }               
            }
        }
        public List<ThanhVienPhongModelView> GetThanhVienByPhong(int maPhong)
        {
            var data = _thanhVienPhongRepository.FindAll(p => p.MaPhong == maPhong).Select(p => new ThanhVienPhongModelView()
            {
                Id = p.Id,
                MaKH = p.MaKH,
                MaPhong = p.MaPhong,
            }).ToList();
            return data;
        }
    }
}
