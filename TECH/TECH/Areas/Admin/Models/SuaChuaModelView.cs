using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TECH.Data.DatabaseEntity;

namespace TECH.Areas.Admin.Models
{
    public class SuaChuaModelView
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public KhachHangModelView? KhachHang { get; set; }

        public int? MaPhong { get; set; }
        public PhongModelView? Phong { get; set; }

        public DateTime? NgayTao { get; set; }
        public string? NgayTaoStr { get; set; }
        public int? Status { get; set; }

        public string? StatusStr { get; set; }

        public string? Comment { get; set; }
    }
}
