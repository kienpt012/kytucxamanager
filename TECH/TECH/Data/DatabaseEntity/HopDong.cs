using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;
using static TECH.General.General;
namespace TECH.Data.DatabaseEntity
{

    [Table("HopDong")]
    public class HopDong : DomainEntity<int>
    {
        public int? MaPhong { get; set; }
        [ForeignKey("MaPhong")]
        public Phong? Phong { get; set; }

        public int? MaNha { get; set; }
        [ForeignKey("MaNha")]
        public Nha? Nha { get; set; }

        public int? MaNV { get; set; }
        [ForeignKey("MaNV")]
        public NhanVien? NhanVien { get; set; }

        public int? MaKH { get; set; }
        [ForeignKey("MaKH")]
        public KhachHang? KhachHang { get; set; }

        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }

        [Column(TypeName = "decimal(18,0)")]
        public decimal? TienCoc { get; set; }        
        public int? TrangThai { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? GhiChu { get; set; }
        public bool? IsDeteled { get; set; } = false;
    }
}
