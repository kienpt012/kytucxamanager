
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
    public interface ISuaChuaService
    {
        PagedResult<SuaChuaModelView> GetAllPaging(SuaChuaViewModelSearch SuaChuaModelViewSearch);
        SuaChuaModelView GetByid(int id);
        List<SuaChuaModelView> GetAll();
        void Add(SuaChuaModelView view);
        bool Update(SuaChuaModelView view);
        bool Deleted(int id);
        void Save();
        int GetCount();
    }

    public class SuaChuaService : ISuaChuaService
    {
        private readonly ISuaChuaRepository _suaChuaRepository;
        private IUnitOfWork _unitOfWork;
        public SuaChuaService(ISuaChuaRepository suaChuaRepository,
            IUnitOfWork unitOfWork)
        {
            _suaChuaRepository = suaChuaRepository;
            _unitOfWork = unitOfWork;
        }
        //public bool IsExist(string tenDichVu)
        //{
        //    var data = _suaChuaRepository.FindAll().Any(p => p.MaThe == tenDichVu);
        //    return data;
        //}
        public SuaChuaModelView GetByid(int id)
        {
            var data = _suaChuaRepository.FindAll(p => p.Id == id).FirstOrDefault();
            if (data != null)
            {
                var model = new SuaChuaModelView()
                {
                    Id = data.Id,
                    UserId = data.UserId,
                    MaPhong = data.MaPhong,
                    NgayTao = data.NgayTao,
                    Status = data.Status,
                    Comment = data.Comment,
                    NgayTaoStr = data.NgayTao.HasValue ? data.NgayTao.Value.ToString("dd/MM/yyyy") : "",
                };
                return model;
            }
            return null;
        }
        public int GetCount()
        {
            int count = 0;
            count = _suaChuaRepository.FindAll().Count();
            return count;
        }
        public void Add(SuaChuaModelView view)
        {
            try
            {
                if (view != null)
                {
                    var nhanvien = new SuaChua
                    {
                        UserId = view.UserId,
                        MaPhong = view.MaPhong,
                        NgayTao = DateTime.Now,
                        Status = view.Status,
                        Comment = view.Comment,
                    };
                    _suaChuaRepository.Add(nhanvien);
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
        public bool Update(SuaChuaModelView view)
        {
            try
            {
                var dataServer = _suaChuaRepository.FindById(view.Id);
                if (dataServer != null)
                {                 

                    dataServer.UserId = view.UserId;
                    dataServer.MaPhong = view.MaPhong;
                    dataServer.Status = view.Status;
                    dataServer.Comment = view.Comment;
                    _suaChuaRepository.Update(dataServer);
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
                var dataServer = _suaChuaRepository.FindById(id);
                if (dataServer != null)
                {
                    _suaChuaRepository.Remove(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }
        public List<SuaChuaModelView> GetAll()
        {
            var data = _suaChuaRepository.FindAll().OrderByDescending(p => p.Id).Select(c => new SuaChuaModelView()
            {
                Id = c.Id,
                //TenDV = !string.IsNullOrEmpty(c.TenDV) ? c.TenDV : "",
                //GhiChu = !string.IsNullOrEmpty(c.GhiChu) ? c.GhiChu : "",
                //DonGia = c.DonGia,
                //LoaiDV = c.LoaiDV,
                //LoaiDVStr = Common.GetDichVu(c.LoaiDV.HasValue ? c.LoaiDV.Value : 5),
                //DonGiaStr = c.DonGia.HasValue && c.DonGia.Value > 0 ? c.DonGia.Value.ToString("#,###") : ""
            }).ToList();
            return data;
        }
        public PagedResult<SuaChuaModelView> GetAllPaging(SuaChuaViewModelSearch SuaChuaModelViewSearch)
        {
            try
            {
                var query = _suaChuaRepository.FindAll();

                //if (!string.IsNullOrEmpty(SuaChuaModelViewSearch.name))
                //{
                //    query = query.Where(c => c.MaThe.ToLower().Trim().Contains(SuaChuaModelViewSearch.name.ToLower().Trim()));
                //}
                //if (SuaChuaModelViewSearch.loaiDV.HasValue)
                //{
                //    query = query.Where(c => c.LoaiDV == SuaChuaModelViewSearch.loaiDV);
                //}
                if (SuaChuaModelViewSearch != null && SuaChuaModelViewSearch.UserId.HasValue && SuaChuaModelViewSearch.UserId.Value > 0)
                {
                    query = query.Where(c => c.UserId == SuaChuaModelViewSearch.UserId.Value);
                }
                int totalRow = query.Count();
                query = query.Skip((SuaChuaModelViewSearch.PageIndex - 1) * SuaChuaModelViewSearch.PageSize).Take(SuaChuaModelViewSearch.PageSize);
                var data = query.OrderByDescending(p => p.Id).Select(c => new SuaChuaModelView()
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    MaPhong = c.MaPhong,
                    NgayTao = c.NgayTao,
                    NgayTaoStr = c.NgayTao.HasValue ? c.NgayTao.Value.ToString("dd/MM/yyyy") : "",
                    Status = c.Status,
                    StatusStr = Common.GetStatusSuaChua(c.Status.Value),
                    Comment = !string.IsNullOrEmpty(c.Comment) ? c.Comment : "",
                }).ToList();

                var pagingData = new PagedResult<SuaChuaModelView>
                {
                    Results = data,
                    CurrentPage = SuaChuaModelViewSearch.PageIndex,
                    PageSize = SuaChuaModelViewSearch.PageSize,
                    RowCount = totalRow,
                };
                return pagingData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
