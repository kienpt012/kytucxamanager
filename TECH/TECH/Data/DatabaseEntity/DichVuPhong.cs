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

    [Table("DichVuPhong")]
    public class DichVuPhong : DomainEntity<int>
    {        
        public int? MaDV { get; set; }
        [ForeignKey("MaDV")]
        public DichVu? DichVu { get; set; }
        public int? MaPhong { get; set; }
        [ForeignKey("MaPhong")]
        public Phong? Phong { get; set; }
        public int? SoLuong { get; set; }

    }
}
