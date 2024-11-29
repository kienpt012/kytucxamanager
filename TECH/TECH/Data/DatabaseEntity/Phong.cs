using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;

namespace TECH.Data.DatabaseEntity
{
    [Table("Phong")]
    public class Phong : DomainEntity<int>
    {
        public int? MaNha { get; set; }

        [ForeignKey("MaNha")]
        public Nha? Nha { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        public string? TenPhong { get; set; }
        //public int? user_id { get; set; }
        [Column(TypeName = "decimal(18,0)")]
        public decimal? DonGia { get; set; }
        public int? SLNguoiMax { get; set; }
        [Column(TypeName = "float")]
        public float? ChieuDai { get; set; }
        [Column(TypeName = "float")]
        public float? ChieuRong { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        public string? MoTa { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string? HinhAnh { get; set; }
        public int? LoaiPhong { get; set; }
        public int? TinhTrang { get; set; }
        public decimal? SoDienCu { get; set; } // Chỉ số điện cũ
        public decimal? SoNuocCu { get; set; } // Chỉ số nước cũ
    }
}
