using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Service;
using System.Text.RegularExpressions;
using TECH.General;
using TECH.Data.DatabaseEntity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TECH.Utilities;
using Newtonsoft.Json;

namespace TECH.Controllers
{
    public class HoaDonController : Controller
    {
        private readonly IHoaDonService _hoaDonService;
        private readonly IHopDongService _hopDongService;
        private readonly INhaService _nhaService;
        private readonly IPhongService _phongService;
        private readonly IDichVuPhongService _dichVuPhongService;
        private readonly IDichVuService _dichVuService;
        private readonly IChiTietHoaDonService _chiTietHoaDonService;
        private readonly ILoiPhamService _loiPhamService;
        private readonly IVnPayService _vnPayservice;
        public IHttpContextAccessor _httpContextAccessor;
        private readonly IKhachHangService _khachHangService;
        private readonly INhanVienService _nhanVienService;

        public HoaDonController(IHoaDonService hoaDonService,
            INhaService nhaService,
            IPhongService phongService,
            IKhachHangService khachHangService,
            IHopDongService hopDongService,
            IDichVuPhongService dichVuPhongService,
            IDichVuService dichVuService,
            IChiTietHoaDonService chiTietHoaDonService,
            ILoiPhamService loiPhamService,
            IHttpContextAccessor httpContextAccessor,
        INhanVienService nhanVienService

            )
        {
            _hopDongService = hopDongService;
            _hoaDonService = hoaDonService;
            _nhaService = nhaService;
            _phongService = phongService;
            _khachHangService = khachHangService;
            _nhanVienService = nhanVienService;
            _dichVuService = dichVuService;
            _dichVuPhongService = dichVuPhongService;
            _chiTietHoaDonService = chiTietHoaDonService;
            _loiPhamService = loiPhamService;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var user = new UserMapModelView();
            if (string.IsNullOrEmpty(userString))
            {
                return Redirect("/");
            }
            return View();
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
                    if (HoaDonModelView.MaPhongs.IndexOf(0) >=0)
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
        public JsonResult GetHoaDonByMaHoaDonMaPhong(int mahoadon, int maphong)
        {
            var hoaDon = _hoaDonService.GetByid(mahoadon);
            if (hoaDon != null)
            {
                hoaDon.TongTien = GetTotalTien(mahoadon, maphong);
                if (hoaDon.TongTien > 0)
                {
                    hoaDon.TongTienStr = hoaDon.TongTien.HasValue && hoaDon.TongTien.Value > 0 ? hoaDon.TongTien.Value.ToString("#,###"):"";
                }
            }
            return Json(new
            {
                Data = hoaDon
            });
        }

        [HttpPost]
        public JsonResult PaymentBill(HoaDonModelView HoaDonModelView)
        {
            _hoaDonService.PayBill(HoaDonModelView);
            return Json(new
            {
                success = true
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
                                        else if(dichvuParsent.LoaiDV == 0 || dichvuParsent.LoaiDV == 1)
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
                    //lstChiTietHoaDonIndexModelViews.TongTien += totalDichVu;
                    //lstChiTietHoaDonIndexModelViews.ChiTietHoaDonModelViews = lstChiTietDichVu;

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
                       
                        //lstChiTietHoaDonIndexModelViews.TongTien += tienPhat;
                        //lstChiTietHoaDonIndexModelViews.LoiPhamModelViews = lstLoiPham;
                    }
                }
            }
            return tongtien;
        }

