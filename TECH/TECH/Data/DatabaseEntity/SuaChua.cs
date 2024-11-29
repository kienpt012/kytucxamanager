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

    [Table("SuaChua")]
    public class SuaChua : DomainEntity<int>
    {

        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public KhachHang? KhachHang { get; set; }

        public int? MaPhong { get; set; }

        [ForeignKey("MaPhong")]
        public Phong? Phong { get; set; }

        public DateTime? NgayTao { get; set; }
        public int? Status { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? Comment { get; set; }
    }
}
