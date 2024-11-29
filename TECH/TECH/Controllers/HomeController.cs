using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.General;
using TECH.Models;
using TECH.Service;

namespace TECH.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IPhongService _phongService;
        private readonly INhaService _nhaService;
        private readonly IDichVuService _dichVuService;
        private readonly IThanhVienPhongService _thanhVienPhongService;
        public HomeController(IPhongService phongService, INhaService nhaService, IDichVuService dichVuService, IThanhVienPhongService thanhVienPhongService)
        {
            _phongService = phongService;
            _nhaService = nhaService;
            _dichVuService = dichVuService;
            _thanhVienPhongService = thanhVienPhongService;
        }

        public IActionResult Index()
        {
            PhongViewModelSearch phongViewModelSearch = new PhongViewModelSearch();
            phongViewModelSearch.PageIndex = 1;
            phongViewModelSearch.PageSize = 200;
            //phongViewModelSearch.status = 1;
            var data = _phongService.GetAllPaging(phongViewModelSearch);
            if (data != null && data.Results != null && data.Results.Count > 0)
            {
                foreach (var item in data.Results)
                {
                    if (item.MaNha.HasValue && item.MaNha.Value > 0)
                    {
                        var nha = _nhaService.GetByid(item.MaNha.Value);
                        if (nha != null && !string.IsNullOrEmpty(nha.TenNha))
                        {
                            item.TenNha = nha.TenNha;
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
                    else
                    {
                        item.TenNha = "";
                    }
                    var UserRooms = _thanhVienPhongService.GetThanhVienByPhong(item.Id);
                    item.SoLuongNguoiDangO = UserRooms.Count();
                    // get thành viên phòng

                }
                if (phongViewModelSearch != null && !string.IsNullOrEmpty(phongViewModelSearch.name))
                {
                    data.Results = data.Results.Where(p => p.TenNha.Contains(phongViewModelSearch.name) ||
                    p.TenPhong.Contains(phongViewModelSearch.name) ||
                    p.TinhTrangStr.Contains(phongViewModelSearch.name)).ToList();
                }
                if (phongViewModelSearch.status > 0)
                {
                    if (phongViewModelSearch.status == 1)
                    {
                        data.Results = data.Results.Where(p => p.TinhTrang == phongViewModelSearch.status).ToList();
                    }
                    else
                    {
                        data.Results = data.Results.Where(p => p.TinhTrang != 1).ToList();
                    }

                }
            }
            var dataPhong = data.Results.GroupBy(p=>p.TenNha).ToDictionary(p=>p.Key, p=>p.ToList());
            var danhsachdichvu = _dichVuService.GetAll();
            var homeModelView = new HomeModelViewCustomer
            {
                PhongModelViews = dataPhong,
                DichVus = danhsachdichvu
            };
            return View(homeModelView);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult ThanhToan()
        {
            return View();
        }
        public IActionResult DieuKhoanSuDung()
        {
            return View();
        }
        public IActionResult GiaiQuyetKhieuNai()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}