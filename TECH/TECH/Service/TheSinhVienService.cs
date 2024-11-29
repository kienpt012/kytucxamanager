
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
    public interface ITheSinhVienService
    {
        PagedResult<TheSinhVienModelView> GetAllPaging(TheSinhVienViewModelSearch TheSinhVienModelViewSearch);
        TheSinhVienModelView GetByid(int id);
        List<TheSinhVienModelView> GetAll();
        void Add(TheSinhVienModelView view);
        bool Update(TheSinhVienModelView view);
        bool Deleted(int id);
        void Save();
        int GetCount();
        bool IsExist(string tenDichVu);
    }

    public class TheSinhVienService : ITheSinhVienService
    {
        private readonly ITheSinhVienRepository _theSinhVienRepository;
        private IUnitOfWork _unitOfWork;
        public TheSinhVienService(ITheSinhVienRepository theSinhVienRepository,
            IUnitOfWork unitOfWork)
        {
            _theSinhVienRepository = theSinhVienRepository;
            _unitOfWork = unitOfWork;
        }
        public bool IsExist(string tenDichVu)
        {
            var data = _theSinhVienRepository.FindAll().Any(p => p.MaThe == tenDichVu);
            return data;
        }
        public TheSinhVienModelView GetByid(int id)
        {
            var data = _theSinhVienRepository.FindAll(p => p.Id == id).FirstOrDefault();
            if (data != null)
            {
                var model = new TheSinhVienModelView()
                {
                    Id = data.Id,
                    UserId = data.UserId,
                    MaThe = data.MaThe,
                    NgayTaoThe = data.NgayTaoThe,
                    NgayHetHan = data.NgayHetHan,
                    Status = data.Status,
                    Comment = data.Comment,
                    NgayTaoTheStr = data.NgayTaoThe.HasValue ? data.NgayTaoThe.Value.ToString("dd/MM/yyyy") : "",
                    NgayHetHanStr = data.NgayHetHan.HasValue ? data.NgayHetHan.Value.ToString("dd/MM/yyyy") : "",
                };
                return model;
            }
            return null;
        }
        public int GetCount()
        {
            int count = 0;
            count = _theSinhVienRepository.FindAll().Count();
            return count;
        }
        public void Add(TheSinhVienModelView view)
        {
            try
            {
                if (view != null)
                {
                    var nhanvien = new TheSinhVien
                    {
                        UserId = view.UserId,
                        MaThe = view.MaThe,
                        NgayTaoThe = view.NgayTaoThe,
                        NgayHetHan = view.NgayHetHan,
                        Status = view.Status,
                        Comment = view.Comment,
                    };
                    _theSinhVienRepository.Add(nhanvien);
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
        public bool Update(TheSinhVienModelView view)
        {
            try
            {
                var dataServer = _theSinhVienRepository.FindById(view.Id);
                if (dataServer != null)
                {
                    if (dataServer.MaThe.ToLower().Trim() != view.MaThe.ToLower().Trim())
                    {
                        if (IsExist(view.MaThe))
                        {
                            return false;
                        }
                    }

                    dataServer.UserId = view.UserId;
                    dataServer.MaThe = view.MaThe;
                    dataServer.NgayTaoThe = view.NgayTaoThe;
                    dataServer.NgayHetHan = view.NgayHetHan;
                    dataServer.Status = view.Status;
                    dataServer.Comment = view.Comment;
                    _theSinhVienRepository.Update(dataServer);
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
                var dataServer = _theSinhVienRepository.FindById(id);
                if (dataServer != null)
                {
                    _theSinhVienRepository.Remove(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }
        public List<TheSinhVienModelView> GetAll()
        {
            var data = _theSinhVienRepository.FindAll().OrderByDescending(p => p.Id).Select(c => new TheSinhVienModelView()
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
        public PagedResult<TheSinhVienModelView> GetAllPaging(TheSinhVienViewModelSearch TheSinhVienModelViewSearch)
        {
            try
            {
                var query = _theSinhVienRepository.FindAll();

                if (!string.IsNullOrEmpty(TheSinhVienModelViewSearch.name))
                {
                    query = query.Where(c => c.MaThe.ToLower().Trim().Contains(TheSinhVienModelViewSearch.name.ToLower().Trim()));
                }
                //if (TheSinhVienModelViewSearch.loaiDV.HasValue)
                //{
                //    query = query.Where(c => c.LoaiDV == TheSinhVienModelViewSearch.loaiDV);
                //}

                int totalRow = query.Count();
                query = query.Skip((TheSinhVienModelViewSearch.PageIndex - 1) * TheSinhVienModelViewSearch.PageSize).Take(TheSinhVienModelViewSearch.PageSize);
                var data = query.OrderByDescending(p => p.Id).Select(c => new TheSinhVienModelView()
                {
                    Id=c.Id,
                    UserId = c.UserId,
                    MaThe = c.MaThe,
                    NgayTaoThe = c.NgayTaoThe,
                    NgayHetHan = c.NgayHetHan,
                    NgayTaoTheStr = c.NgayTaoThe.HasValue ? c.NgayTaoThe.Value.ToString("dd/MM/yyyy") : "",
                    NgayHetHanStr = c.NgayHetHan.HasValue ? c.NgayHetHan.Value.ToString("dd/MM/yyyy") : "",
                    Status = c.Status,
                    StatusStr = c.Status.HasValue && c.Status.Value > 0  && c.Status.Value == 1?"Đang hoạt động":"Đã hủy", 
                    Comment = !string.IsNullOrEmpty(c.Comment)? c.Comment:"",
                }).ToList();

                var pagingData = new PagedResult<TheSinhVienModelView>
                {
                    Results = data,
                    CurrentPage = TheSinhVienModelViewSearch.PageIndex,
                    PageSize = TheSinhVienModelViewSearch.PageSize,
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
