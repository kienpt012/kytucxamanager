using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Service;

namespace TECH.Areas.Admin.Controllers
{
    public class SuaChuaController : BaseController
    {
        private readonly ISuaChuaService _suaChuaService;
        private readonly IPhongService _phongService;
        private readonly IKhachHangService _khachhangService;
        public IHttpContextAccessor _httpContextAccessor;
        public SuaChuaController(ISuaChuaService theSinhVienService,
            IPhongService phongService,
            IKhachHangService khachhangService)
        {
            _suaChuaService = theSinhVienService;
            _khachhangService = khachhangService;
            _phongService = phongService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetById(int id)
        {
            var model = new SuaChuaModelView();
            if (id > 0)
            {
                model = _suaChuaService.GetByid(id);
            }
            return Json(new
            {
                Data = model
            });
        }
        [HttpGet]
        public JsonResult GetAllDichVu()
        {
            var model = _suaChuaService.GetAll();
            return Json(new
            {
                Data = model
            });
        }

        public IActionResult ViewDetail()
        {
            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var model = new SuaChuaModelView();
            if (userString != null)
            {
                var user = JsonConvert.DeserializeObject<SuaChuaModelView>(userString);
                if (user != null)
                {
                    var dataUser = _suaChuaService.GetByid(user.Id);
                    model = dataUser;
                }

            }
            return View(model);
        }


        [HttpPost]
        public JsonResult UpdateViewDetail(SuaChuaModelView SuaChuaModelView)
        {
            bool status = false;
            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var model = new SuaChuaModelView();
            if (userString != null)
            {
                var user = JsonConvert.DeserializeObject<SuaChuaModelView>(userString);
                if (user != null)
                {
                    var dataUser = _suaChuaService.GetByid(user.Id);
                    if (dataUser != null)
                    {                        
                        SuaChuaModelView.Id = dataUser.Id;
                        status = _suaChuaService.Update(SuaChuaModelView);
                        _suaChuaService.Save();
                        return Json(new
                        {
                            success = status,
                            isExistEmail = false,
                            isExistPhone = false,
                        });
                    }
                }
            }


            return Json(new
            {
                success = status
            });
        }




        [HttpGet]
        public IActionResult AddOrUpdate()
        {
            return View();
        }
        
        [HttpPost]
        public JsonResult Add(SuaChuaModelView SuaChuaModelView)
        {
            
            _suaChuaService.Add(SuaChuaModelView);
            _suaChuaService.Save();
            return Json(new
            {
                success = true
            });
            //return Json(new
            //{
            //    success = false,
            //    //isMailExist = isMailExist,
            //    //isPhoneExist = isPhoneExist
            //});

        }       

        [HttpPost]
        public JsonResult Update(SuaChuaModelView SuaChuaModelView)
        {           
            var result = _suaChuaService.Update(SuaChuaModelView);
            _suaChuaService.Save();
            return Json(new
            {
                success = result
            });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var result = _suaChuaService.Deleted(id);
            _suaChuaService.Save();
            return Json(new
            {
                success = result
            });
        }

        [HttpGet]
        public JsonResult GetAllPaging(SuaChuaViewModelSearch Search)
        {
            var data = _suaChuaService.GetAllPaging(Search);
            if (data != null && data.Results != null && data.Results.Count > 0)
            {
                foreach (var item in data.Results)
                {
                    
                    if (item.UserId.HasValue && item.UserId.Value > 0)
                    {
                        var khachHang = _khachhangService.GetByid(item.UserId.Value);
                        if (khachHang != null)
                        {
                            item.KhachHang = khachHang;
                        }
                    }
                    if (item.MaPhong.HasValue && item.MaPhong.Value > 0)
                    {
                        var Phong = _phongService.GetByid(item.MaPhong.Value);
                        if (Phong != null)
                        {
                            item.Phong = Phong;
                        }
                    }
                }
            }
            return Json(new { data = data });
        }
    }
}
