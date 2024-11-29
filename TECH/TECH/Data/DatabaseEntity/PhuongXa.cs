using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;

namespace TECH.Data.DatabaseEntity
{
    [Table("PhuongXa")]
    public class PhuongXa : DomainEntity<int>
    {
        public int? MaQH { get; set; }
        [ForeignKey("MaQH")]
        public QuanHuyen? QuanHuyen { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? Ten { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? GhiChu { get; set; }
    }
}
