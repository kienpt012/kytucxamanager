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

    [Table("HoaDon")]
    public class HoaDon : DomainEntity<int>
    {
        public int? MaHopDong { get; set; }
        [ForeignKey("MaHopDong")]
        public HopDong? HopDong { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        public string? NguoiDong { get; set; }
        public DateTime? NgayDongTien { get; set; }
        public DateTime? HanDong { get; set; }

        [Column(TypeName = "decimal(18,0)")]
        public decimal? TongTien { get; set; }
        [Column(TypeName = "decimal(18,0)")]
        public decimal? TienDong { get; set; }
        public int? TrangThai { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        public string? GhiChu { get; set; }
        
    }
}
