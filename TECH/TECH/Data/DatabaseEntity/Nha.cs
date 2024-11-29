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

    [Table("Nha")]
    public class Nha : DomainEntity<int>
    {
        [Column(TypeName = "nvarchar(250)")]
        public string? TenNha { get; set; }
        public int? MaTP { get; set; }
        [ForeignKey("MaTP")]
        public ThanhPho? ThanhPho { get; set; }
        public int? MaQH { get; set; }
        [ForeignKey("MaQH")]
        public QuanHuyen? QuanHuyen { get; set; }
        public int? MaPX { get; set; }

        [ForeignKey("MaPX")]
        public PhuongXa? PhuongXa { get; set; }
        public int? SLPhong { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        public string? DiaChi { get; set; }
    }
}
