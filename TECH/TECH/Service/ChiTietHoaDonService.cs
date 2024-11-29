
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
    public interface IChiTietHoaDonService
    {
        //PagedResult<HoaDonModelView> GetAllPaging(HoaDonViewModelSearch HoaDonModelViewSearch);
        ChiTietHoaDonModelView GetByid(int id);
        List<ChiTietHoaDonModelView> GetChiTietHoaDonByMaHoaDon(int maHoaDon);
        void Add(ChiTietHoaDonModelView view);
        void AddLoi(int maHoaDon,int maLoi);
        bool Update(ChiTietHoaDonModelView view);
        bool Deleted(int id);
        void Deleted(int maHoaDon,int maDichVu);
        void Save();
        
    }

    public class ChiTietHoaDonService : IChiTietHoaDonService
    {
        private readonly IChiTietHoaDonRepository _chiTietHoaDonRepository;
        private IUnitOfWork _unitOfWork;
        public ChiTietHoaDonService(IChiTietHoaDonRepository chiTietHoaDonRepository,
            IUnitOfWork unitOfWork)
        {
            _chiTietHoaDonRepository = chiTietHoaDonRepository;
            _unitOfWork = unitOfWork;
        }
        public List<ChiTietHoaDonModelView> GetChiTietHoaDonByMaHoaDon(int maHoaDon)
        {
            var data = _chiTietHoaDonRepository.FindAll(p => p.MaHoaDon == maHoaDon).Select(p => new ChiTietHoaDonModelView()
            {
                Id = p.Id,
                MaHoaDon = p.MaHoaDon,
                MaDV = p.MaDV,
                MaLoi = p.MaLoi,
                ChiSoMoi = p.ChiSoMoi,
                ChiSoDung = p.ChiSoDung,
                ThanhTienStr = p.ThanhTien.HasValue && p.ThanhTien.Value > 0 ? p.ThanhTien.Value.ToString("#,###") : ""
            }).ToList();

            return data;
        }

        public void Deleted(int maHoaDon, int maDichVu)
        {
            var data = _chiTietHoaDonRepository.FindAll(p => p.MaHoaDon == maHoaDon && p.MaDV == maDichVu).FirstOrDefault();
            if (data != null)
            {
                _chiTietHoaDonRepository.Remove(data);
                Save();
            }
        }
        public ChiTietHoaDonModelView GetByid(int id)
        {
            var data = _chiTietHoaDonRepository.FindAll(p => p.Id == id).FirstOrDefault();
            if (data != null)
            {
                var model = new ChiTietHoaDonModelView()
                {
                    Id = data.Id,
                    MaHoaDon = data.MaHoaDon,
                    MaDV = data.MaDV,
                    MaLoi = data.MaLoi,
                    ChiSoMoi = data.ChiSoMoi,
                    ChiSoDung = data.ChiSoDung,
                    ThanhTien = data.ThanhTien,
                    ThanhTienStr = data.ThanhTien.HasValue && data.ThanhTien.Value > 0 ? data.ThanhTien.Value.ToString("#,###"):""
                };
                return model;
            }
            return null;
        }
        
        public void Add(ChiTietHoaDonModelView view)
        {
            try
            {
                if (view != null)
                {
                    var hopdong = new ChiTietHoaDon
                    {                        
                        MaHoaDon = view.MaHoaDon,
                        MaDV = view.MaDV,
                        MaLoi = view.MaLoi,
                        ChiSoCu = view.ChiSoCu,
                        ChiSoMoi = view.ChiSoMoi,
                        ChiSoDung = view.ChiSoDung,
                        ThanhTien = view.ThanhTien
                    };
                    _chiTietHoaDonRepository.Add(hopdong);
                }
            }
            catch (Exception ex)
            {
            }
        }
        public void AddLoi(int maHoaDon, int maLoi)
        {
            var chiTietHoaDon = new ChiTietHoaDon
            {
                MaHoaDon = maHoaDon,
                MaLoi = maLoi
            };
            _chiTietHoaDonRepository.Add(chiTietHoaDon);
        }
        public void Save()
        {
            _unitOfWork.Commit();
        }
        public bool Update(ChiTietHoaDonModelView view)
        {
            try
            {
                var dataServer = _chiTietHoaDonRepository.FindById(view.Id);
                if (dataServer != null)
                {
                    // Cập nhật ChiSoCu trước khi thay đổi ChiSoMoi
                    dataServer.ChiSoCu = dataServer.ChiSoMoi;

                    // Ghi đè giá trị mới
                    dataServer.ChiSoMoi = view.ChiSoMoi;

                    // Các cập nhật khác
                    dataServer.MaHoaDon = view.MaHoaDon;
                    dataServer.MaDV = view.MaDV;
                    dataServer.MaLoi = view.MaLoi;
                    dataServer.ChiSoDung = view.ChiSoDung;
                    dataServer.ThanhTien = view.ThanhTien;

                    // Lưu thay đổi vào cơ sở dữ liệu
                    _chiTietHoaDonRepository.Update(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                return false;
            }

            return false;
        }
        public bool Deleted(int id)
        {
            try
            {
                var dataServer = _chiTietHoaDonRepository.FindById(id);
                if (dataServer != null)
                {
                    _chiTietHoaDonRepository.Remove(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }
        //public PagedResult<HoaDonModelView> GetAllPaging(HoaDonViewModelSearch HoaDonModelViewSearch)
        //{
        //    try
        //    {
        //        var query = _chiTietHoaDonRepository.FindAll();

        //        int totalRow = query.Count();
        //        query = query.Skip((HoaDonModelViewSearch.PageIndex - 1) * HoaDonModelViewSearch.PageSize).Take(HoaDonModelViewSearch.PageSize);
        //        var data = query.OrderByDescending(p => p.Id).Select(c => new HoaDonModelView()
        //        {
        //            Id = c.Id,
        //            MaHopDong = c.MaHopDong,
        //            NguoiDong = !string.IsNullOrEmpty(c.NguoiDong) ? c.NguoiDong : "",
        //            NgayDongTien = c.NgayDongTien,
        //            HanDong = c.HanDong,
        //            NgayDongTienStr = c.NgayDongTien.HasValue ? c.NgayDongTien.Value.ToString("dd-MM-yyyy") : "",
        //            HanDongStr = c.HanDong.HasValue ? c.HanDong.Value.ToString("dd-MM-yyyy") : "",
        //            TrangThaiStr = c.TrangThai.HasValue && c.TrangThai.Value > 0 ? Common.GetTrangThaiHoaDon(c.TrangThai.Value) : "",
        //            TrangThai = c.TrangThai
        //        }).ToList();

        //        var pagingData = new PagedResult<HoaDonModelView>
        //        {
        //            Results = data,
        //            CurrentPage = HoaDonModelViewSearch.PageIndex,
        //            PageSize = HoaDonModelViewSearch.PageSize,
        //            RowCount = totalRow,
        //        };
        //        return pagingData;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
    }
}
