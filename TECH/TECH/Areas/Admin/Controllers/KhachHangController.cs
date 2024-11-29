using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Service;

namespace TECH.Areas.Admin.Controllers
{
    public class KhachHangController : BaseController
    {
        private readonly IKhachHangService _khachhangService;
        public IHttpContextAccessor _httpContextAccessor;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        public KhachHangController(IKhachHangService nhanVienService,
             IHttpContextAccessor httpContextAccessor,
             Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _khachhangService = nhanVienService;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetById(int id)
        {
            var model = new KhachHangModelView();
            if (id > 0)
            {
                model = _khachhangService.GetByid(id);
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
        public JsonResult Add(KhachHangModelView KhachHangModelView)
        {
            bool isMailExist = false;
            bool isPhoneExist = false;
            if (KhachHangModelView != null && !string.IsNullOrEmpty(KhachHangModelView.Email))
            {
                var isMail = _khachhangService.IsMailExist(KhachHangModelView.Email);
                if (isMail)
                {
                    isMailExist = true;
                }
            }

            if (KhachHangModelView != null && !string.IsNullOrEmpty(KhachHangModelView.SoDienThoai))
            {
                var isPhone = _khachhangService.IsPhoneExist(KhachHangModelView.SoDienThoai);
                if (isPhone)
                {
                    isPhoneExist = true;
                }
            }

            if (!isMailExist && !isPhoneExist)
            {
                _khachhangService.Add(KhachHangModelView);
                _khachhangService.Save();
                return Json(new
                {
                    success = true
                });
            }
            return Json(new
            {
                success = false,
                isMailExist = isMailExist,
                isPhoneExist = isPhoneExist
            });
        }

        [HttpPost]
        public JsonResult Update(KhachHangModelView KhachHangModelView)
        {
            var result = _khachhangService.Update(KhachHangModelView);
            _khachhangService.Save();
            return Json(new
            {
                success = result
            });

        }

        [HttpGet]
        public JsonResult GetAll()
        {
            var data = _khachhangService.GetAll();
            return Json(new { Data = data });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var result = _khachhangService.Deleted(id);
            _khachhangService.Save();
            return Json(new
            {
                success = result
            });
        }

        [HttpGet]
        public JsonResult GetAllPaging(KhachHangViewModelSearch colorViewModelSearch)
        {
            var data = _khachhangService.GetAllPaging(colorViewModelSearch);
            return Json(new { data = data });
        }
        [HttpPost]
        public JsonResult UpdateViewDetail(UserMapModelView UserModelView)
        {
            bool status = false;
            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var model = new UserMapModelView();
            if (userString != null)
            {
                var user = JsonConvert.DeserializeObject<UserMapModelView>(userString);
                if (user != null)
                {
                    var dataUser = _khachhangService.GetByid(user.Id);
                    if (dataUser != null)
                    {

                        if (dataUser.Email.ToLower().Trim() != UserModelView.Email.ToLower().Trim())
                        {
                            bool isEmail = _khachhangService.IsMailExist(UserModelView.Email.ToLower().Trim());
                            if (isEmail)
                            {
                                return Json(new
                                {
                                    success = false,
                                    isExistEmail = true,
                                    isExistPhone = false,
                                });
                            }
                        }

                        if (dataUser.SoDienThoai.ToLower().Trim() != UserModelView.SDT.ToLower().Trim())
                        {
                            bool isPhone = _khachhangService.IsPhoneExist(UserModelView.SDT.ToLower().Trim());
                            if (isPhone)
                            {
                                return Json(new
                                {
                                    success = false,
                                    isExistEmail = false,
                                    isExistPhone = true,
                                });
                            }
                        }

                        UserModelView.Id = dataUser.Id;
                        status = _khachhangService.UpdateDetailView(UserModelView);
                        _khachhangService.Save();
                        dataUser = _khachhangService.GetByid(user.Id);
                        _httpContextAccessor.HttpContext.Session.SetString("UserInfor", JsonConvert.SerializeObject(dataUser));
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
    }
}
