using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Service;

namespace TECH.Areas.Admin.Controllers
{
    public class NhaController : BaseController
    {
        private readonly INhaService _nhaService;
        private readonly IThanhPhoService _thanhPhoService;
        private readonly IQuanHuyenService _quanHuyenService;
        private readonly IPhuongXaService _phuongXaService;
        public IHttpContextAccessor _httpContextAccessor;
        public NhaController(INhaService nhaService,
            IThanhPhoService thanhPhoService,
            IQuanHuyenService quanHuyenService,
            IPhuongXaService phuongXaService
            )
        {
            _nhaService = nhaService;
            _thanhPhoService = thanhPhoService;
            _quanHuyenService = quanHuyenService;
            _phuongXaService = phuongXaService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetById(int id)
        {
            var model = new NhaModelView();
            if (id > 0)
            {
                model = _nhaService.GetByid(id);
            }
            return Json(new
            {
                Data = model
            });
        }        
        [HttpGet]
        public IActionResult AddOrUpdate()
        {
            return View();
        }
        
        [HttpPost]
        public JsonResult Add(NhaModelView NhaModelView)
        {
            if (_nhaService.IsExist(NhaModelView.TenNha))
            {
                return Json(new
                {
                    success = false
                });
            }
            _nhaService.Add(NhaModelView);
            _nhaService.Save();
            return Json(new
            {
                success = true
            });
        }       

        [HttpPost]
        public JsonResult Update(NhaModelView NhaModelView)
        {           
            var result = _nhaService.Update(NhaModelView);
            _nhaService.Save();
            return Json(new
            {
                success = result
            });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var result = _nhaService.Deleted(id);
            _nhaService.Save();
            return Json(new
            {
                success = result
            });
        }
        [HttpGet]
        public JsonResult GetAll()
        {
            var data = _nhaService.GetAll();            
            return Json(new { Data = data });
        }

        [HttpGet]
        public JsonResult GetAllPaging(NhaViewModelSearch Search)
        {
            var data = _nhaService.GetAllPaging(Search);
            if (data != null && data.Results != null && data.Results.Count > 0)
            {                
                foreach (var item in data.Results)
                {
                    if (item.MaTP.HasValue && item.MaTP.Value > 0)
                    {
                        var dataThanhPho = _thanhPhoService.GetById(item.MaTP.Value);
                        if (dataThanhPho != null && !string.IsNullOrEmpty(dataThanhPho.Ten))
                        {
                            item.MaTPStr = dataThanhPho.Ten;
                        }
                        var dataQuanHuyen = _quanHuyenService.GetByid(item.MaQH.Value);
                        if (dataQuanHuyen != null && !string.IsNullOrEmpty(dataQuanHuyen.Ten))
                        {
                            item.MaQHStr = dataQuanHuyen.Ten;
                        }
                        var dataPhuongXa = _phuongXaService.GetById(item.MaPX.Value);
                        if (dataPhuongXa != null && !string.IsNullOrEmpty(dataPhuongXa.Ten))
                        {
                            item.MaPXStr = dataPhuongXa.Ten;
                        }
                    }
                }
            }
            return Json(new { data = data });
        }
        [HttpGet]
        public JsonResult GetAllCity()
        {
            var model = _thanhPhoService.GetAll();
            return Json(new
            {
                Data = model
            });
        }

        [HttpGet]
        public JsonResult GetAllDistricts()
        {
            var model = _quanHuyenService.GetAll();
            return Json(new
            {
                Data = model
            });
        }

        [HttpGet]
        public JsonResult GetDistrictsForCityId(int cityId)
        {
            var model = _quanHuyenService.GetDistrictForCityId(cityId);
            return Json(new
            {
                Data = model
            });
        }

        [HttpGet]
        public JsonResult GetWardsForDistrictId(int districtId)
        {
            var model = _phuongXaService.GetWardsForDistrictId(districtId);
            return Json(new
            {
                Data = model
            });
        }


        [HttpGet]
        public JsonResult GetAllWards()
        {
            var model = _phuongXaService.GetAll();
            return Json(new
            {
                Data = model
            });
        }
    }
}
