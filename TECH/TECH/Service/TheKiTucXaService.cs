
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
    public interface ITheKiTucXaService
    {
        PagedResult<TheKiTucXaModelView> GetAllPaging(DichVuViewModelSearch TheKiTucXaModelViewSearch);
        TheKiTucXaModelView GetByid(int id);
        //List<TheKiTucXaModelView> GetAll();
        void Add(TheKiTucXaModelView view);
        bool Update(TheKiTucXaModelView view);
        bool Deleted(int id);
        void Save();
        int GetCount();
        bool IsExist(string tenDichVu);
    }

    public class TheKiTucXaService : ITheKiTucXaService
    {
        private readonly ITheKiTucXaRepository _theKiTucXaRepository;
        private IUnitOfWork _unitOfWork;
        public TheKiTucXaService(ITheKiTucXaRepository theKiTucXaRepository,
            IUnitOfWork unitOfWork)
        {
            theKiTucXaRepository = _theKiTucXaRepository;
            _unitOfWork = unitOfWork;
        }
        public bool IsExist(string mathe)
        {
            var data = _theKiTucXaRepository.FindAll().Any(p => p.MaThe == mathe);
            return data;
        }
        public TheKiTucXaModelView GetByid(int id)
        {
            var data = _theKiTucXaRepository.FindAll(p => p.Id == id).FirstOrDefault();
            if (data != null)
            {
                var model = new TheKiTucXaModelView()
                {
                    Id = data.Id,
                    MaThe = data.MaThe,
                    MaSinhVien = data.MaSinhVien,
                    NgayBatDau = data.NgayBatDau,
                    NgayBatDauSrc = data.NgayBatDau.HasValue ? data.NgayBatDau.Value.ToString("dd-MM-yyyy") : "",
                    NgayKetThuc = data.NgayKetThuc,
                    NgayKetThucSrc = data.NgayKetThuc.HasValue ? data.NgayKetThuc.Value.ToString("dd-MM-yyyy") : "",
                    Status = data.Status,
                };
                return model;
            }
            return null;
        }
        public int GetCount()
        {
            int count = 0;
            count = _theKiTucXaRepository.FindAll().Count();
            return count;
        }
        public void Add(TheKiTucXaModelView view)
        {
            try
            {
                if (view != null)
                {
                    var nhanvien = new TheKiTucXa
                    {
                        MaSinhVien = view.MaSinhVien,
                        MaThe = view.MaThe,
                        NgayBatDau = view.NgayBatDau,
                        NgayKetThuc = view.NgayKetThuc,
                        Status = view.Status
                    };
                    _theKiTucXaRepository.Add(nhanvien);
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
        public bool Update(TheKiTucXaModelView view)
        {
            try
            {
                var dataServer = _theKiTucXaRepository.FindById(view.Id);
                if (dataServer != null)
                {
                    if (dataServer.MaThe.ToLower().Trim() != view.MaThe.ToLower().Trim())
                    {
                        if (IsExist(view.MaThe))
                        {
                            return false;
                        }
                    }
                    dataServer.MaSinhVien = view.MaSinhVien;
                    dataServer.NgayKetThuc = view.NgayKetThuc;
                    dataServer.NgayBatDau = view.NgayBatDau;
                    dataServer.MaThe = view.MaThe;
                    _theKiTucXaRepository.Update(dataServer);
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
                var dataServer = _theKiTucXaRepository.FindById(id);
                if (dataServer != null)
                {
                    _theKiTucXaRepository.Remove(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }
        //public List<TheKiTucXaModelView> GetAll()
        //{
        //    var data = _theKiTucXaRepository.FindAll().OrderByDescending(p => p.Id).Select(c => new TheKiTucXaModelView()
        //    {
        //        Id = c.Id,
        //        TenDV = !string.IsNullOrEmpty(c.TenDV) ? c.TenDV : "",
        //        GhiChu = !string.IsNullOrEmpty(c.GhiChu) ? c.GhiChu : "",
        //        DonGia = c.DonGia,
        //        LoaiDV = c.LoaiDV,
        //        LoaiDVStr = Common.GetDichVu(c.LoaiDV.HasValue ? c.LoaiDV.Value : 5),
        //        DonGiaStr = c.DonGia.HasValue && c.DonGia.Value > 0 ? c.DonGia.Value.ToString("#,###") : ""
        //    }).ToList();
        //    return data;
        //}
        public PagedResult<TheKiTucXaModelView> GetAllPaging(DichVuViewModelSearch TheKiTucXaModelViewSearch)
        {
            try
            {
                var query = _theKiTucXaRepository.FindAll();

                if (!string.IsNullOrEmpty(TheKiTucXaModelViewSearch.name))
                {
                    query = query.Where(c => c.MaThe.ToLower().Trim().Contains(TheKiTucXaModelViewSearch.name.ToLower().Trim()));
                }
                //if (TheKiTucXaModelViewSearch.loaiDV.HasValue)
                //{
                //    query = query.Where(c => c.LoaiDV == TheKiTucXaModelViewSearch.loaiDV);
                //}

                int totalRow = query.Count();
                query = query.Skip((TheKiTucXaModelViewSearch.PageIndex - 1) * TheKiTucXaModelViewSearch.PageSize).Take(TheKiTucXaModelViewSearch.PageSize);
                var data = query.OrderByDescending(p => p.Id).Select(c => new TheKiTucXaModelView()
                {
                    Id = c.Id,
                    MaThe = !string.IsNullOrEmpty(c.MaThe) ? c.MaThe : "",
                    MaSinhVien = c.MaSinhVien,
                    NgayBatDau = c.NgayBatDau,
                    NgayBatDauSrc = c.NgayBatDau.HasValue ? c.NgayBatDau.Value.ToString("dd-MM-yyyy") : "",
                    NgayKetThuc = c.NgayKetThuc,
                    NgayKetThucSrc = c.NgayKetThuc.HasValue ? c.NgayKetThuc.Value.ToString("dd-MM-yyyy") : "",
                    Status = c.Status,
                }).ToList();

                var pagingData = new PagedResult<TheKiTucXaModelView>
                {
                    Results = data,
                    CurrentPage = TheKiTucXaModelViewSearch.PageIndex,
                    PageSize = TheKiTucXaModelViewSearch.PageSize,
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
