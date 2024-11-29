using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Service;
using System.Text.RegularExpressions;
using TECH.General;
using TECH.Data.DatabaseEntity;
using Newtonsoft.Json;

namespace TECH.Controllers
{
    public class ChiTietHoaDonController : Controller
    {
        private readonly IHoaDonService _hoaDonService;
        private readonly IHopDongService _hopDongService;
        private readonly INhaService _nhaService;
        private readonly IPhongService _phongService;
        private readonly IKhachHangService _khachHangService;
        private readonly INhanVienService _nhanVienService;
        private readonly IChiTietHoaDonService _chiTietHoaDonService;
        private readonly IDichVuPhongService _dichVuPhongService;
        private readonly IDichVuService _dichVuService;
        private readonly ILoiPhamService _loiPhamService;
        public IHttpContextAccessor _httpContextAccessor;
        public ChiTietHoaDonController(IHoaDonService hoaDonService,
            INhaService nhaService,
            IPhongService phongService,
            IKhachHangService khachHangService,
            IHopDongService hopDongService,
            INhanVienService nhanVienService,
            IDichVuPhongService dichVuPhongService,
            IDichVuService dichVuService,
            ILoiPhamService loiPhamService,
            IChiTietHoaDonService chiTietHoaDonService,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _hopDongService = hopDongService;
            _hoaDonService = hoaDonService;
            _nhaService = nhaService;
            _phongService = phongService;
            _khachHangService = khachHangService;
            _nhanVienService = nhanVienService;
            _chiTietHoaDonService = chiTietHoaDonService;
            _dichVuPhongService = dichVuPhongService;
            _dichVuService = dichVuService;
            _loiPhamService = loiPhamService;
        }
        public IActionResult Index(int mahoadon, int maphong)
        {

            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var user = new UserMapModelView();
            if (string.IsNullOrEmpty(userString))
            {
                return Redirect("/");
            }

        var lstChiTietHoaDonIndexModelViews = new ChiTietHoaDonIndexModelViews();
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
                    lstChiTietHoaDonIndexModelViews.TongTien = phong.DonGia.HasValue && phong.DonGia.Value > 0 ? phong.DonGia.Value : 0;
                    lstChiTietHoaDonIndexModelViews.Phong = phong;
                }

                var dichvus = _dichVuPhongService.GetDichVuByPhong(maphong);

