using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Service;
using System.Text.RegularExpressions;
using TECH.General;
using System.Net.Mail;
using Newtonsoft.Json;

namespace TECH.Areas.Admin.Controllers
{
    public class HopDongController : BaseController
    {
        private readonly IHopDongService _hopDongService;
        private readonly INhaService _nhaService;
        private readonly IPhongService _phongService;
        private readonly IDichVuPhongService _dichVuPhongService;
        private readonly IKhachHangService _khachHangService;
        private readonly INhanVienService _nhanVienService;
        private readonly IThanhVienPhongService _thanhVienPhongService;
        private readonly IVnPayService _vnPayservice;
        public IHttpContextAccessor _httpContextAccessor;
        public HopDongController(IHopDongService hopDongService,
            INhaService nhaService,
            IPhongService phongService,
            IKhachHangService khachHangService,
            INhanVienService nhanVienService,
            IDichVuPhongService dichVuPhongService,
             IVnPayService vnPayservice,
             IHttpContextAccessor httpContextAccessor,
            IThanhVienPhongService thanhVienPhongService
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _vnPayservice = vnPayservice;
            _hopDongService = hopDongService;
            _nhaService = nhaService;
            _phongService = phongService;
            _khachHangService = khachHangService;
            _nhanVienService = nhanVienService;
            _dichVuPhongService = dichVuPhongService;
            _thanhVienPhongService = thanhVienPhongService;
            //_nhaService = nhaService;
        }
        public IActionResult Index()
        {
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
            var model = new HopDongModelView();
            if (id > 0)
            {
                model = _hopDongService.GetByid(id);
            }
            return Json(new
            {
                Data = model
            });
        }

        [HttpPost]
        public JsonResult Add(HopDongModelView HopDongModelView)
        {
            // get thông tin hop đồng
            // nếu tồn tại user trong hợp đồng thì không add
            // ngược lại thì sẽ được add vào
            
            // update trạng thái phòng thành đã có khách thuê
            if (HopDongModelView.MaPhong.HasValue && HopDongModelView.MaPhong.Value > 0 && HopDongModelView.TrangThai.HasValue && (HopDongModelView.TrangThai.Value == 1 || HopDongModelView.TrangThai.Value == 2))
            {
                
                var hopDong = _hopDongService.GetHopDongByKhachHangAndPhong(HopDongModelView.MaKH.Value, HopDongModelView.MaPhong.Value);
                if (hopDong == null)
                {
                    if (HopDongModelView.TrangThaiPayment == 1)
                    {
                        _httpContextAccessor.HttpContext.Session.SetString("OrdersModelView", JsonConvert.SerializeObject(HopDongModelView));
                        var vnPayModel = new VnPaymentRequestModel
                        {
                            Amount = HopDongModelView.TienCoc.Value,
                            CreatedDate = DateTime.Now,
                            //Description = $"{HopDongModelView.full_name} {user.phone_number}",
                            //FullName = user.full_name,
                            OrderId = HopDongModelView.MaPhong.ToString()
                        };
                        string htmlvpn = _vnPayservice.CreatePaymentUrl(HttpContext, vnPayModel);
                        return Json(new
                        {
                            success = true,
                            vnpay = htmlvpn
                        });
                    }
                    else
                    {
                        _hopDongService.Add(HopDongModelView);

                        _phongService.UpdateTrangThai(HopDongModelView.MaPhong.Value, 2); // đã thuê
                                                                                          // Add thành viên phòng
                        var thanhvienphong = new ThanhVienPhongModelView();
                        thanhvienphong.MaKH = HopDongModelView.MaKH;
                        thanhvienphong.MaPhong = HopDongModelView.MaPhong;
                        _thanhVienPhongService.Add(thanhvienphong);
                        _hopDongService.Save();
                    }
                }
                    
            }
           
            return Json(new
            {
                success = true
            });
        }

        //public IActionResult PaymentCallBack(string vnp_Amount, string vnp_BankCode, string vnp_BankTranNo, string vnp_CardType, string vnp_OrderInfo, string vnp_PayDate, string vnp_ResponseCode, string vnp_TmnCode, string vnp_TransactionNo,
        //    string vnp_TransactionStatus, string vnp_TxnRef, string vnp_SecureHash)
        //{
        //    var response = _vnPayservice.PaymentExecute(Request.Query);
        //    string Message = "";
        //    if (response == null || vnp_ResponseCode != "00")
        //    {
        //        Message = $"Lỗi thanh toán VN Pay: {response.VnPayResponseCode}";
        //    }

        //    var data = _httpContextAccessor.HttpContext.Session.GetString("OrdersModelView");
        //    if (data != null)
        //    {
        //        var dataConvert = JsonConvert.DeserializeObject<HopDongModelView>(data);
        //        if (dataConvert != null)
        //        {
        //            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");

        //            _hopDongService.Add(dataConvert);

