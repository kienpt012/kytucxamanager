using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Service;

namespace TECH.Areas.Admin.Controllers
{
    public class LoiPhamController : BaseController
    {
        private readonly ILoiPhamService _loiPhamService;
        public IHttpContextAccessor _httpContextAccessor;
        public LoiPhamController(ILoiPhamService loiPhamService)
        {
            _loiPhamService = loiPhamService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetById(int id)
        {
            var model = new LoiPhamModelView();
            if (id > 0)
            {
                model = _loiPhamService.GetByid(id);
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
        public JsonResult Add(LoiPhamModelView LoiPhamModelView)
        {
            _loiPhamService.Add(LoiPhamModelView);
            _loiPhamService.Save();
            return Json(new
            {
                success = true
            });
        }       

        [HttpPost]
        public JsonResult Update(LoiPhamModelView LoiPhamModelView)
        {           
            var result = _loiPhamService.Update(LoiPhamModelView);
            _loiPhamService.Save();
            return Json(new
            {
                success = result
            });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var result = _loiPhamService.Deleted(id);
            _loiPhamService.Save();
            return Json(new
            {
                success = result
            });
        }
    }
}
