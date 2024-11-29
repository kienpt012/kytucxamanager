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
    public class HopDongController : Controller
    {
        private readonly IHopDongService _hopDongService;
        private readonly INhaService _nhaService;
        private readonly IPhongService _phongService;
        private readonly IDichVuPhongService _dichVuPhongService;
        private readonly IKhachHangService _khachHangService;
        private readonly INhanVienService _nhanVienService;
        private readonly IThanhVienPhongService _thanhVienPhongService;
        public IHttpContextAccessor _httpContextAccessor;
        public HopDongController(IHopDongService hopDongService,
            INhaService nhaService,
            IPhongService phongService,
            IKhachHangService khachHangService,
            INhanVienService nhanVienService,
            IDichVuPhongService dichVuPhongService,
            IHttpContextAccessor httpContextAccessor,
            IThanhVienPhongService thanhVienPhongService
            )
        {
            _hopDongService = hopDongService;
            _nhaService = nhaService;
            _phongService = phongService;
            _khachHangService = khachHangService;
            _nhanVienService = nhanVienService;
            _dichVuPhongService = dichVuPhongService;
            _thanhVienPhongService = thanhVienPhongService;
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
                HopDongViewModelSearch phongViewModelSearch = new HopDongViewModelSearch()
                {
                    PageSize = 100,
                    PageIndex = 1,
                    maKH = user?.Id
                };
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
                    data.Results = data.Results.Where(p => p.TrangThai == phongViewModelSearch.status).ToList();
                }
                return View(data.Results.ToList());
            }
           
            return Redirect("/");
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
