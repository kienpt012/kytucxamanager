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

    [Table("TheKiTucXa")]
    public class TheKiTucXa : DomainEntity<int>
    {
        public int? MaSinhVien { get; set; }
        [ForeignKey("MaSinhVien")]
        public KhachHang? KhachHang { get; set; }
        public int? Status { get; set; }
        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string? MaThe { get; set; }


    }
}
