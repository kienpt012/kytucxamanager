using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TECH.Data.DatabaseEntity;

namespace TECH.Areas.Admin.Models
{
    public class HoaDonModelView
    {
        public int Id { get; set; }
        public int? MaHopDong { get; set; }
        public int? MaKH { get; set; }
        public HopDongModelView HopDong { get; set; }
        public List<int> MaPhongs { get; set; }
        public string? NguoiDong { get; set; }
        public DateTime? NgayDongTien { get; set; }
        public string? NgayDongTienStr { get; set; }
        public DateTime? HanDong { get; set; }
        public string? HanDongStr { get; set; }
        public int? TrangThai { get; set; }
        public string? TrangThaiStr { get; set; }
        public string? GhiChu { get; set; }

        public decimal? TongTien { get; set; }
        public string? TongTienStr { get; set; }
        public decimal TienDaDong { get; set; }
        public decimal TienNo { get; set; }

        public decimal? TienDong { get; set; }
        public string? TienDongStr { get; set; }
        public bool? IsDeteled { get; set; }
        public decimal? SoDienMoi { get; set; }
        public decimal? SoNuocMoi { get; set; }
        public decimal? SoDienCu { get; set; }
        public decimal? SoNuocCu { get; set; }
        public decimal? SoDienSuDung { get; set; }
        public decimal? SoNuocSuDung { get; set; }
    }
    public class HoaDonModelViewStatistical
    {
        public int TotalDaDong { get; set; }
        public int TotalChuaDong { get; set; }
        public int TotalConNo { get; set; }
    }
}
