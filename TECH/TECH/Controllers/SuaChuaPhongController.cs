using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Service;
using System.Text.RegularExpressions;
using TECH.General;
using Newtonsoft.Json;

namespace TECH.Controllers
{
    public class SuaChuaPhongController : Controller
    {
        private readonly IHopDongService _hopDongService;
        private readonly ISuaChuaService _suaChuaService;
        private readonly INhaService _nhaService;
        private readonly IPhongService _phongService;
        private readonly IKhachHangService _khachHangService;
        private readonly INhanVienService _nhanVienService;
        public IHttpContextAccessor _httpContextAccessor;
        public SuaChuaPhongController(IHopDongService hopDongService,
            INhaService nhaService,
            ISuaChuaService suaChuaService,
            IPhongService phongService,
            IKhachHangService khachHangService,
            INhanVienService nhanVienService,
            IDichVuPhongService dichVuPhongService,
            IHttpContextAccessor httpContextAccessor,
            IThanhVienPhongService thanhVienPhongService
            )
        {
            _suaChuaService = suaChuaService;
            _hopDongService = hopDongService;
            _nhaService = nhaService;
            _phongService = phongService;
            _khachHangService = khachHangService;
            _nhanVienService = nhanVienService;
            //_dichVuPhongService = dichVuPhongService;
            //_thanhVienPhongService = thanhVienPhongService;
            _httpContextAccessor = httpContextAccessor;
            //_nhaService = nhaService;
        }
        public IActionResult Index()
        {
            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var user = new UserMapModelView();
            if (!string.IsNullOrEmpty(userString))
            {
                user = JsonConvert.DeserializeObject<UserMapModelView>(userString);
                SuaChuaViewModelSearch phongViewModelSearch = new SuaChuaViewModelSearch()
                {
                    PageSize = 1000,
                    PageIndex = 1,
                    UserId = user?.Id
                };
                var data = _suaChuaService.GetAllPaging(phongViewModelSearch);
                if (data != null && data.Results != null && data.Results.Count > 0)
                {
                    foreach (var item in data.Results)
                    {
                        if (item.MaPhong.HasValue && item.MaPhong.Value > 0)
                        {
                            item.Phong = _phongService.GetByid(item.MaPhong.Value);
                        }
                        if (item.UserId.HasValue && item.UserId.Value > 0)
                        {
                            item.KhachHang = _khachHangService.GetByid(item.UserId.Value);
                        }
                        if (item.Status.HasValue && item.Status.Value > 0)
                        {
                            item.StatusStr = Common.GetStatusSuaChua(item.Status.Value);
                        }
                    }
                    return View(data.Results.ToList());
                }                
            }
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

        [HttpGet]
        public JsonResult GetAllPaging(HopDongViewModelSearch phongViewModelSearch)
        {
            phongViewModelSearch.PageIndex = 1;
            phongViewModelSearch.PageSize = 1;
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
                data.Results = data.Results.Where(p => p.TenNha.Contains(phongViewModelSearch.name) ||
                p.TenPhong.Contains(phongViewModelSearch.name) ||
                p.TenKhachHang.Contains(phongViewModelSearch.name) ||
                p.TenNhanVien.Contains(phongViewModelSearch.name)).ToList();
            }
            if (phongViewModelSearch.status > 0)
            {
                data.Results = data.Results.Where(p=>p.TrangThai == phongViewModelSearch.status).ToList();
            }
            return Json(new { data = data });
        }

    }
}