                var chiTietHoaDon = _chiTietHoaDonService.GetChiTietHoaDonByMaHoaDon(mahoadon);
                if (dichvus != null && dichvus.Count > 0)
                {
                    var lstChiTietDichVu = new List<ChiTietHoaDonModelView>();
                    decimal totalDichVu = 0;
                    foreach (var dichvu in dichvus)
                    {
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
                                    }
                                    totalDichVu += dichvuParsent.DonGia.HasValue && dichvuParsent.DonGia.Value > 0 ? dichvuParsent.DonGia.Value : 0;
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
                                    }
                                    totalDichVu += dichvuParsent.DonGia.HasValue && dichvuParsent.DonGia.Value > 0 ? dichvuParsent.DonGia.Value : 0;
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
                                }
                                totalDichVu += dichvuParsent.DonGia.HasValue && dichvuParsent.DonGia.Value > 0 ? dichvuParsent.DonGia.Value : 0;
                            }
                            lstChiTietDichVu.Add(chiTietDichVuOject);
                        }
                    }
                    lstChiTietHoaDonIndexModelViews.TongTien += totalDichVu;
                    lstChiTietHoaDonIndexModelViews.ChiTietHoaDonModelViews = lstChiTietDichVu;

                }
                // lấy thông tin mã lỗi
                if (chiTietHoaDon != null && chiTietHoaDon.Count > 0)
                {
                    var lois = chiTietHoaDon.Where(p => p.MaLoi.HasValue && p.MaLoi.Value > 0).ToList();
                    if (lois != null && lois.Count > 0)
                    {
                        var lstLoiPham = new List<LoiPhamModelView>();
                        decimal tienPhat = 0;
                        foreach (var item in lois)
                        {
                            var loiPham = _loiPhamService.GetByid(item.MaLoi.Value);
                            tienPhat += loiPham.TienPhat.HasValue && loiPham.TienPhat.Value > 0 ? loiPham.TienPhat.Value : 0;
                            lstLoiPham.Add(loiPham);
                        }
                        lstChiTietHoaDonIndexModelViews.TongTien += tienPhat;
                        lstChiTietHoaDonIndexModelViews.LoiPhamModelViews = lstLoiPham;
                    }
                }
            }
            return View(lstChiTietHoaDonIndexModelViews);

            //var data = _chiTietHoaDonService.GetChiTietHoaDonByMaHoaDon(mahoadon);
            // trong trường hợp nếu data == null thì chưa được add
            // lấy danh sách các dịch vụ của phòng sau đó compare với các dịch ở vụ ở trong bảng chi tiết hóa đơn
            // sẽ lấy dự liệu từ bảng dịch vụ phòng để làm chuẩn 
            // 1 nếu trong bảng chi tiết dịch vụ chưa có thì sẽ view all các dịch vụ của phòng lấy 
            // 2 nếu trong bảng chi tiết dịch vụ có tồn tại data

        }

        [HttpGet]
        public IActionResult ViewDichVu(int maDichVu, int maPhong, int maHoaDon)
        {

            var data = new ChiTietHoaDonModelView();
            if (maDichVu > 0 && maPhong > 0 && maHoaDon > 0)
            {
                var dichvus = _dichVuPhongService.GetDichVuByPhong(maPhong);
                if (dichvus != null && dichvus.Count > 0)
                {
                    var chiTietHoaDon = _chiTietHoaDonService.GetChiTietHoaDonByMaHoaDon(maHoaDon);
                    if (chiTietHoaDon != null && chiTietHoaDon.Count > 0)
                    {
                        var dichVuHoaDon = chiTietHoaDon.Find(p => p.MaDV == maDichVu);
                        if (dichVuHoaDon != null)
                        {
                            data = dichVuHoaDon;
                        }

                    }

                    var dichvu = _dichVuService.GetByid(maDichVu);
                    if (dichvu != null)
                    {
                        var dichVuPhong = dichvus.Find(p => p.MaDV == dichvu.Id);
                        if (dichVuPhong != null)
                        {
                            dichvu.SoLuong = dichVuPhong.SoLuong;
                        }
                        data.DichVu = dichvu;
                    }                                           
                }
                var hoadon = _hoaDonService.GetByid(maHoaDon);
                data.HoaDon = hoadon;
                data.MaHoaDon = maHoaDon;
            }
            return View("ViewDichVu", data);
        }


        [HttpGet]
        public IActionResult AddView()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetById(int id)
        {
            var model = new HoaDonModelView();
            if (id > 0)
            {
                model = _hoaDonService.GetByid(id);
            }
            return Json(new
            {
                Data = model
            });
        }
        [HttpPost]
        public JsonResult SaveDichVuThanhToan(ChiTietHoaDonModelView chiTietHoaDonModelView)
        {
            if (chiTietHoaDonModelView != null)
            {                
                if (chiTietHoaDonModelView.MaDV.HasValue && chiTietHoaDonModelView.MaDV.Value > 0)
                {
                    if (chiTietHoaDonModelView.MaHoaDon.HasValue && chiTietHoaDonModelView.MaHoaDon.Value > 0)
                    {
                        _chiTietHoaDonService.Deleted(chiTietHoaDonModelView.MaHoaDon.Value, chiTietHoaDonModelView.MaDV.Value);
                    }
                    var dichvu = _dichVuService.GetByid(chiTietHoaDonModelView.MaDV.Value);
                    if (dichvu != null && dichvu.DonGia.HasValue && dichvu.DonGia.Value > 0 && chiTietHoaDonModelView.ChiSoDung.HasValue && chiTietHoaDonModelView.ChiSoDung.Value > 0)
                    {
                        chiTietHoaDonModelView.ThanhTien = dichvu.DonGia.Value * chiTietHoaDonModelView.ChiSoDung.Value;
                    }
                    _chiTietHoaDonService.Add(chiTietHoaDonModelView);
                    _chiTietHoaDonService.Save();
                    return Json(new
                    {
                        success = true
                    });
                }
            }
            return Json(new
            {
                success = false
            });
        }        
        public JsonResult SaveChiTietHoaDon(int maPhong, int maHoaDon)
        {
            if (maHoaDon > 0 && maPhong > 0)
            {
                //if (chiTietHoaDonModelView.MaDV.HasValue && chiTietHoaDonModelView.MaDV.Value > 0)
                //{
                //    if (chiTietHoaDonModelView.MaHoaDon.HasValue && chiTietHoaDonModelView.MaHoaDon.Value > 0)
                //    {
                //        _chiTietHoaDonService.Deleted(chiTietHoaDonModelView.MaHoaDon.Value, chiTietHoaDonModelView.MaDV.Value);
                //    }
                //    var dichvu = _dichVuService.GetByid(chiTietHoaDonModelView.MaDV.Value);
                //    if (dichvu != null && dichvu.DonGia.HasValue && dichvu.DonGia.Value > 0 && chiTietHoaDonModelView.ChiSoDung.HasValue && chiTietHoaDonModelView.ChiSoDung.Value > 0)
                //    {
                //        chiTietHoaDonModelView.ThanhTien = dichvu.DonGia.Value * chiTietHoaDonModelView.ChiSoDung.Value;
                //    }
                //    _chiTietHoaDonService.Add(chiTietHoaDonModelView);
                //    _chiTietHoaDonService.Save();
                //    return Json(new
                //    {
                //        success = true
                //    });
                //}
            }
            return Json(new
            {
                success = false
            });
        }

        public List<PhongModelView> GetDataPhongByHopDong()
        {
            var model = new List<HopDongModelView>();
            model = _hopDongService.GetPhongByHopDong();
            if (model != null && model.Count > 0)
            {
                var data = new List<PhongModelView>();
                foreach (var item in model)
                {
                    if (item.MaPhong.HasValue && item.MaPhong.Value > 0)
                    {
                        var phong = _phongService.GetByid(item.MaPhong.Value);
                        if (phong != null)
                        {
                            data.Add(phong);
                        }
                    }
                }
                if (data.Count > 0)
                {
                    return data;
                }
            }
            return null;
        }
        [HttpGet]
        public JsonResult GetPhongByHopDong()
        {
            var data = GetDataPhongByHopDong();
            return Json(new
            {
                Data = data
            });
        }

        [HttpPost]
        public JsonResult Add(HoaDonModelView HoaDonModelView)
        {
            if (HoaDonModelView != null)
            {
                if (HoaDonModelView.MaPhongs != null && HoaDonModelView.MaPhongs.Count > 0)
                {
                    if (HoaDonModelView.MaPhongs.IndexOf(0) >= 0)
                    {
                        var data = GetDataPhongByHopDong();
                        if (data != null && data.Count > 0)
                        {
                            foreach (var item in data)
                            {
                                var hopdong = _hopDongService.GetHopDongByPhong(item.Id);
                                if (hopdong.Id > 0)
                                {
                                    HoaDonModelView.MaHopDong = hopdong.Id;
                                    _hoaDonService.Add(HoaDonModelView);
                                }
                            }
                        }
                        else
                        {
                            var hopdongs = _hopDongService.GetHopDongByPhong(HoaDonModelView.MaPhongs);
                            if (hopdongs != null && hopdongs.Count > 0)
                            {
                                foreach (var item in hopdongs)
                                {
                                    HoaDonModelView.MaHopDong = item.Id;
                                    _hoaDonService.Add(HoaDonModelView);
                                }
                            }
                        }
                    }
                }
            }


            _hoaDonService.Save();
            return Json(new
            {
                success = true
            });
        }

        [HttpPost]
        public JsonResult Update(HoaDonModelView HoaDonModelView)
        {
            var result = _hoaDonService.Update(HoaDonModelView);
            //if (HoaDonModelView.TrangThai.HasValue && HoaDonModelView.TrangThai.Value == 3)
            //{
            //    _phongService.UpdateTrangThai(HoaDonModelView.MaPhong.Value, 1); // Trống
            //}
            _hoaDonService.Save();
            return Json(new
            {
                success = result
            });
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            if (id > 0)
            {
                //var data = _hoaDonService.GetByid(id);
                //if (data != null && data.MaPhong.HasValue && data.MaPhong.Value > 0)
                //{
                //    _phongService.UpdateTrangThai(data.MaPhong.Value, 1); // Trống
                //}
                var result = _hoaDonService.Deleted(id);
                _hoaDonService.Save();
                return Json(new
                {
                    success = result
                });
            }
            return Json(new
            {
                success = false
            });
        }

        [HttpGet]
        public JsonResult GetAllPaging(HoaDonViewModelSearch hoaDonViewModelSearch)
        {
            var data = _hoaDonService.GetAllPaging(hoaDonViewModelSearch);
            if (data.Results != null && data.Results.Count > 0)
            {
                foreach (var item in data.Results)
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
                            }
                            if (hopDong.MaNha.HasValue && hopDong.MaNha.Value > 0)
                            {
                                hopDong.Nha = _nhaService.GetByid(hopDong.MaNha.Value);
                            }
                            item.HopDong = hopDong;
                        }
                    }
                }
            }
            return Json(new { data = data });
        }
        // Phạt tiền khi phạm lỗi

        [HttpPost]
        public JsonResult AddLoiPhamAndAddLoiChiTietHoaDon(LoiPhamModelView loiPhamModelView,int maHoaDon)
        {
            if (loiPhamModelView != null && maHoaDon > 0)
            {
                int maLoiPham = _loiPhamService.Add(loiPhamModelView);
                if (maLoiPham > 0)
                {
                    _chiTietHoaDonService.AddLoi(maHoaDon,maLoiPham);
                    _hoaDonService.Save();
                }                
            }            
            return Json(new
            {
                success = true
            });
        }

    }
}