        //            _phongService.UpdateTrangThai(dataConvert.MaPhong.Value, 2); // đã thuê
        //                                                                              // Add thành viên phòng
        //            var thanhvienphong = new ThanhVienPhongModelView();
        //            thanhvienphong.MaKH = dataConvert.MaKH;
        //            thanhvienphong.MaPhong = dataConvert.MaPhong;
        //            _thanhVienPhongService.Add(thanhvienphong);
        //            _hopDongService.Save();
        //            _httpContextAccessor.HttpContext.Session.Remove("OrdersModelView");
        //            Message = "Thanh toán thành công";
        //        }
        //    }
        //    TempData["Message"] = Message;
        //    return View();
        //}


        //    public string BodyHtmlMail(KhachHangModelView khachHang, HopDongModelView hopdong,PhongModelView phong)
        //    {
        //        var html =
        //"<div width='100%' style='margin: 0; padding: 20px !important; background-color: #f1f1f1;'>" +
        //    "<h3 style='padding: 0px 0px 0px 0px;font-size: 20px;'>Xin Chào Quý Khách</h3>                               " +
        //    "<p style='padding: 10px 0px 0px 0px;font-size: 17px;'>Khách sạn cà mau xin chân thành cảm ơn quý khách đã tin tưởng và đặt phòng.</p>                               " +
        //    "<p style='padding: 10px 0px 0px 0px;'>Thông tin phòng của bạn.</p>                               " +
        //     "<p style='padding: 10px 0px 0px 0px;'>Khách Hang: "+khachHang.TenKH+".</p>                               " +
        //     "<p style='padding: 10px 0px 0px 0px;'>Số điện thoại: " + khachHang.SoDienThoai + ".</p>                               " +
        //     "<p style='padding: 10px 0px 0px 0px;'>Địa chỉ: " + khachHang.DiaChi + ".</p>                               " +
        //     "<p style='padding: 10px 0px 0px 0px;'>Số phòng: " + phong.TenPhong + ".</p>                               " +
        //     "<p style='padding: 10px 0px 0px 0px;'>Chiều dài: " + phong.ChieuDai + ".</p>                               " +
        //     "<p style='padding: 10px 0px 0px 0px;'>Chiều rộng: " + phong.ChieuRong + ".</p>                               " +
        //     "<p style='padding: 10px 0px 0px 0px;'>Tiền cọc: " + hopdong.TienCoc + ".</p>                               " +
        //     "<p style='padding: 10px 0px 0px 0px;'>Ngày bắt đầu: " + hopdong.NgayBatDau.Value.ToString("yyyy/MM/dd") + ".</p>                               " +
        //     "<p style='padding: 10px 0px 0px 0px;'>Tiền phòng: " + phong.DonGiaStr + ".</p>                               " +
        //     "<p style='padding: 10px 0px 0px 0px;'>Lưu ý.</p>                               " +
        //     "<p style='padding: 10px 0px 0px 0px;'>Quý khách vui lòng đến địa chỉ Số 16 – Ngô Quyền – Khóm 5 – Phường – Thành Phố Cà Mau để tiến hành cách thủ tục nhận phòng và thanh toán tiền để tiếp nhận các cơ sở vật chất và các dịc vụ phòng..</p>                               " +
        //     "<p style='padding: 10px 0px 0px 0px;'>Nếu sau 3 ngày kể từ ngày khách hàng đặt cọc nhưng không đến nơi để tiến hành các thủ tục nhận phòng thì chúng tôi xẽ không trả lại tiền cọc. Xin cảm ơn!\r\nTiền phòng chưa bao gồm dịch vụ. Quyền lợi của khác hàng được thể hiện đầy đủ ở cuối trang web, quý khách hãy đok kĩ trước khi tiến hành thuê phòng..</p>                               " +
        //     "<p style='padding: 10px 0px 0px 0px;'>Quý khách vui lòng liên hệ với chúng tôi:</p>                               " +
        //     "<p style='padding: 10px 0px 0px 0px;'>SDT: 0346732288</p>                               " +
        //     "<p style='padding: 10px 0px 0px 0px;'>FACEBOOK: https://www.facebook.com/</p>                               " +
        //     "<p style='padding: 10px 0px 0px 0px;'>ZALO: 0346732288</p>                               " +
        //     "<p style='padding: 10px 0px 0px 0px;'>GMAIL: 160501004@student.bdu.edu.vn</p>                               " +
        //     "<p style='padding: 10px 0px 0px 0px;'>Địa chỉ văn phòng: Số 16 – Ngô Quyền – Khóm 5 – Phường – Thành Phố Cà Mau.</p>                               " +
        //     "<p style='padding: 10px 0px 0px 0px;'>Xin trân thành cảm ơn!.</p>                               </div>";
        //        return html;
        //    }

