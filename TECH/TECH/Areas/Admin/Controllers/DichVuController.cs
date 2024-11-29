using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Service;

namespace TECH.Areas.Admin.Controllers
{
    public class DichVuController : BaseController
    {
        private readonly IDichVuService _dichVuService;
        public IHttpContextAccessor _httpContextAccessor;
        public DichVuController(IDichVuService dichVuService)
        {
            _dichVuService = dichVuService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetById(int id)
        {
            var model = new DichVuModelView();
            if (id > 0)
            {
                model = _dichVuService.GetByid(id);
            }
            return Json(new
            {
                Data = model
            });
        }
        [HttpGet]
        public JsonResult GetAllDichVu()
        {
            var model = _dichVuService.GetAll();
            return Json(new
            {
                Data = model
            });
        }

        public IActionResult ViewDetail()
        {
            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var model = new DichVuModelView();
            if (userString != null)
            {
                var user = JsonConvert.DeserializeObject<DichVuModelView>(userString);
                if (user != null)
                {
                    var dataUser = _dichVuService.GetByid(user.Id);
                    model = dataUser;
                }

            }
            return View(model);
        }


        [HttpPost]
        public JsonResult UpdateViewDetail(DichVuModelView DichVuModelView)
        {
            bool status = false;
            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var model = new DichVuModelView();
            if (userString != null)
            {
                var user = JsonConvert.DeserializeObject<DichVuModelView>(userString);
                if (user != null)
                {
                    var dataUser = _dichVuService.GetByid(user.Id);
                    if (dataUser != null)
                    {                        
                        DichVuModelView.Id = dataUser.Id;
                        status = _dichVuService.Update(DichVuModelView);
                        _dichVuService.Save();
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
        public JsonResult Add(DichVuModelView DichVuModelView)
        {
            if (_dichVuService.IsExist(DichVuModelView.TenDV))
            {
                return Json(new
                {
                    success = false
                });
            }
            _dichVuService.Add(DichVuModelView);
            _dichVuService.Save();
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
        public JsonResult Update(DichVuModelView DichVuModelView)
        {           
            var result = _dichVuService.Update(DichVuModelView);
            _dichVuService.Save();
            return Json(new
            {
                success = result
            });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var result = _dichVuService.Deleted(id);
            _dichVuService.Save();
            return Json(new
            {
                success = result
            });
        }

        [HttpGet]
        public JsonResult GetAllPaging(DichVuViewModelSearch Search)
        {
            var data = _dichVuService.GetAllPaging(Search);
            return Json(new { data = data });
        }
    }
}
