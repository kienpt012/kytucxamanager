using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.General;
using TECH.Models;
using TECH.Service;
using TECH.Utilities;

namespace TECH.Controllers
{
    public class PhongController : Controller
    {
        private readonly IPhongService _phongService;
        private readonly IHopDongService _hopDongService;
        private readonly INhaService _nhaService;
        //private readonly ICategoryService _categoryService;
        //private readonly ISizesService _sizesService;
        private readonly IThanhVienPhongService _thanhVienPhongService;
        public IHttpContextAccessor _httpContextAccessor;
        private readonly IVnPayService _vnPayservice;
        public PhongController(IPhongService phongService,
            IHttpContextAccessor httpContextAccessor,
             IThanhVienPhongService thanhVienPhongService,
              IHopDongService hopDongService,
        IVnPayService vnPayservice,
        INhaService nhaService
            )
        {
            _hopDongService = hopDongService;
            _vnPayservice = vnPayservice;
            _phongService = phongService;
            _nhaService = nhaService;
            _httpContextAccessor = httpContextAccessor;
            _thanhVienPhongService = thanhVienPhongService;
        }

        public IActionResult PhongCategory(int categoryId)
        {
            var productViewModelSearch = new PhongViewModelSearch();
            productViewModelSearch.PageIndex = 1;
            productViewModelSearch.PageSize = 250;
            productViewModelSearch.categoryId = categoryId;
            var data = _phongService.GetAllPaging(productViewModelSearch);
            if (data != null && data.Results != null && data.Results.Count > 0)
            {
                //data.Results = data.Results.Where(p => p.ishidden != 1).ToList();
                foreach (var item in data.Results)
                {
                    if (item.MaNha.HasValue && item.MaNha.Value > 0)
                    {
                        var category = _nhaService.GetByid(item.MaNha.Value);
                        if (category != null && !string.IsNullOrEmpty(category.TenNha))
                        {
                            item.TenNha = category.TenNha;
                        }
                        else
                        {
                            item.TenNha = "Chờ xử lý";
                        }
                        var UserRooms = _thanhVienPhongService.GetThanhVienByPhong(item.Id);
                        if (UserRooms != null)
                        {
                            item.SoLuongNguoiDangO = UserRooms.Count();
                        }
                      
                    }
                    else
                    {
                        item.TenNha = "";
                    }
                    if (item.LoaiPhong.HasValue)
                    {
                        item.LoaiPhongStr = Common.GetLoaiPhong(item.LoaiPhong.Value);
                    }
                    if (item.TinhTrang.HasValue)
                    {
                        item.TinhTrangStr = Common.GetTinhTrangPhong(item.TinhTrang.Value);
                    }
                }
            }
            return View(data.Results.ToList());
        }
        public IActionResult PaymentCallBack(string vnp_Amount, string vnp_BankCode, string vnp_BankTranNo, string vnp_CardType, string vnp_OrderInfo, string vnp_PayDate, string vnp_ResponseCode, string vnp_TmnCode, string vnp_TransactionNo,
           string vnp_TransactionStatus, string vnp_TxnRef, string vnp_SecureHash)
        {
            var response = _vnPayservice.PaymentExecute(Request.Query);
            string Message = "";
            if (response == null || vnp_ResponseCode != "00")
            {
                Message = $"Lỗi thanh toán VN Pay: {response.VnPayResponseCode}";
            }

            var data = _httpContextAccessor.HttpContext.Session.GetString("OrdersModelView");
            if (data != null)
            {
                var dataConvert = JsonConvert.DeserializeObject<HopDongModelView>(data);
                if (dataConvert != null)
                {
                    var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");

                    _hopDongService.Add(dataConvert);

                    _phongService.UpdateTrangThai(dataConvert.MaPhong.Value, 2); // đã thuê
                                                                                 // Add thành viên phòng
                    var thanhvienphong = new ThanhVienPhongModelView();
                    thanhvienphong.MaKH = dataConvert.MaKH;
                    thanhvienphong.MaPhong = dataConvert.MaPhong;
                    _thanhVienPhongService.Add(thanhvienphong);
                    _hopDongService.Save();
                    _httpContextAccessor.HttpContext.Session.Remove("OrdersModelView");
                    Message = "Thanh toán thành công";
                }
            }
            TempData["Message"] = Message;
            return View();
        }
        public IActionResult PhongDetail(int phongId)
        {
            var model = new PhongModelView();
            if (phongId > 0)
            {
                model = _phongService.GetByid(phongId);
                if (model != null)
                {
                    if (model.MaNha.HasValue && model.MaNha.Value > 0)
                    {
                        var nha = _nhaService.GetByid(model.MaNha.Value);
                        if (nha != null && !string.IsNullOrEmpty(nha.TenNha))
                        {
                            model.TenNha = nha.TenNha;
                        }
                        if (model.LoaiPhong.HasValue)
                        {
                            model.LoaiPhongStr = Common.GetLoaiPhong(model.LoaiPhong.Value);
                        }
                        if (model.TinhTrang.HasValue)
                        {
                            model.TinhTrangStr = Common.GetTinhTrangPhong(model.TinhTrang.Value);
                        }
                        var UserRooms = _thanhVienPhongService.GetThanhVienByPhong(model.Id);
                        if (UserRooms != null)
                        {
                            model.SoLuongNguoiDangO = UserRooms.Count();
                        }
                    }
                    else
                    {
                        model.TenNha = "";
                    }
                }
            }
            return View(model);
        }


