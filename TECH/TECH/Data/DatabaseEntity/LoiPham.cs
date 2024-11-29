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

    [Table("LoiPham")]
    public class LoiPham : DomainEntity<int>
    {
        [Column(TypeName = "nvarchar(250)")]
        public string? TenLoi { get; set; }

        [Column(TypeName = "decimal(18,0)")]
        public decimal? TienPhat { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        public string? GhiChu { get; set; }
    }
}
