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

    [Table("ChiTietHoaDon")]
    public class ChiTietHoaDon : DomainEntity<int>
    {
        public int? MaHoaDon { get; set; }
        [ForeignKey("MaHoaDon")]
        public HoaDon? HoaDon { get; set; }

        public int? MaDV { get; set; }
        [ForeignKey("MaDV")]
        public DichVu? DichVu { get; set; }

        public int? MaLoi { get; set; }
        [ForeignKey("MaLoi")]
        public LoiPham? LoiPham { get; set; }

        //[Column(TypeName = "nvarchar(250)")]
        //public string? NguoiDong { get; set; }
        //public DateTime? NgayDongTien { get; set; }        
        public int? ChiSoCu { get; set; }
        public int? ChiSoMoi { get; set; }
        public int? ChiSoDung { get; set; }
        public int? SoLuong { get; set; }

        [Column(TypeName = "decimal(18,0)")]
        public decimal? ThanhTien { get; set; }
    }
}