        public IActionResult ProductSearch(string textSearch)
        {
            //var data = new List<ProductModelView>();
            if (!string.IsNullOrEmpty(textSearch))
            {
                var ProductModelViewSearch = new PhongViewModelSearch();
                ProductModelViewSearch.name = textSearch.Replace("timkiem=", "");
                ProductModelViewSearch.PageIndex = 1;
                ProductModelViewSearch.PageSize = 20;
                //ProductModelViewSearch.status = 1;
                if (!string.IsNullOrEmpty(ProductModelViewSearch.name))
                {
                    ProductModelViewSearch.status = Common.GetTinhTrangPhongForText(ProductModelViewSearch.name);
                    ProductModelViewSearch.loaiphong = Common.GetLoaiPhongForText(ProductModelViewSearch.name);
                    if (ProductModelViewSearch.status > 0 || ProductModelViewSearch.loaiphong>0)
                    {
                        ProductModelViewSearch.name = "";
                    }
                }
                var data = _phongService.GetAllPaging(ProductModelViewSearch);
                if (data != null && data.Results != null && data.Results.Count > 0)
                {
                    //data.Results = data.Results.Where(p => p.ishidden != 1).ToList();
                    foreach (var item in data.Results)
                    {
                        if (item.MaNha.HasValue && item.MaNha.Value > 0)
                        {
                            var category = _nhaService.GetByid(item.MaNha.Value);
                            if (category != null && !string.IsNullOrEmpty(category.TenNha))
                            {
                                item.TenNha = category.TenNha;
                            }
                            else
                            {
                                item.TenNha = "Chờ xử lý";
                            }
                        }
                        else
                        {
                            item.TenNha = "";
                        }
                        if (item.LoaiPhong.HasValue)
                        {
                            item.LoaiPhongStr = Common.GetLoaiPhong(item.LoaiPhong.Value);
                        }
                        if (item.TinhTrang.HasValue)
                        {
                            item.TinhTrangStr = Common.GetTinhTrangPhong(item.TinhTrang.Value);
                        }
                        var UserRooms = _thanhVienPhongService.GetThanhVienByPhong(item.Id);
                        if (UserRooms != null)
                        {
                            item.SoLuongNguoiDangO = UserRooms.Count();
                        }
                    }
                }
                return View(data.Results.ToList());
            }
            return View();
        }


        [HttpGet]
        public JsonResult AddView()
        {
            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var user = new UserMapModelView();
            if (userString != null)
            {
                user = JsonConvert.DeserializeObject<UserMapModelView>(userString);
                if (user != null)
                    return Json(new
                    {
                        success = true,
                        data = user
                    });
            }
            return Json(new
            {
                success = false,
            });
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}