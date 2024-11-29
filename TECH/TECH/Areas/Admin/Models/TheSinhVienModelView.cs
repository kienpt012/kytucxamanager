using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TECH.Data.DatabaseEntity;

namespace TECH.Areas.Admin.Models
{
    public class TheSinhVienModelView
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public KhachHangModelView? KhachHang { get; set; }
        public string? MaThe { get; set; }
        public DateTime? NgayTaoThe { get; set; }
        public string? NgayTaoTheStr { get; set; }
        public DateTime? NgayHetHan { get; set; }
        public string? NgayHetHanStr { get; set; }
        public int? Status { get; set; }
        public string? StatusStr{ get; set; }
        public string? Comment { get; set; }
    }
}
