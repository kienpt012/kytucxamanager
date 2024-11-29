
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
    public interface IDichVuService
    {
        PagedResult<DichVuModelView> GetAllPaging(DichVuViewModelSearch DichVuModelViewSearch);
        DichVuModelView GetByid(int id);
        List<DichVuModelView> GetAll();
        void Add(DichVuModelView view);
        bool Update(DichVuModelView view);
        bool Deleted(int id);
        void Save();
        int GetCount();
        bool IsExist(string tenDichVu);
    }

    public class DichVuService : IDichVuService
    {
        private readonly IDichVuRepository _dichvuRepository;
        private IUnitOfWork _unitOfWork;
        public DichVuService(IDichVuRepository dichVuRepository,
            IUnitOfWork unitOfWork)
        {
            _dichvuRepository = dichVuRepository;
            _unitOfWork = unitOfWork;
        }
        public bool IsExist(string tenDichVu)
        {
            var data = _dichvuRepository.FindAll().Any(p => p.TenDV == tenDichVu);
            return data;
        }
        public DichVuModelView GetByid(int id)
        {
            var data = _dichvuRepository.FindAll(p => p.Id == id).FirstOrDefault();
            if (data != null)
            {
                var model = new DichVuModelView()
                {
                    Id = data.Id,
                    TenDV = data.TenDV,
                    DonGia = data.DonGia,
                    GhiChu = data.GhiChu,
                    LoaiDV = data.LoaiDV,
                    DonGiaStr = data.DonGia.HasValue && data.DonGia.Value > 0? data.DonGia.Value.ToString("#,###"):""
                };
                return model;
            }
            return null;
        }
        public int GetCount()
        {
            int count = 0;
            count = _dichvuRepository.FindAll().Count();
            return count;
        }
        public void Add(DichVuModelView view)
        {
            try
            {
                if (view != null)
                {
                    var nhanvien = new DichVu
                    {
                        TenDV = view.TenDV,
                        DonGia = view.DonGia,
                        LoaiDV = view.LoaiDV,
                        GhiChu = view.GhiChu                    
                    };
                    _dichvuRepository.Add(nhanvien);
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
        public bool Update(DichVuModelView view)
        {
            try
            {
                var dataServer = _dichvuRepository.FindById(view.Id);
                if (dataServer != null)
                {
                    if (dataServer.TenDV.ToLower().Trim() != view.TenDV.ToLower().Trim())
                    {
                        if (IsExist(view.TenDV))
                        {
                            return false;
                        }
                    }
                    dataServer.TenDV = view.TenDV;
                    dataServer.DonGia = view.DonGia;
                    dataServer.GhiChu = view.GhiChu;
                    dataServer.LoaiDV = view.LoaiDV;
                    _dichvuRepository.Update(dataServer);
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
                var dataServer = _dichvuRepository.FindById(id);
                if (dataServer != null)
                {
                    _dichvuRepository.Remove(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }
        public List<DichVuModelView> GetAll()
        {
            var data = _dichvuRepository.FindAll().OrderByDescending(p => p.Id).Select(c => new DichVuModelView()
            {
                Id = c.Id,
                TenDV = !string.IsNullOrEmpty(c.TenDV) ? c.TenDV : "",
                GhiChu = !string.IsNullOrEmpty(c.GhiChu) ? c.GhiChu : "",
                DonGia = c.DonGia,
                LoaiDV = c.LoaiDV,
                LoaiDVStr = Common.GetDichVu(c.LoaiDV.HasValue ? c.LoaiDV.Value : 5),
                DonGiaStr = c.DonGia.HasValue && c.DonGia.Value > 0 ? c.DonGia.Value.ToString("#,###") : ""
            }).ToList();
            return data;
        }
        public PagedResult<DichVuModelView> GetAllPaging(DichVuViewModelSearch DichVuModelViewSearch)
        {
            try
            {
                var query = _dichvuRepository.FindAll();

                if (!string.IsNullOrEmpty(DichVuModelViewSearch.name))
                {
                    query = query.Where(c => c.TenDV.ToLower().Trim().Contains(DichVuModelViewSearch.name.ToLower().Trim()));
                }
                if (DichVuModelViewSearch.loaiDV.HasValue)
                {
                    query = query.Where(c => c.LoaiDV == DichVuModelViewSearch.loaiDV);
                }

                int totalRow = query.Count();
                query = query.Skip((DichVuModelViewSearch.PageIndex - 1) * DichVuModelViewSearch.PageSize).Take(DichVuModelViewSearch.PageSize);
                var data = query.OrderByDescending(p => p.Id).Select(c => new DichVuModelView()
                {
                    Id = c.Id,
                    TenDV = !string.IsNullOrEmpty(c.TenDV) ? c.TenDV : "",
                    GhiChu = !string.IsNullOrEmpty(c.GhiChu) ? c.GhiChu : "",
                    DonGia = c.DonGia,
                    LoaiDV = c.LoaiDV,
                    LoaiDVStr = Common.GetDichVu(c.LoaiDV.HasValue ? c.LoaiDV.Value: 5),
                    DonGiaStr = c.DonGia.HasValue && c.DonGia.Value > 0? c.DonGia.Value.ToString("#,###"):""                
                }).ToList();

                var pagingData = new PagedResult<DichVuModelView>
                {
                    Results = data,
                    CurrentPage = DichVuModelViewSearch.PageIndex,
                    PageSize = DichVuModelViewSearch.PageSize,
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
