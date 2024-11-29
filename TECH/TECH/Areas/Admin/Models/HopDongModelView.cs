using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TECH.Data.DatabaseEntity;

namespace TECH.Areas.Admin.Models
{
    public class HopDongModelView
    {
        public int Id { get; set; }
        public int? MaPhong { get; set; }
        public string? TenPhong { get; set; }
        public PhongModelView Phong { get; set; }
        public int? MaNha { get; set; }
        public NhaModelView Nha { get; set; }
        public string? TenNha { get; set; }
        public int? MaNV { get; set; }
        public string? TenNhanVien { get; set; }
        public int? MaKH { get; set; }
        public KhachHangModelView KhachHang { get; set; }
        public string? TenKhachHang { get; set; }
        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public string? NgayBatDauStr { get; set; }
        public string? NgayKetThucStr { get; set; }
        public decimal? TienCoc { get; set; }
        public int? TrangThai { get; set; }
        public int TrangThaiPayment { get; set; }
        public string? TrangThaiStr { get; set; }
        public string? GhiChu { get; set; }

        public decimal TongTien { get; set; }
        public string? TongTienStr { get; set; }
        public decimal TienDaDong { get; set; }
        public decimal TienNo { get; set; }
        public bool? IsDeteled { get; set; }
    }
}
