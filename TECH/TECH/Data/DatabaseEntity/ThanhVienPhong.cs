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

    [Table("ThanhVienPhong")]
    public class ThanhVienPhong : DomainEntity<int>
    {        
        public int? MaKH { get; set; }
        [ForeignKey("MaKH")]
        public KhachHang? KhachHang { get; set; }
        public int? MaPhong { get; set; }
        [ForeignKey("MaPhong")]
        public Phong? Phong { get; set; }

    }
}
