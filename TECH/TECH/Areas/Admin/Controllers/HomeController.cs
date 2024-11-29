using Microsoft.AspNetCore.Mvc;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Service;

namespace TECH.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IDichVuService _dichVuService;
        private readonly INhaService _nhaService;
        private readonly IPhongService _phongService;
        private readonly IKhachHangService _khachHangService;
        private readonly IHoaDonService _hoaDonService;
        private readonly IHopDongService _hopDongService;
        private readonly IDichVuPhongService _dichVuPhongService;
        private readonly IChiTietHoaDonService _chiTietHoaDonService;
        private readonly ILoiPhamService _loiPhamService;
        public HomeController(
                IDichVuService dichVuService,
                INhaService nhaService,
                IPhongService phongService,
                IHoaDonService hoaDonService,
                IKhachHangService khachHangService,
                IHopDongService hopDongService,
                IDichVuPhongService dichVuPhongService,
                ILoiPhamService loiPhamService,
                IChiTietHoaDonService chiTietHoaDonService
            )
        {
            _dichVuService = dichVuService;
            _nhaService = nhaService;
            _phongService = phongService;
            _khachHangService = khachHangService;
            _hoaDonService = hoaDonService;
            _hopDongService = hopDongService;
            _dichVuPhongService = dichVuPhongService;
            _loiPhamService = loiPhamService;
            _chiTietHoaDonService = chiTietHoaDonService;
        }

        public IActionResult Index()
        {
            var home = new HomeModelView();
            home.DichVuCount = _dichVuService.GetCount();
            home.NhaCount = _nhaService.GetCount();
            home.PhongCount = _phongService.GetCount();
            home.KhachHangCount = _khachHangService.GetCount();
            return View(home);
        }
        [HttpGet]
        public JsonResult GetHoaDonStatistical()
        {
            var model = _hoaDonService.GetHoaDonStatistical();
            return Json(new
            {
                Data = model
            });
        }

        [HttpGet]
        public JsonResult GetDoanhThuTheoThang()
        {
            var model = _hoaDonService.GetListHoaDons();
            if (model != null && model.Count > 0)
            {
                foreach (var item in model)
                {
                    if (item.MaHopDong.HasValue && item.MaHopDong.Value > 0)
                    {
                        var hopDong = _hopDongService.GetByid(item.MaHopDong.Value);
                        if (hopDong != null)
                        {
                            if (hopDong.MaKH.HasValue && hopDong.MaKH.Value > 0)
                            {
                                hopDong.KhachHang = _khachHangService.GetByid(hopDong.MaKH.Value);
                            }
                            if (hopDong.MaPhong.HasValue && hopDong.MaPhong.Value > 0)
                            {
                                hopDong.Phong = _phongService.GetByid(hopDong.MaPhong.Value);
                                item.TongTien = GetTotalTien(item.Id, hopDong.MaPhong.Value);                                
                            }
                            if (hopDong.MaNha.HasValue && hopDong.MaNha.Value > 0)
                            {
                                hopDong.Nha = _nhaService.GetByid(hopDong.MaNha.Value);
                            }
                            item.TienDongStr = item.TienDong.HasValue && item.TienDong.Value > 0 ? item.TienDong.Value.ToString("#,###") : "";
                            item.HopDong = hopDong;
                        }
                    }
                }
                //var data = model.GroupBy(p => p.HanDong).ToList();
                var dataKeyList = model.GroupBy(p => p.HanDong).ToList();
                var doanThuThang = new Dictionary<string, decimal>();
                foreach (var item in dataKeyList)
                {
                    string datestr = item.Key.Value.ToString("MM/yyyy");
                    if (item != null && item.Count() > 0)
                    {
                        decimal total = item.Sum(p => p.TongTien.Value);
                        doanThuThang.Add(datestr, total);
                    }
                }
                return Json(new
                {
                    Data = doanThuThang,
                    MaxPrice= doanThuThang.Max(p=>p.Value)
                });
            }
            return Json(new
            {
                Data = ""
            });
        }
        public decimal GetTotalTien(int mahoadon, int maphong)
        {
            var lstChiTietHoaDonIndexModelViews = new ChiTietHoaDonIndexModelViews();
            decimal tongtien = 0;
            if (mahoadon > 0 && maphong > 0)
            {
                var hoadon = _hoaDonService.GetByid(mahoadon);
                if (hoadon != null)
                {
                    lstChiTietHoaDonIndexModelViews.HoaDon = hoadon;
                }
                var phong = _phongService.GetByid(maphong);
                if (phong != null)
                {
                    tongtien = phong.DonGia.HasValue && phong.DonGia.Value > 0 ? phong.DonGia.Value : 0;
                }

                var dichvus = _dichVuPhongService.GetDichVuByPhong(maphong);

                var chiTietHoaDon = _chiTietHoaDonService.GetChiTietHoaDonByMaHoaDon(mahoadon);
                if (dichvus != null && dichvus.Count > 0)
                {
                    var lstChiTietDichVu = new List<ChiTietHoaDonModelView>();

                    foreach (var dichvu in dichvus)
                    {
                        decimal totalDichVu = 0;
                        var dichvuParsent = _dichVuService.GetByid(dichvu.MaDV.Value);
                        if (chiTietHoaDon != null)
                        {
                            var chiTietHoaDonObject = chiTietHoaDon.Find(p => p.MaDV == dichvu.MaDV);
                            if (chiTietHoaDonObject != null)
                            {
                                if (dichvuParsent != null)
                                {
                                    chiTietHoaDonObject.DichVu = dichvuParsent;
                                    if (dichvuParsent.Id > 0)
                                    {
                                        var dichVuPhong = _dichVuPhongService.GetDichVuByPhong(dichvuParsent.Id, maphong);
                                        chiTietHoaDonObject.DichVu.SoLuong = dichVuPhong.SoLuong;

                                        if (dichVuPhong.SoLuong.HasValue &&
                                        dichVuPhong.SoLuong.Value > 0 &&
                                        dichvuParsent.DonGia.HasValue &&
                                        dichvuParsent.DonGia.Value > 0)
                                        {
                                            totalDichVu = dichvuParsent.DonGia.Value * dichVuPhong.SoLuong.Value;
                                        }
                                        else if (dichvuParsent.LoaiDV == 0 || dichvuParsent.LoaiDV == 1)
                                        {
                                            if (chiTietHoaDonObject.ChiSoDung.HasValue &&
                                                chiTietHoaDonObject.ChiSoDung.Value > 0)
                                            {
                                                totalDichVu = dichvuParsent.DonGia.Value * chiTietHoaDonObject.ChiSoDung.Value;
                                            }
                                        }
                                        else
                                        {
                                            totalDichVu = dichvuParsent.DonGia.Value;
                                        }
                                    }
                                }
                                lstChiTietDichVu.Add(chiTietHoaDonObject);
                            }
                            else
                            {
                                var chiTietDichVuOject = new ChiTietHoaDonModelView();
                                chiTietDichVuOject.MaDV = dichvu.MaDV;
                                if (dichvuParsent != null)
                                {
                                    chiTietDichVuOject.DichVu = dichvuParsent;
                                    if (dichvuParsent.Id > 0)
                                    {
                                        var dichVuPhong = _dichVuPhongService.GetDichVuByPhong(dichvuParsent.Id, maphong);
                                        chiTietDichVuOject.DichVu.SoLuong = dichVuPhong.SoLuong;
                                        if (dichVuPhong.SoLuong.HasValue &&
                                        dichVuPhong.SoLuong.Value > 0 &&
                                        dichvuParsent.DonGia.HasValue &&
                                        dichvuParsent.DonGia.Value > 0)
                                        {
                                            totalDichVu = dichvuParsent.DonGia.Value * dichVuPhong.SoLuong.Value;
                                        }
                                        else if (dichvuParsent.LoaiDV == 0 || dichvuParsent.LoaiDV == 1)
                                        {
                                            if (chiTietDichVuOject.ChiSoDung.HasValue &&
                                                chiTietDichVuOject.ChiSoDung.Value > 0)
                                            {
                                                totalDichVu = dichvuParsent.DonGia.Value * chiTietDichVuOject.ChiSoDung.Value;
                                            }
                                        }
                                        else
                                        {
                                            totalDichVu = dichvuParsent.DonGia.Value;
                                        }
                                        //else if (chiTietHoaDonObject.ChiSoDung.HasValue &&
                                        //    chiTietHoaDonObject.ChiSoDung.Value > 0)
                                        //{
                                        //    totalDichVu += dichvuParsent.DonGia.Value * chiTietHoaDonObject.ChiSoDung.Value;
                                        //}
                                    }
                                    //totalDichVu += dichvuParsent.DonGia.HasValue && dichvuParsent.DonGia.Value > 0 ? dichvuParsent.DonGia.Value : 0;
                                }
                                lstChiTietDichVu.Add(chiTietDichVuOject);
                            }
                        }
                        else
                        {
                            var chiTietDichVuOject = new ChiTietHoaDonModelView();
                            chiTietDichVuOject.MaDV = dichvu.MaDV;
                            if (dichvuParsent != null)
                            {
                                chiTietDichVuOject.DichVu = dichvuParsent;
                                if (dichvuParsent.Id > 0)
                                {
                                    var dichVuPhong = _dichVuPhongService.GetDichVuByPhong(dichvuParsent.Id, maphong);
                                    chiTietDichVuOject.DichVu.SoLuong = dichVuPhong.SoLuong;
                                    if (dichVuPhong.SoLuong.HasValue &&
                                        dichVuPhong.SoLuong.Value > 0 &&
                                        dichvuParsent.DonGia.HasValue &&
                                        dichvuParsent.DonGia.Value > 0)
                                    {
                                        totalDichVu = dichvuParsent.DonGia.Value * dichVuPhong.SoLuong.Value;
                                    }
                                    else if (dichvuParsent.LoaiDV == 0 || dichvuParsent.LoaiDV == 1)
                                    {
                                        if (chiTietDichVuOject.ChiSoDung.HasValue &&
                                                chiTietDichVuOject.ChiSoDung.Value > 0)
                                        {
                                            totalDichVu = dichvuParsent.DonGia.Value * chiTietDichVuOject.ChiSoDung.Value;
                                        }
                                    }
                                    else
                                    {
                                        totalDichVu = dichvuParsent.DonGia.Value;
                                    }
                                    //else if (chiTietDichVuOject.ChiSoDung.HasValue &&
                                    //    chiTietDichVuOject.ChiSoDung.Value > 0)
                                    //{
                                    //    totalDichVu += dichvuParsent.DonGia.Value * chiTietDichVuOject.ChiSoDung.Value;
                                    //}
                                }
                                //totalDichVu += dichvuParsent.DonGia.HasValue && dichvuParsent.DonGia.Value > 0 ? dichvuParsent.DonGia.Value : 0;
                            }
                            lstChiTietDichVu.Add(chiTietDichVuOject);
                        }
                        tongtien += totalDichVu;
                    }

                }
                // lấy thông tin mã lỗi
                if (chiTietHoaDon != null && chiTietHoaDon.Count > 0)
                {
                    var lois = chiTietHoaDon.Where(p => p.MaLoi.HasValue && p.MaLoi.Value > 0).ToList();
                    if (lois != null && lois.Count > 0)
                    {
                        var lstLoiPham = new List<LoiPhamModelView>();

                        foreach (var item in lois)
                        {
                            decimal tienPhat = 0;
                            var loiPham = _loiPhamService.GetByid(item.MaLoi.Value);
                            tienPhat = loiPham.TienPhat.HasValue && loiPham.TienPhat.Value > 0 ? loiPham.TienPhat.Value : 0;
                            lstLoiPham.Add(loiPham);
                            tongtien += tienPhat;
                        }

                    }
                }
            }
            return tongtien;
        }

    }
}