        [HttpPost]
        public JsonResult Update(HopDongModelView HopDongModelView)
        {
            var hopDongServer = new HopDongModelView();
            if (HopDongModelView != null && HopDongModelView.Id > 0)
            {
                hopDongServer = _hopDongService.GetByid(HopDongModelView.Id);
                if (hopDongServer != null && HopDongModelView != null)
                {
                    if (hopDongServer.MaPhong != HopDongModelView.MaPhong)
                    {
                        _phongService.UpdateTrangThai(hopDongServer.MaPhong.Value, 1); // Trống
                        _dichVuPhongService.DeletedByMaPhong(hopDongServer.MaPhong.Value);  // xóa dịch vụ của phòng
                        _thanhVienPhongService.DeletedByMaPhong(hopDongServer.MaPhong.Value);  // xóa thành viên phòng

                        _phongService.UpdateTrangThai(HopDongModelView.MaPhong.Value, 2); // đã thuê
                    }                  
                    if (hopDongServer.MaKH != HopDongModelView.MaKH)
                    {
                        // lấy thành viên cũ 
                        var thanhviencuphong = _thanhVienPhongService.GetByThanhVienByMaPhongMaKH(hopDongServer.MaKH.Value, hopDongServer.MaPhong.Value);
                        if (thanhviencuphong != null && thanhviencuphong.Id > 0)
                        {
                            _thanhVienPhongService.Deleted(thanhviencuphong.Id);
                            _thanhVienPhongService.Save();
                        }
                        var thanhvienphong = new ThanhVienPhongModelView();
                        thanhvienphong.MaKH = HopDongModelView.MaKH;
                        thanhvienphong.MaPhong = HopDongModelView.MaPhong;
                        _thanhVienPhongService.Add(thanhvienphong);
                    }
                }               
            }
            var result = _hopDongService.Update(HopDongModelView);
            if (HopDongModelView.TrangThai.HasValue && HopDongModelView.TrangThai.Value == 2)
            {
                _phongService.UpdateTrangThai(HopDongModelView.MaPhong.Value, 1); // Trống
                _dichVuPhongService.DeletedByMaPhong(HopDongModelView.MaPhong.Value);  // xóa dịch vụ của phòng
                _thanhVienPhongService.DeletedByMaPhong(HopDongModelView.MaPhong.Value);  // xóa thành viên phòng

            }else if (HopDongModelView.TrangThai.HasValue && HopDongModelView.TrangThai.Value == 1)
            {
                _phongService.UpdateTrangThai(HopDongModelView.MaPhong.Value, 2); // đã thuê
            }

            _hopDongService.Save();
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
                var data = _hopDongService.GetByid(id);
                if (data != null && data.MaPhong.HasValue && data.MaPhong.Value > 0)
                {
                    _phongService.UpdateTrangThai(data.MaPhong.Value, 1); // Trống
                    _dichVuPhongService.DeletedByMaPhong(data.MaPhong.Value);  // xóa dịch vụ của phòng
                    _thanhVienPhongService.DeletedByMaPhong(data.MaPhong.Value);  // xóa thành viên phòng
                }
                 _hopDongService.UpdateIsDeteled(id,true);
                _hopDongService.Save();
                return Json(new
                {
                    success = true
                });
            }
            return Json(new
            {
                success = false
            });
        }

        [HttpGet]
        public JsonResult GetAllPaging(HopDongViewModelSearch phongViewModelSearch)
        {
            var data = _hopDongService.GetAllPaging(phongViewModelSearch);
            foreach (var item in data.Results)
            {
                if (item.MaNha.HasValue && item.MaNha.Value > 0)
                {
                    item.TenNha = _nhaService.GetByid(item.MaNha.Value)?.TenNha;
                }
                if (item.MaPhong.HasValue && item.MaPhong.Value > 0)
                {
                    item.TenPhong = _phongService.GetByid(item.MaPhong.Value)?.TenPhong;
                }
                if (item.MaKH.HasValue && item.MaKH.Value > 0)
                {
                    item.TenKhachHang = _khachHangService.GetByid(item.MaKH.Value)?.TenKH;
                }
                if (item.MaNV.HasValue && item.MaNV.Value > 0)
                {
                    item.TenNhanVien = _nhanVienService.GetByid(item.MaNV.Value)?.TenNV;
                }
                if (item.TrangThai.HasValue && item.TrangThai.Value > 0)
                {
                    item.TrangThaiStr = Common.GetTinhTrangHoaDon(item.TrangThai.Value);
                }
            }
            if (phongViewModelSearch != null && !string.IsNullOrEmpty(phongViewModelSearch.name))
            {
                data.Results = data.Results.Where(p => !string.IsNullOrEmpty(p.TenNha) && p.TenNha.ToLower().Contains(phongViewModelSearch.name.ToLower()) ||
               (!string.IsNullOrEmpty(p.TenPhong) && p.TenPhong.ToLower().Contains(phongViewModelSearch.name.ToLower())) ||
               (!string.IsNullOrEmpty(p.TenKhachHang) && p.TenKhachHang.ToLower().Contains(phongViewModelSearch.name.ToLower()))||
               (!string.IsNullOrEmpty(p.TenNhanVien) && p.TenNhanVien.ToLower().Contains(phongViewModelSearch.name.ToLower()))).ToList();
            }
            if (phongViewModelSearch.status > 0)
            {
                data.Results = data.Results.Where(p=>p.TrangThai == phongViewModelSearch.status).ToList();
            }
            data.Results = data.Results.OrderByDescending(p => p.NgayBatDau).ToList();
            return Json(new { data = data });
        }

    }
}
