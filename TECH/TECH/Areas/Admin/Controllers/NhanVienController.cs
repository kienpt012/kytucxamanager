using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Service;

namespace TECH.Areas.Admin.Controllers
{
    public class NhanVienController : BaseController
    {
        private readonly INhanVienService _nhanVienService;
        public IHttpContextAccessor _httpContextAccessor;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        public NhanVienController(INhanVienService nhanVienService,
             IHttpContextAccessor httpContextAccessor,
             Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _nhanVienService = nhanVienService;
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
            var model = new NhanVienModelView();
            if (id > 0)
            {
                model = _nhanVienService.GetByid(id);
            }
            return Json(new
            {
                Data = model
            });
        }


        public IActionResult ViewDetail()
        {
            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var model = new NhanVienModelView();
            if (userString != null)
            {
                var user = JsonConvert.DeserializeObject<NhanVienModelView>(userString);
                if (user != null)
                {
                    var dataUser = _nhanVienService.GetByid(user.Id);
                    model = dataUser;
                }

            }
            return View(model);
        }

        //[HttpPost]
        //public JsonResult ChangeServerPassWord(int userId, string current_password, string new_password)
        //{
        //    var model = _nhanVienService.ChangePassWord(userId, current_password, new_password);
        //    _nhanVienService.Save();
        //    return Json(new
        //    {
        //        success = model
        //    });
        //}

        //[HttpGet]
        //public IActionResult ChangePassWord()
        //{
        //    var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
        //    var model = new NhanVienModelView();
        //    if (userString != null)
        //    {
        //        var user = JsonConvert.DeserializeObject<NhanVienModelView>(userString);
        //        if (user != null)
        //        {
        //            var dataUser = _nhanVienService.GetByid(user.id);
        //            model = dataUser;
        //        }
        //        return View(model);
        //    }
        //    return Redirect("/home");

        //}
        [HttpGet]
        public JsonResult GetAll()
        {
            var data = _nhanVienService.GetAll();
            return Json(new { Data = data });
        }


        [HttpPost]
        public JsonResult ChangeServerPassWord(int userId, string current_password, string new_password)
        {
            var model = _nhanVienService.ChangePassWord(userId, current_password, new_password);
            _nhanVienService.Save();
            return Json(new
            {
                success = model
            });
        }
        public IActionResult LogOut()
        {

            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var user = new NhanVienModelView();
            if (userString != null)
            {
                user = JsonConvert.DeserializeObject<NhanVienModelView>(userString);
                _httpContextAccessor.HttpContext.Session.Remove("UserInfor");
            }

            return Redirect("/");

        }

