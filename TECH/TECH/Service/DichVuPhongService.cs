
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
    public interface IDichVuPhongService
    {
        //PagedResult<DichVuPhongModelView> GetAllPaging(DichVuViewModelSearch DichVuPhongModelViewSearch);
        DichVuPhongModelView GetByid(int id);
        void Add(DichVuPhongModelView view);
        bool Update(DichVuPhongModelView view);
        bool Deleted(int id);
        void DeletedByMaPhong(int maPhong);
        void Save();
        List<DichVuPhongModelView> GetDichVuByPhong(int maPhong);
        DichVuPhongModelView GetDichVuByPhong(int maDichVu, int maPhong);
    }

    public class DichVuPhongService : IDichVuPhongService
    {
        private readonly IDichVuPhongRepository _dichVuPhongRepository;
        private IUnitOfWork _unitOfWork;
        public DichVuPhongService(IDichVuPhongRepository dichVuPhongRepository,
            IUnitOfWork unitOfWork)
        {
            _dichVuPhongRepository = dichVuPhongRepository;
            _unitOfWork = unitOfWork;
        }       
        public DichVuPhongModelView GetByid(int id)
        {
            var data = _dichVuPhongRepository.FindAll(p => p.Id == id).FirstOrDefault();
            if (data != null)
            {
                var model = new DichVuPhongModelView()
                {
                    Id = data.Id,
                    MaDV = data.MaDV,
                    MaPhong = data.MaPhong,
                    SoLuong = data.SoLuong,
                };
                return model;
            }
            return null;
        }
     
        public void Add(DichVuPhongModelView view)
        {
            try
            {
                if (view != null)
                {
                    var dichVuPhong = new DichVuPhong
                    {
                        MaDV = view.MaDV,
                        MaPhong = view.MaPhong,     
                        SoLuong = view.SoLuong
                    };
                    _dichVuPhongRepository.Add(dichVuPhong);
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
        public bool Update(DichVuPhongModelView view)
        {
            try
            {
                var dataServer = _dichVuPhongRepository.FindById(view.Id);
                if (dataServer != null)
                {
                    dataServer.MaDV = view.MaDV;
                    dataServer.MaPhong = view.MaPhong;
                    dataServer.SoLuong = view.SoLuong;
                    _dichVuPhongRepository.Update(dataServer);
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
                var dataServer = _dichVuPhongRepository.FindById(id);
                if (dataServer != null)
                {
                    _dichVuPhongRepository.Remove(dataServer);
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
                var dichVuPhongByMaPhong = _dichVuPhongRepository.FindAll(d=>d.MaPhong == maPhong).ToList();
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
        //public void void DeletedAll(List<DichVuPhongModelView> DichVuPhongModelViews)
        //{
        //    _dichVuPhongRepository.re
        //}
        public List<DichVuPhongModelView> GetDichVuByPhong(int maPhong)
        {
            var data = _dichVuPhongRepository.FindAll(p => p.MaPhong == maPhong).Select(p=>new DichVuPhongModelView()
            {
                Id = p.Id,
                MaDV = p.MaDV,
                MaPhong = p.MaPhong,
                SoLuong = p.SoLuong
            }).ToList();
            return data;
        }
        public DichVuPhongModelView GetDichVuByPhong(int maDichVu,int maPhong)
        {
            var data = _dichVuPhongRepository.FindAll(p => p.MaPhong == maPhong && p.MaDV == maDichVu).Select(p => new DichVuPhongModelView()
            {
                Id = p.Id,
                MaDV = p.MaDV,
                MaPhong = p.MaPhong,
                SoLuong = p.SoLuong
            }).FirstOrDefault();
            return data;
        }
    }
}
