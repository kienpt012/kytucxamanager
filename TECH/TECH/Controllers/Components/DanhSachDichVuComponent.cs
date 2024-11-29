using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TECH.Service;

namespace TECH.Controllers.Components

{
    [ViewComponent(Name = "DanhSachDichVuComponent")]
    public class DanhSachDichVuComponent : ViewComponent
    {
        private readonly IDichVuService _dichVuService;
        public DanhSachDichVuComponent(IDichVuService dichVuService)
        {
            _dichVuService = dichVuService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {           
            var danhsachdichvu = _dichVuService.GetAll();
            return View(danhsachdichvu);
        }
    }
}