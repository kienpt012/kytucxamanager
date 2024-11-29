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

    [Table("TheSinhVien")]
    public class TheSinhVien : DomainEntity<int>
    {

        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public KhachHang? KhachHang { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string? MaThe { get; set; }

        public DateTime? NgayTaoThe { get; set; }
        public DateTime? NgayHetHan { get; set; }
        public int? Status { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? Comment { get; set; }
    }
}
