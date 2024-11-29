using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TECH.Data.DatabaseEntity;

namespace TECH.Areas.Admin.Models
{
    public class ThanhVienPhongModelView
    {
        public int Id { get; set; }
        public int? MaKH { get; set; }
        public KhachHangModelView? KhachHang { get; set; }
        public int? MaPhong { get; set; }
        public PhongModelView? Phong { get; set; }
    }
}
