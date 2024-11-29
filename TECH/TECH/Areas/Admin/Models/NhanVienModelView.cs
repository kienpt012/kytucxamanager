using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TECH.Areas.Admin.Models
{
    public class NhanVienModelView
    {
        public int Id { get; set; }
        public string? TenNV { get; set; }
        public string? SoDienThoai { get; set; }
        public string? Email { get; set; }
        public string? CMND { get; set; }
        public string? GioiTinh { get; set; }
        public string? GioiTinhStr { get; set; }
        public string? DiaChi { get; set; }
        public string? TenDangNhap { get; set; }
        public string? MatKhau { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string? NgaySinhStr { get; set; }
        public int? Role { get; set; }
        public string? RoleStr { get; set; }
    }
}
