using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TECH.Service;

namespace TECH.Controllers.Components

{
    [ViewComponent(Name = "DichVuNewComponent")]
    public class DichVuNewComponent : ViewComponent
    {
        private readonly INhaService _nhaService;
        public DichVuNewComponent(INhaService nhaService)
        {
            _nhaService = nhaService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {           
            var categoryModel = _nhaService.GetAllMenu();
            return View(categoryModel);
        }
    }
}