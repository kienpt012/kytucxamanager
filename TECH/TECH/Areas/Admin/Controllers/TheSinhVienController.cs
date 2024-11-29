using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Service;

namespace TECH.Areas.Admin.Controllers
{
    public class TheSinhVienController : BaseController
    {
        private readonly ITheSinhVienService _theSinhVienService;
        private readonly IKhachHangService _khachhangService;
        public IHttpContextAccessor _httpContextAccessor;
        public TheSinhVienController(ITheSinhVienService theSinhVienService,
            IKhachHangService khachhangService)
        {
            _theSinhVienService = theSinhVienService;
            _khachhangService = khachhangService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetById(int id)
        {
            var model = new TheSinhVienModelView();
            if (id > 0)
            {
                model = _theSinhVienService.GetByid(id);
            }
            return Json(new
            {
                Data = model
            });
        }
        [HttpGet]
        public JsonResult GetAllDichVu()
        {
            var model = _theSinhVienService.GetAll();
            return Json(new
            {
                Data = model
            });
        }

        public IActionResult ViewDetail()
        {
            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var model = new TheSinhVienModelView();
            if (userString != null)
            {
                var user = JsonConvert.DeserializeObject<TheSinhVienModelView>(userString);
                if (user != null)
                {
                    var dataUser = _theSinhVienService.GetByid(user.Id);
                    model = dataUser;
                }

            }
            return View(model);
        }


        [HttpPost]
        public JsonResult UpdateViewDetail(TheSinhVienModelView TheSinhVienModelView)
        {
            bool status = false;
            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var model = new TheSinhVienModelView();
            if (userString != null)
            {
                var user = JsonConvert.DeserializeObject<TheSinhVienModelView>(userString);
                if (user != null)
                {
                    var dataUser = _theSinhVienService.GetByid(user.Id);
                    if (dataUser != null)
                    {                        
                        TheSinhVienModelView.Id = dataUser.Id;
                        status = _theSinhVienService.Update(TheSinhVienModelView);
                        _theSinhVienService.Save();
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
        public JsonResult Add(TheSinhVienModelView TheSinhVienModelView)
        {
            if (_theSinhVienService.IsExist(TheSinhVienModelView.MaThe))
            {
                return Json(new
                {
                    success = false
                });
            }
            _theSinhVienService.Add(TheSinhVienModelView);
            _theSinhVienService.Save();
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
        public JsonResult Update(TheSinhVienModelView TheSinhVienModelView)
        {           
            var result = _theSinhVienService.Update(TheSinhVienModelView);
            _theSinhVienService.Save();
            return Json(new
            {
                success = result
            });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var result = _theSinhVienService.Deleted(id);
            _theSinhVienService.Save();
            return Json(new
            {
                success = result
            });
        }

        [HttpGet]
        public JsonResult GetAllPaging(TheSinhVienViewModelSearch Search)
        {
            var data = _theSinhVienService.GetAllPaging(Search);
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
                }
            }
            return Json(new { data = data });
        }
    }
}