        [HttpGet]
        public IActionResult ChangePassWord()
        {
            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var model = new NhanVienModelView();
            if (userString != null)
            {
                var user = JsonConvert.DeserializeObject<NhanVienModelView>(userString);
                if (user != null)
                {
                    var dataUser = _nhanVienService.GetByid(user.Id);
                    model = dataUser;
                }
                return View(model); 
            }
            return Redirect("/home");

        }
        [HttpPost]
        public JsonResult UpdateViewDetail(NhanVienModelView NhanVienModelView)
        {
            bool status = false;
            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var model = new NhanVienModelView();
            if (userString != null)
            {
                var user = JsonConvert.DeserializeObject<NhanVienModelView>(userString);
                if (user != null)
                {
                    var dataUser = _nhanVienService.GetByid(user.Id);
                    if (dataUser != null)
                    {

                        if (dataUser.Email.ToLower().Trim() != NhanVienModelView.Email.ToLower().Trim())
                        {
                            bool isEmail = _nhanVienService.IsMailExist(NhanVienModelView.Email.ToLower().Trim());
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

                        if (dataUser.SoDienThoai.ToLower().Trim() != NhanVienModelView.SoDienThoai.ToLower().Trim())
                        {
                            bool isPhone = _nhanVienService.IsPhoneExist(NhanVienModelView.SoDienThoai.ToLower().Trim());
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

                        NhanVienModelView.Id = dataUser.Id;
                        status = _nhanVienService.UpdateDetailView(NhanVienModelView);
                        _nhanVienService.Save();
                        //dataUser = _nhanVienService.GetByid(user.id);
                        //_httpContextAccessor.HttpContext.Session.SetString("UserInfor", JsonConvert.SerializeObject(dataUser));
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


        //[HttpPost]
        //public JsonResult UpdateViewDetail(UserModelView UserModelView)
        //{
        //    bool status = false;
        //    var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
        //    var model = new UserModelView();
        //    if (userString != null)
        //    {
        //        var user = JsonConvert.DeserializeObject<UserModelView>(userString);
        //        if (user != null)
        //        {
        //            var dataUser = _appUserService.GetByid(user.id);
        //            if (dataUser != null)
        //            {

        //                if (dataUser.email.ToLower().Trim() != UserModelView.email.ToLower().Trim())
        //                {
        //                    bool isEmail = _appUserService.IsMailExist(UserModelView.email.ToLower().Trim());
        //                    if (isEmail)
        //                    {
        //                        return Json(new
        //                        {
        //                            success = false,
        //                            isExistEmail = true,
        //                            isExistPhone = false,
        //                        });
        //                    }
        //                }

        //                if (dataUser.phone_number.ToLower().Trim() != UserModelView.phone_number.ToLower().Trim())
        //                {
        //                    bool isPhone = _appUserService.IsPhoneExist(UserModelView.phone_number.ToLower().Trim());
        //                    if (isPhone)
        //                    {
        //                        return Json(new
        //                        {
        //                            success = false,
        //                            isExistEmail = false,
        //                            isExistPhone = true,
        //                        });
        //                    }
        //                }

        //                UserModelView.id = dataUser.id;
        //                status = _appUserService.UpdateDetailView(UserModelView);
        //                _appUserService.Save();
        //                dataUser = _appUserService.GetByid(user.id);
        //                _httpContextAccessor.HttpContext.Session.SetString("UserInfor", JsonConvert.SerializeObject(dataUser));
        //                return Json(new
        //                {
        //                    success = status,
        //                    isExistEmail = false,
        //                    isExistPhone = false,
        //                });
        //            }
        //        }
        //    }







            [HttpGet]
        public IActionResult AddOrUpdate()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult UploadImageAvatar()
        //{
        //    var files = Request.Form.Files;
        //    if (files != null && files.Count > 0)
        //    {
        //        string folder = _hostingEnvironment.WebRootPath + $@"\avatar";

        //        if (!Directory.Exists(folder))
        //        {
        //            Directory.CreateDirectory(folder);
        //        }
        //        var _lstImageName = new List<string>();

        //        foreach (var itemFile in files)
        //        {
        //            string filePath = Path.Combine(folder, itemFile.FileName);
        //            if (!System.IO.File.Exists(filePath))
        //            {
        //                _lstImageName.Add(itemFile.FileName);
        //                using (FileStream fs = System.IO.File.Create(filePath))
        //                {
        //                    itemFile.CopyTo(fs);
        //                    fs.Flush();
        //                }
        //            }
        //        }                
        //    }
        //    return Json(new
        //    {
        //        success = true
        //    });
        //}

        //[HttpPost]
        //public JsonResult IsEmailExist(string email)
        //{
        //    bool isMail = false;
        //    if (!string.IsNullOrEmpty(email))
        //    {
        //        isMail = _nhanVienService.IsMailExist(email);
        //    }

        //    return Json(new
        //    {
        //        success = isMail
        //    });
        //}

        //[HttpPost]
        //public JsonResult IsPhoneExist(string phone)
        //{
        //    bool isphone = false;
        //    if (!string.IsNullOrEmpty(phone))
        //    {
        //        isphone = _nhanVienService.IsPhoneExist(phone);
        //    }

        //    return Json(new
        //    {
        //        success = isphone
        //    }) ;
        //}


        //[HttpPost]
        //public IActionResult UploadImageAvartar()
        //{
        //    var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
        //    if (!string.IsNullOrEmpty(userString))
        //    {
        //        var model = new NhanVienModelView();
        //        var user = JsonConvert.DeserializeObject<NhanVienModelView>(userString);
        //        if (user != null)
        //        {
        //            var dataUser = _nhanVienService.GetByid(user.Id);
        //            if (dataUser != null)
        //            {
        //                var _lstImageName = new List<string>();
        //                var files = Request.Form.Files;
        //                if (files != null && files.Count > 0)
        //                {
        //                    var imageFolder = $@"\avartar\";

        //                    string folder = _hostingEnvironment.WebRootPath + imageFolder;

        //                    if (!Directory.Exists(folder))
        //                    {
        //                        Directory.CreateDirectory(folder);
        //                    }


        //                    foreach (var itemFile in files)
        //                    {
        //                        string fileNameFormat = Regex.Replace(itemFile.FileName.ToLower(), @"\s+", "");
        //                        string filePath = Path.Combine(folder, fileNameFormat);
        //                        if (!System.IO.File.Exists(filePath))
        //                        {
        //                            _lstImageName.Add(fileNameFormat);
        //                            using (FileStream fs = System.IO.File.Create(filePath))
        //                            {
        //                                itemFile.CopyTo(fs);
        //                                fs.Flush();
        //                            }
        //                        }
        //                    }
        //                }


        //                if (_lstImageName != null && _lstImageName.Count > 0)
        //                {
        //                    foreach (var item in _lstImageName)
        //                    {
        //                        var NhanVienModelView = new NhanVienModelView();
        //                        NhanVienModelView.avatar = item;
        //                        NhanVienModelView.id = dataUser.id;
        //                        _nhanVienService.UpdateAvartar(NhanVienModelView);
        //                        _nhanVienService.Save();
        //                        user.avatar = item;
        //                    }

        //                }

        //                _httpContextAccessor.HttpContext.Session.SetString("UserInfor", JsonConvert.SerializeObject(user));
        //            }
        //        }
        //    }
        //    return Json(new
        //    {
        //        success = true
        //    });

        //}





        [HttpPost]
        public JsonResult Add(NhanVienModelView NhanVienModelView)
        {
            bool isMailExist = false;
            bool isPhoneExist = false;
            //if (NhanVienModelView != null && !string.IsNullOrEmpty(NhanVienModelView.email))
            //{
            //    var isMail = _nhanVienService.IsMailExist(NhanVienModelView.email);
            //    if (isMail)
            //    {
            //        isMailExist = true;
            //    }
            //}

            //if (NhanVienModelView != null && !string.IsNullOrEmpty(NhanVienModelView.phone_number))
            //{
            //    var isPhone = _nhanVienService.IsPhoneExist(NhanVienModelView.phone_number);
            //    if (isPhone)
            //    {
            //        isPhoneExist = true;
            //    }
            //}

            if (!isMailExist && !isPhoneExist)
            {
                _nhanVienService.Add(NhanVienModelView);
                _nhanVienService.Save();
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

        //[HttpGet]
        //public JsonResult UpdateStatus(int id, int status)
        //{
        //    if (id > 0)
        //    {
        //        var model = _nhanVienService.UpdateStatus(id, status);
        //        _nhanVienService.Save();
        //        return Json(new
        //        {
        //            success = model
        //        });
        //    }
        //    return Json(new
        //    {
        //        success = false
        //    });

        //}

        [HttpPost]
        public JsonResult Update(NhanVienModelView NhanVienModelView)
        {

            //bool isMailExist = false;
            //bool isPhoneExist = false;
            //if (NhanVienModelView != null && !string.IsNullOrEmpty(NhanVienModelView.email))
            //{
            //    var isMail = _nhanVienService.IsMailExist(NhanVienModelView.email);
            //    if (isMail)
            //    {
            //        isMailExist = true;
            //    }
            //}

            //if (NhanVienModelView != null && !string.IsNullOrEmpty(NhanVienModelView.phone_number))
            //{
            //    var isPhone = _nhanVienService.IsPhoneExist(NhanVienModelView.phone_number);
            //    if (isPhone)
            //    {
            //        isPhoneExist = true;
            //    }
            //}

            var result = _nhanVienService.Update(NhanVienModelView);
            _nhanVienService.Save();
            return Json(new
            {
                success = result
            });



        }

        //[HttpPost]
        //public JsonResult AddUserRoles (int userId, int[] rolesId)
        //{
        //    try
        //    {
        //        _appUserRoleService.AddAppUserRole(userId, rolesId);

        //        return Json(new
        //        {
        //            success = true
        //        });
        //    }
        //    catch (Exception)
        //    {
        //        return Json(new
        //        {
        //            success = false
        //        });
        //    }

        //}

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var result = _nhanVienService.Deleted(id);
            _nhanVienService.Save();
            return Json(new
            {
                success = result
            });
        }

        [HttpGet]
        public JsonResult GetAllPaging(NhanVienViewModelSearch colorViewModelSearch)
        {
            var data = _nhanVienService.GetAllPaging(colorViewModelSearch);
            return Json(new { data = data });
        }
        //[HttpGet]
        //public JsonResult GetAll()
        //{
        //    var data = _nhanVienService.GetAll();
        //    return Json(new { Data = data });
        //}
    }
}
