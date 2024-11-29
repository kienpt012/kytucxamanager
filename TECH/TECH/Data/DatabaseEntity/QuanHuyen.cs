using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;

namespace TECH.Data.DatabaseEntity
{
    [Table("QuanHuyen")]
    public class QuanHuyen : DomainEntity<int>
    {
        public int? MaTP { get; set; }
        [ForeignKey("MaTP")]
        public ThanhPho? ThanhPho { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? Ten { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? GhiChu { get; set; }
    }
}
