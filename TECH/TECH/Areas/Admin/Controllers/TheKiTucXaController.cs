using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Service;

namespace TECH.Areas.Admin.Controllers
{
    public class TheKiTucXaController : BaseController
    {
        private readonly ITheKiTucXaService _theKiTucXaService;
        public IHttpContextAccessor _httpContextAccessor;
        public TheKiTucXaController(ITheKiTucXaService theKiTucXaService)
        {
            _theKiTucXaService = theKiTucXaService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetById(int id)
        {
            var model = new TheKiTucXaModelView();
            if (id > 0)
            {
                model = _theKiTucXaService.GetByid(id);
            }
            return Json(new
            {
                Data = model
            });
        }
        //[HttpGet]
        //public JsonResult GetAllDichVu()
        //{
        //    var model = _theKiTucXaService.GetAll();
        //    return Json(new
        //    {
        //        Data = model
        //    });
        //}

        public IActionResult ViewDetail()
        {
            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var model = new TheKiTucXaModelView();
            if (userString != null)
            {
                var user = JsonConvert.DeserializeObject<TheKiTucXaModelView>(userString);
                if (user != null)
                {
                    var dataUser = _theKiTucXaService.GetByid(user.Id);
                    model = dataUser;
                }

            }
            return View(model);
        }


        [HttpPost]
        public JsonResult UpdateViewDetail(TheKiTucXaModelView TheKiTucXaModelView)
        {
            bool status = false;
            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var model = new TheKiTucXaModelView();
            if (userString != null)
            {
                var user = JsonConvert.DeserializeObject<TheKiTucXaModelView>(userString);
                if (user != null)
                {
                    var dataUser = _theKiTucXaService.GetByid(user.Id);
                    if (dataUser != null)
                    {                        
                        TheKiTucXaModelView.Id = dataUser.Id;
                        status = _theKiTucXaService.Update(TheKiTucXaModelView);
                        _theKiTucXaService.Save();
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
        public JsonResult Add(TheKiTucXaModelView TheKiTucXaModelView)
        {
            if (_theKiTucXaService.IsExist(TheKiTucXaModelView.MaThe))
            {
                return Json(new
                {
                    success = false
                });
            }
            _theKiTucXaService.Add(TheKiTucXaModelView);
            _theKiTucXaService.Save();
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
        public JsonResult Update(TheKiTucXaModelView TheKiTucXaModelView)
        {           
            var result = _theKiTucXaService.Update(TheKiTucXaModelView);
            _theKiTucXaService.Save();
            return Json(new
            {
                success = result
            });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var result = _theKiTucXaService.Deleted(id);
            _theKiTucXaService.Save();
            return Json(new
            {
                success = result
            });
        }

        [HttpGet]
        public JsonResult GetAllPaging(DichVuViewModelSearch Search)
        {
            var data = _theKiTucXaService.GetAllPaging(Search);
            return Json(new { data = data });
        }
    }
}