        [HttpGet]
        public JsonResult GetAllPaging(HoaDonViewModelSearch hoaDonViewModelSearch)
        {
            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var user = new UserMapModelView();
            if (!string.IsNullOrEmpty(userString))
            {
                user = JsonConvert.DeserializeObject<UserMapModelView>(userString);
            }
            hoaDonViewModelSearch.PageSize = 300;
            var data = _hoaDonService.GetAllPaging(hoaDonViewModelSearch);
            var lstData = new PagedResult<HoaDonModelView>();
            if (data.Results != null && data.Results.Count > 0)
            {
                foreach (var item in data.Results)
                {
                    if (item.MaHopDong.HasValue && item.MaHopDong.Value > 0)
                    {
                        var hopDong = _hopDongService.GetByid(item.MaHopDong.Value);
                        if (hopDong != null)
                        {
                            if (hopDong.MaKH.HasValue && hopDong.MaKH.Value > 0 && hopDong.MaKH.Value == user?.Id)
                            {
                                hopDong.KhachHang = _khachHangService.GetByid(hopDong.MaKH.Value);
                                if (hopDong.MaPhong.HasValue && hopDong.MaPhong.Value > 0)
                                {
                                    hopDong.Phong = _phongService.GetByid(hopDong.MaPhong.Value);
                                    item.TongTien = GetTotalTien(item.Id, hopDong.MaPhong.Value);
                                    item.TongTienStr = item.TongTien.HasValue && item.TongTien.Value > 0 ? item.TongTien.Value.ToString("#,###") : "";
                                }
                                if (hopDong.MaNha.HasValue && hopDong.MaNha.Value > 0)
                                {
                                    hopDong.Nha = _nhaService.GetByid(hopDong.MaNha.Value);
                                }
                                item.TienDongStr = item.TienDong.HasValue && item.TienDong.Value > 0 ? item.TienDong.Value.ToString("#,###") : "";
                                item.HopDong = hopDong;
                                lstData.Results.Add(item);
                            }
                           
                        }
                    }                  
                }
                data.Results = lstData.Results;
                //if (hoaDonViewModelSearch != null && !string.IsNullOrEmpty(hoaDonViewModelSearch.name))
                //{
                //    data.Results = data.Results.Where(p => p.k.Contains(hoaDonViewModelSearch.name) ||
                //    p.TenPhong.Contains(phongViewModelSearch.name) ||
                //    p.TenKhachHang.Contains(phongViewModelSearch.name) ||
                //    p.TenNhanVien.Contains(phongViewModelSearch.name)).ToList();
                //}
                if (hoaDonViewModelSearch.status > 0)
                {
                    data.Results = data.Results.Where(p => p.TrangThai == hoaDonViewModelSearch.status).ToList();
                }
            }
            
            return Json(new { data = data });
        }
        [HttpPost]
        public IActionResult CreatePaymentBill(int mahoadon, int maphong)
        {
            // Get bill details
            var hoaDon = _hoaDonService.GetByid(mahoadon);
            if (hoaDon == null)
            {
                return Json(new { success = false, message = "Không tìm thấy hóa đơn" });
            }

            // Calculate total amount if not already set
            if (!hoaDon.TongTien.HasValue || hoaDon.TongTien.Value == 0)
            {
                hoaDon.TongTien = GetTotalTien(mahoadon, maphong);
            }

            // Create payment request model
            var paymentRequest = new VnPaymentRequestModel
            {
                OrderId = mahoadon.ToString(), // Convert int to string
                Amount = hoaDon.TongTien.Value,
                CreatedDate = DateTime.Now,
                Description = $"{mahoadon}_{maphong}" // Use Description instead of OrderDescription
            };

            // Generate payment URL
            var url = _vnPayservice.CreatePaymentUrl(HttpContext, paymentRequest);
            return Json(new { success = true, url = url });
        }

        public IActionResult PaymentCallback()
        {
            // Process VNPay callback
            var response = _vnPayservice.PaymentExecute(Request.Query);

            if (response == null || !response.Success)
            {
                return Json(new
                {
                    success = false,
                    message = $"Lỗi thanh toán VN Pay: {response?.VnPayResponseCode ?? "Unknown Error"}"
                });
            }

            // Extract mahoadon and maphong from OrderInfo instead of OrderDescription
            var orderInfo = response.OrderDescription.Split('_');
            if (orderInfo.Length < 2 ||
                !int.TryParse(orderInfo[0], out int mahoadon) ||
                !int.TryParse(orderInfo[1], out int maphong))
            {
                return Json(new
                {
                    success = false,
                    message = "Không thể xác định thông tin hóa đơn từ giao dịch"
                });
            }

            // Update bill status
            var hoaDon = _hoaDonService.GetByid(mahoadon);
            if (hoaDon == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Không tìm thấy hóa đơn"
                });
            }

            // Update payment information
            hoaDon.TrangThai = 1; // Đã thanh toán
            hoaDon.TienDong = hoaDon.TongTien;
            hoaDon.NgayDongTien = DateTime.Now;
            // Remove MaGiaoDich assignment if it's not in the model
            // hoaDon.MaGiaoDich = response.TransactionId;

            _hoaDonService.Update(hoaDon);
            _hoaDonService.Save();

            return Json(new
            {
                success = true,
                message = "Thanh toán thành công",
                data = new
                {
                    OrderId = response.OrderId,
                    TransactionId = response.TransactionId,
                    PaymentMethod = response.PaymentMethod,
                    Amount = hoaDon.TongTien
                }
            });
        }
    }
}
