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
    public interface IHoaDonService
    {
        PagedResult<HoaDonModelView> GetAllPaging(HoaDonViewModelSearch HoaDonModelViewSearch);
        HoaDonModelView GetByid(int id);        
        void Add(HoaDonModelView view);
        bool Update(HoaDonModelView view);
        void PayBill(HoaDonModelView view);
        bool Deleted(int id);
        void Save();
        int GetCount();
        Dictionary<string, decimal> GetDoanhThu();
        List<HoaDonModelView> GetListHoaDons();
        List<HoaDonModelView> GetAllHoaDons(DateTime? month = null);

        Dictionary<string, HoaDonModelViewStatistical> GetHoaDonStatistical();
    }

    public class HoaDonService : IHoaDonService
    {
        private readonly IHoaDonRepository _hoaDonRepository;
        private IUnitOfWork _unitOfWork;
        public HoaDonService(IHoaDonRepository hoaDonRepository,
            IUnitOfWork unitOfWork)
        {
            _hoaDonRepository = hoaDonRepository;
            _unitOfWork = unitOfWork;
        }
        public void PayBill(HoaDonModelView view)
        {
            var dataServer = _hoaDonRepository.FindById(view.Id);
            if (dataServer != null)
            {
                dataServer.Id = view.Id;
                dataServer.NguoiDong = view.NguoiDong;
                dataServer.NgayDongTien = DateTime.Now;
                dataServer.TienDong = view.TienDong;
                dataServer.TrangThai = view.TrangThai;
                //dataServer.HanDong = view.HanDong;
                dataServer.GhiChu = view.GhiChu;
                _hoaDonRepository.Update(dataServer);
                Save();
            }
        }
        public List<HoaDonModelView> GetAllHoaDons(DateTime? month = null)
        {
            var query = _hoaDonRepository.FindAll();

            // Nếu có truyền vào month thì lọc theo tháng
            if (month.HasValue)
            {
                query = query.Where(hd => hd.HanDong.HasValue &&
                                          hd.HanDong.Value.Year == month.Value.Year &&
                                          hd.HanDong.Value.Month == month.Value.Month);
            }

            var result = query.OrderByDescending(hd => hd.HanDong).Select(hd => new HoaDonModelView()
            {
                Id = hd.Id,
                MaHopDong = hd.MaHopDong,
                NguoiDong = hd.NguoiDong,
                NgayDongTien = hd.NgayDongTien,
                TienDong = hd.TienDong,
                TongTien = hd.TongTien,
                GhiChu = hd.GhiChu,
                TrangThai = hd.TrangThai,
                HanDongStr = hd.HanDong.HasValue ? hd.HanDong.Value.ToString("MM/yyyy") : "",
                TrangThaiStr = hd.TrangThai.HasValue && hd.TrangThai.Value > 0 ? Common.GetTrangThaiHoaDon(hd.TrangThai.Value) : ""
            }).ToList();

            return result;
        }

        public HoaDonModelView GetByid(int id)
        {
            var data = _hoaDonRepository.FindAll(p => p.Id == id).FirstOrDefault();
            if (data != null)
            {
                var model = new HoaDonModelView()
                {
                    Id = data.Id,
                    MaHopDong = data.MaHopDong,
                    NguoiDong = data.NguoiDong,
                    NgayDongTien = data.NgayDongTien,
                    TienDong = data.TienDong,
                    TongTien = data.TongTien,
                    GhiChu=data.GhiChu,
                    TrangThai = data.TrangThai,
                    HanDongStr = data.HanDong.HasValue ? data.HanDong.Value.ToString("dd-MM-yyyy") : "",
                    TrangThaiStr = data.TrangThai.HasValue && data.TrangThai.Value > 0 ? Common.GetTrangThaiHoaDon(data.TrangThai.Value) : ""
                };
                return model;
            }
            return null;
        }
        //public List<HoaDonModelView> GetPhongByNha(int id)
        //{
        //    if (id > 0)
        //    {
        //        var data = _hoaDonRepository.FindAll(p => p.Id == id).Select(c => new HoaDonModelView()
        //        {
        //            Id = c.Id,
        //            MaNha = c.MaNha,
        //            TenPhong = c.TenPhong,
        //            DonGia = c.DonGia,
        //            SLNguoiMax = c.SLNguoiMax,
        //            ChieuDai = c.ChieuDai,
        //            ChieuRong = c.ChieuRong,
        //            MoTa = c.MoTa,
        //            LoaiPhong = c.LoaiPhong,
        //            TinhTrang = c.TinhTrang,
        //            TinhTrangStr = Common.GetTinhTrangPhong(c.TinhTrang.Value)
        //        }).ToList();

        //        return data;
        //    }
        //    return null;
        //}
        public int GetCount()
        {
            int count = 0;
            count = _hoaDonRepository.FindAll().Count();
            return count;
        }
        public void Add(HoaDonModelView view)
        {
            try
            {
                if (view != null)
                {
                    var hopdong = new HoaDon
                    {
                        Id = view.Id,
                        MaHopDong = view.MaHopDong,
                        NguoiDong = view.NguoiDong,
                        NgayDongTien = view.NgayDongTien,
                        TrangThai = view.TrangThai,
                        HanDong = view.HanDong,
                        GhiChu = view.GhiChu
                    };
                    _hoaDonRepository.Add(hopdong);
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
        public bool Update(HoaDonModelView view)
        {
            try
            {
                var dataServer = _hoaDonRepository.FindById(view.Id);
                if (dataServer != null)
                {
                    dataServer.Id = view.Id;
                    dataServer.MaHopDong = view.MaHopDong;
                    dataServer.NguoiDong = view.NguoiDong;
                    dataServer.NgayDongTien = view.NgayDongTien;
                    dataServer.TrangThai = view.TrangThai;
                    dataServer.HanDong = view.HanDong;
                    dataServer.GhiChu = view.GhiChu;
                    _hoaDonRepository.Update(dataServer);
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
                var dataServer = _hoaDonRepository.FindById(id);
                if (dataServer != null)
                {
                    _hoaDonRepository.Remove(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }
        public Dictionary<string, decimal> GetDoanhThu()
        {
            var doanThuThang = new Dictionary<string, decimal>();
            var query = _hoaDonRepository.FindAll();
            var data = query.OrderByDescending(p => p.HanDong).Select(c => new HoaDonModelView()
            {
                Id = c.Id,
                MaHopDong = c.MaHopDong,
                NguoiDong = !string.IsNullOrEmpty(c.NguoiDong) ? c.NguoiDong : "",
                NgayDongTien = c.NgayDongTien,
                HanDong = c.HanDong,
                TienDong = c.TienDong,
                NgayDongTienStr = c.NgayDongTien.HasValue ? c.NgayDongTien.Value.ToString("dd-MM-yyyy") : "",
                HanDongStr = c.HanDong.HasValue ? c.HanDong.Value.ToString("MM/yyyy") : "",
                TrangThaiStr = c.TrangThai.HasValue && c.TrangThai.Value > 0 ? Common.GetTrangThaiHoaDon(c.TrangThai.Value) : "",
                TrangThai = c.TrangThai
            }).ToList();
            if (data != null && data.Count > 0)
            {
                var dataKeyList = data.GroupBy(p => p.HanDong).ToList();
                foreach (var item in dataKeyList)
                {
                    string datestr = item.Key.Value.ToString("MM/yyyy");
                    if (item != null && item.Count() > 0)
                    {
                        decimal total = item.Sum(p=>p.TongTien.Value);
                        doanThuThang.Add(datestr, total);
                    }
                    
                }
            }
            return doanThuThang;
        }
        public List<HoaDonModelView> GetListHoaDons()
        {
            var query = _hoaDonRepository.FindAll().OrderByDescending(p => p.Id).Select(c => new HoaDonModelView()
            {
                Id = c.Id,
                MaHopDong = c.MaHopDong,
                NguoiDong = !string.IsNullOrEmpty(c.NguoiDong) ? c.NguoiDong : "",
                NgayDongTien = c.NgayDongTien,
                HanDong = c.HanDong,
                TienDong = c.TienDong,
                NgayDongTienStr = c.NgayDongTien.HasValue ? c.NgayDongTien.Value.ToString("dd-MM-yyyy") : "",
                HanDongStr = c.HanDong.HasValue ? c.HanDong.Value.ToString("dd-MM-yyyy") : "",
                TrangThaiStr = c.TrangThai.HasValue && c.TrangThai.Value > 0 ? Common.GetTrangThaiHoaDon(c.TrangThai.Value) : "",
                TrangThai = c.TrangThai
            }).ToList();
            return query;
        }
        
        public PagedResult<HoaDonModelView> GetAllPaging(HoaDonViewModelSearch HoaDonModelViewSearch)
        {
            try
            {
                var query = _hoaDonRepository.FindAll();

                int totalRow = query.Count();
                query = query.Skip((HoaDonModelViewSearch.PageIndex - 1) * HoaDonModelViewSearch.PageSize).Take(HoaDonModelViewSearch.PageSize);
                var data = query.OrderByDescending(p => p.Id).Select(c => new HoaDonModelView()
                {
                    Id = c.Id,
                    MaHopDong = c.MaHopDong,
                    NguoiDong = !string.IsNullOrEmpty(c.NguoiDong)? c.NguoiDong:"",
                    NgayDongTien = c.NgayDongTien,
                    HanDong = c.HanDong,
                    TienDong = c.TienDong,
                    NgayDongTienStr = c.NgayDongTien.HasValue ? c.NgayDongTien.Value.ToString("dd-MM-yyyy") : "",
                    HanDongStr = c.HanDong.HasValue ? c.HanDong.Value.ToString("dd-MM-yyyy") : "",
                    TrangThaiStr = c.TrangThai.HasValue && c.TrangThai.Value > 0 ?  Common.GetTrangThaiHoaDon(c.TrangThai.Value):"",
                    TrangThai = c.TrangThai
                }).ToList();

                var pagingData = new PagedResult<HoaDonModelView>
                {
                    Results = data,
                    CurrentPage = HoaDonModelViewSearch.PageIndex,
                    PageSize = HoaDonModelViewSearch.PageSize,
                    RowCount = totalRow,
                };
                return pagingData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public Dictionary<string, HoaDonModelViewStatistical> GetHoaDonStatistical()
        {

            var data = _hoaDonRepository.FindAll().Select(o => new HoaDonModelView()
            {
                HanDong = o.HanDong,
                HanDongStr = o.HanDong.HasValue ? o.HanDong.Value.ToString("MM/yyyy") : "",
                TrangThai = o.TrangThai
            }).ToList();
            if (data != null && data.Count > 0)
            {
                var dataConvert = data.GroupBy(p => p.HanDongStr);
                var HoaDonStatistical = new Dictionary<string, HoaDonModelViewStatistical>();
                if (dataConvert != null)
                {
                    foreach (var item in dataConvert)
                    {
                        var orderStatistical = new HoaDonModelViewStatistical();
                        orderStatistical.TotalDaDong = item.Where(p => p.TrangThai == 1).Count(); // đã đóng
                        orderStatistical.TotalChuaDong = item.Where(p => p.TrangThai == 2).Count(); // chưa đóng
                        orderStatistical.TotalConNo = item.Where(p => p.TrangThai == 3).Count(); // còn nợ
                        HoaDonStatistical.Add(item.Key, orderStatistical);
                    }
                }
                return HoaDonStatistical;
            }
            return null;
        }
    }
}
